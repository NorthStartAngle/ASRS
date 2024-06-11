using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LIBS
{
    /// <summary>
    /// SimpleTcp client with SSL support.  
    /// Set the Connected, Disconnected, and DataReceived events.  
    /// Once set, use Connect() to connect to the server.
    /// </summary>
    public class Connector : IDisposable
    {
        #region Public-Members
        public bool IsConnected
        {
            get
            {
                return _isConnected;
            }
            private set
            {
                _isConnected = value;
            }
        }

        public IPEndPoint LocalEndpoint
        {
            get
            {
                if (_client != null && _isConnected)
                {
                    return (IPEndPoint)_client.Client.LocalEndPoint;
                }

                return null;
            }
        }

        public ConnectorEvents Events
        {
            get
            {
                return _events;
            }
            set
            {
                if (value == null) _events = new ConnectorEvents();
                else _events = value;
            }
        }

        public string ServerIpPort
        {
            get
            {
                return $"{_serverIp}:{_serverPort}";
            }
        }

        #endregion

        #region Private-Members

        private string _serverIp = null;
        private int _serverPort = 0;
        private readonly IPAddress _ipAddress = null;
        private ConnectorEvents _events = new ConnectorEvents();
        private TcpClient _client = null;
        private NetworkStream _networkStream = null;

        private readonly SemaphoreSlim _sendLock = new SemaphoreSlim(1, 1);
        private bool _isConnected = false;

        private Task _dataReceiver = null;
        private Task _idleServerMonitor = null;
        private Task _connectionMonitor = null;
        private CancellationTokenSource _tokenSource = new CancellationTokenSource();
        private CancellationToken _token;

        private DateTime _lastActivity = DateTime.Now;
        private bool _isTimeout = false;

        public Action<string> Logger = null;
        #endregion

        public Connector(string ipPort)
        {
            if (string.IsNullOrEmpty(ipPort)) throw new ArgumentNullException(nameof(ipPort));

            Common.ParseIpPort(ipPort, out _serverIp, out _serverPort);
            if (_serverPort < 0) throw new ArgumentException("Port must be zero or greater.");
            if (string.IsNullOrEmpty(_serverIp)) throw new ArgumentNullException("Server IP or hostname must not be null.");

            if (!IPAddress.TryParse(_serverIp, out _ipAddress))
            {
                _ipAddress = Dns.GetHostEntry(_serverIp).AddressList[0];
                _serverIp = _ipAddress.ToString();
            }
        }

        public Connector(string serverIpOrHostname, int port)
        {
            if (string.IsNullOrEmpty(serverIpOrHostname)) throw new ArgumentNullException(nameof(serverIpOrHostname));
            if (port < 0) throw new ArgumentException("Port must be zero or greater.");

            _serverIp = serverIpOrHostname;
            _serverPort = port;

            if (!IPAddress.TryParse(_serverIp, out _ipAddress))
            {
                _ipAddress = Dns.GetHostEntry(serverIpOrHostname).AddressList[0];
                _serverIp = _ipAddress.ToString();
            }

            Logger = Console.WriteLine;
        }

        public Connector(IPAddress serverIpAddress, int port) : this(new IPEndPoint(serverIpAddress, port))
        {
        }

        public Connector(IPEndPoint serverIpEndPoint)
        {
            if (serverIpEndPoint == null) throw new ArgumentNullException(nameof(serverIpEndPoint));
            else if (serverIpEndPoint.Port < 0) throw new ArgumentException("Port must be zero or greater.");
            else
            {
                _ipAddress = serverIpEndPoint.Address;
                _serverIp = serverIpEndPoint.Address.ToString();
                _serverPort = serverIpEndPoint.Port;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Connect()
        {
            if (IsConnected)
            {
                return;
            }
            else
            {
                InitializeClient();
            }
            
            _tokenSource = new CancellationTokenSource();
            _token = _tokenSource.Token;
            _token.Register(() =>
            {
                if (_networkStream == null) return;
                _networkStream.Close();
            });

            IAsyncResult ar = _client.BeginConnect(_serverIp, _serverPort, null, null);
            WaitHandle wh = ar.AsyncWaitHandle;

            try
            {
                if (!ar.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(3000), false))
                {
                    _client.Close();
                    throw new TimeoutException($"Timeout connecting to {ServerIpPort}");
                }

                _client.EndConnect(ar);
                _networkStream = _client.GetStream();
                _networkStream.ReadTimeout = 300;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            _isConnected = true;
            _lastActivity = DateTime.Now;
            _isTimeout = false;
            _events.HandleConnected(this, new ConnectionEventArgs(ServerIpPort));
            _dataReceiver = Task.Run(() => DataReceiver(_token), _token);
            //_idleServerMonitor = Task.Run(IdleServerMonitor, _token);
            _connectionMonitor = Task.Run(ConnectedMonitor, _token);
        }
        
        public void ConnectWithRetries(int? timeoutMs = 1)
        {
            if (timeoutMs != null && timeoutMs < 1)
            {
                Connect();return;
            }

            if (IsConnected)
            {
                return;
            }
            else
            {
                InitializeClient();
            }
            
            _tokenSource = new CancellationTokenSource();
            _token = _tokenSource.Token;
            _token.Register(() =>
            {
                if (_networkStream == null) return;
                _networkStream.Close();
            });


            using (CancellationTokenSource connectTokenSource = new CancellationTokenSource())
            {
                CancellationToken connectToken = connectTokenSource.Token;

                Task cancelTask = Task.Delay(3000, _token);
                Task connectTask = Task.Run(() =>
                {
                    int retryCount = 0;

                    while (true)
                    {
                        try
                        {
                            _client.Dispose();
                            _client = new TcpClient();
                            _client.NoDelay = true;
                            _client.ConnectAsync(_serverIp, _serverPort).Wait(1000, connectToken);
                            Logger?.Invoke($"{retryCount} times trying");
                            if (_client.Connected)
                            {
                                break;
                            }
                        }
                        catch (TaskCanceledException)
                        {
                            break;
                        }
                        catch (OperationCanceledException)
                        {
                            break;
                        }
                        catch (Exception e)
                        {

                        }
                        finally
                        {
                            retryCount++;
                        }
                    }
                }, connectToken);

                Task.WhenAny(cancelTask, connectTask).Wait();

                if (cancelTask.IsCompleted)
                {
                    connectTokenSource.Cancel();
                    _client.Close();
                    throw new TimeoutException($"Timeout connecting to {ServerIpPort}");
                }

                try
                {
                    _networkStream = _client.GetStream();
                    _networkStream.ReadTimeout = 300;

                }
                catch (Exception)
                {
                    throw;
                }

            }

            _isConnected = true;
            _lastActivity = DateTime.Now;
            _isTimeout = false;
            _events.HandleConnected(this, new ConnectionEventArgs(ServerIpPort));
            _dataReceiver = Task.Run(() => DataReceiver(_token), _token);
            _idleServerMonitor = Task.Run(IdleServerMonitor, _token);
            _connectionMonitor = Task.Run(ConnectedMonitor, _token);
        }

        private void InitializeClient()
        {
            _client = new TcpClient();
            _client.NoDelay = true;
        }

        public void Disconnect()
        {
            if (!IsConnected)
            {
                return;
            }
            Logger?.Invoke($"disconnecting from {ServerIpPort}");

            _tokenSource.Cancel();
            WaitCompletion();
            _client.Close();
            _isConnected = false;
        }

        public async Task DisconnectAsync()
        {
            if (!IsConnected)
            {
                return;
            }

            _tokenSource.Cancel();
            await WaitCompletionAsync();
            _client.Close();
            _isConnected = false;
        }

        public void Send(string data)
        {
            if (string.IsNullOrEmpty(data)) throw new ArgumentNullException(nameof(data));
            if (!_isConnected) throw new IOException("Not connected to the server; use Connect() first.");

            byte[] bytes = Encoding.UTF8.GetBytes(data);
            this.Send(bytes);
        }

        public void Send(byte[] data)
        {
            if (data == null || data.Length < 1) throw new ArgumentNullException(nameof(data));
            if (!_isConnected) throw new IOException("Not connected to the server; use Connect() first.");

            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(data, 0, data.Length);
                ms.Seek(0, SeekOrigin.Begin);
                SendInternal(data.Length, ms);
            }
        }

        public void Send(long contentLength, Stream stream)
        {
            if (contentLength < 1) return;
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (!stream.CanRead) throw new InvalidOperationException("Cannot read from supplied stream.");
            if (!_isConnected) throw new IOException("Not connected to the server; use Connect() first.");

            SendInternal(contentLength, stream);
        }

        public async Task SendAsync(string data, CancellationToken token = default)
        {
            if (string.IsNullOrEmpty(data)) throw new ArgumentNullException(nameof(data));
            if (!_isConnected) throw new IOException("Not connected to the server; use Connect() first.");
            if (token == default(CancellationToken)) token = _token;

            byte[] bytes = Encoding.UTF8.GetBytes(data);

            using (MemoryStream ms = new MemoryStream())
            {
                await ms.WriteAsync(bytes, 0, bytes.Length, token).ConfigureAwait(false);
                ms.Seek(0, SeekOrigin.Begin);
                await SendInternalAsync(bytes.Length, ms, token).ConfigureAwait(false);
            }
        }

        public async Task SendAsync(byte[] data, CancellationToken token = default)
        {
            if (data == null || data.Length < 1) throw new ArgumentNullException(nameof(data));
            if (!_isConnected) throw new IOException("Not connected to the server; use Connect() first.");
            if (token == default(CancellationToken)) token = _token;

            using (MemoryStream ms = new MemoryStream())
            {
                await ms.WriteAsync(data, 0, data.Length, token).ConfigureAwait(false);
                ms.Seek(0, SeekOrigin.Begin);
                await SendInternalAsync(data.Length, ms, token).ConfigureAwait(false);
            }
        }

        public async Task SendAsync(long contentLength, Stream stream, CancellationToken token = default)
        {
            if (contentLength < 1) return;
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (!stream.CanRead) throw new InvalidOperationException("Cannot read from supplied stream.");
            if (!_isConnected) throw new IOException("Not connected to the server; use Connect() first.");
            if (token == default(CancellationToken)) token = _token;

            await SendInternalAsync(contentLength, stream, token).ConfigureAwait(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _isConnected = false;

                if (_tokenSource != null)
                {
                    if (!_tokenSource.IsCancellationRequested)
                    {
                        _tokenSource.Cancel();
                        _tokenSource.Dispose();
                    }
                }

                if (_networkStream != null)
                {
                    _networkStream.Close();
                    _networkStream.Dispose();
                }

                if (_client != null)
                {
                    _client.Close();
                    _client.Dispose();
                }
            }
        }

        private async Task DataReceiver(CancellationToken token)
        {
            Stream outerStream = null;
            outerStream = _networkStream;
            
            while (!token.IsCancellationRequested && _client != null && _client.Connected)
            {
                try
                {
                    await DataReadAsync(token).ContinueWith(async task =>
                    {
                        if (task.IsCanceled) return default;
                        var data = task.Result;

                        if (data != null)
                        {
                            _lastActivity = DateTime.Now;

                            Action action = () => _events.HandleDataReceived(this, new DataReceivedEventArgs(ServerIpPort, data));
                            if (true)//Async Data Receive
                            {
                                _ = Task.Run(action, token);
                            }
                            else
                            {
                                action.Invoke();
                            }

                            //_statistics.ReceivedBytes += data.Count;

                            return data;
                        }
                        else
                        {
                            await Task.Delay(100).ConfigureAwait(false);
                            return default;
                        }

                    }, token).ContinueWith(task => { }).ConfigureAwait(false);
                }
                catch (AggregateException)
                {
                    Logger?.Invoke($"data receiver canceled, disconnected");
                    break;
                }
                catch (IOException)
                {
                    Logger?.Invoke($"data receiver canceled, disconnected");
                    break;
                }
                catch (SocketException)
                {
                    Logger?.Invoke($"data receiver canceled, disconnected");
                    break;
                }
                catch (TaskCanceledException)
                {
                    Logger?.Invoke($"data receiver task canceled, disconnected");
                    break;
                }
                catch (OperationCanceledException)
                {
                    Logger?.Invoke($"data receiver operation canceled, disconnected");
                    break;
                }
                catch (ObjectDisposedException)
                {
                    Logger?.Invoke($"data receiver canceled due to disposal, disconnected");
                    break;
                }
                catch (Exception e)
                {
                    Logger?.Invoke($"data receiver exception:{Environment.NewLine}{e}{Environment.NewLine}");
                    break;
                }
            }

           Logger?.Invoke($"disconnection detected");

            _isConnected = false;

            if (!_isTimeout) _events.HandleClientDisconnected(this, new ConnectionEventArgs(ServerIpPort, DisconnectReason.Normal));
            else _events.HandleClientDisconnected(this, new ConnectionEventArgs(ServerIpPort, DisconnectReason.Timeout));

            Dispose();
        }

        private async Task<ArraySegment<byte>> DataReadAsync(CancellationToken token)
        {
            byte[] buffer = new byte[65536]; //buffer size
            int read = 0;

            try
            {
                read = await _networkStream.ReadAsync(buffer, 0, buffer.Length, token).ConfigureAwait(false);

                if (read > 0)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        ms.Write(buffer, 0, read);
                        return new ArraySegment<byte>(ms.GetBuffer(), 0, (int)ms.Length);
                    }
                }
                else
                {
                    IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
                    TcpConnectionInformation[] tcpConnections = ipProperties.GetActiveTcpConnections()
                        .Where(x => x.LocalEndPoint.Equals(this._client.Client.LocalEndPoint) && x.RemoteEndPoint.Equals(this._client.Client.RemoteEndPoint)).ToArray();

                    var isOk = false;

                    if (tcpConnections != null && tcpConnections.Length > 0)
                    {
                        TcpState stateOfConnection = tcpConnections.First().State;
                        if (stateOfConnection == TcpState.Established)
                        {
                            isOk = true;
                        }
                    }

                    if (!isOk)
                    {
                        await this.DisconnectAsync();
                    }

                    throw new SocketException();
                }
            }
            catch (IOException)
            {
                // thrown if ReadTimeout (ms) is exceeded
                // see https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.networkstream.readtimeout?view=net-6.0
                // and https://github.com/dotnet/runtime/issues/24093
                return default;
            }
        }

        private void SendInternal(long contentLength, Stream stream)
        {
            long bytesRemaining = contentLength;
            int bytesRead = 0;
            byte[] buffer = new byte[65536]; // bufer size

            try
            {
                _sendLock.Wait();

                while (bytesRemaining > 0)
                {
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead > 0)
                    {
                        _networkStream.Write(buffer, 0, bytesRead);

                        bytesRemaining -= bytesRead;
                        //_statistics.SentBytes += bytesRead;
                    }
                }

                _networkStream.Flush();
                _events.HandleDataSent(this, new DataSentEventArgs(ServerIpPort, contentLength));
            }
            finally
            {
                _sendLock.Release();
            }
        }

        private async Task SendInternalAsync(long contentLength, Stream stream, CancellationToken token)
        {
            try
            {
                long bytesRemaining = contentLength;
                int bytesRead = 0;
                byte[] buffer = new byte[65536]; //buf size

                await _sendLock.WaitAsync(token).ConfigureAwait(false);

                while (bytesRemaining > 0)
                {
                    bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, token).ConfigureAwait(false);
                    if (bytesRead > 0)
                    {
                        await _networkStream.WriteAsync(buffer, 0, bytesRead, token).ConfigureAwait(false);

                        bytesRemaining -= bytesRead;
                        //_statistics.SentBytes += bytesRead;
                    }
                }

                await _networkStream.FlushAsync(token).ConfigureAwait(false);
                _events.HandleDataSent(this, new DataSentEventArgs(ServerIpPort, contentLength));
            }
            catch (TaskCanceledException)
            {

            }
            catch (OperationCanceledException)
            {

            }
            finally
            {
                _sendLock.Release();
            }
        }

        private void WaitCompletion()
        {
            try
            {
                _dataReceiver.Wait();
            }
            catch (AggregateException ex) when (ex.InnerException is TaskCanceledException)
            {
                Logger?.Invoke("Awaiting a canceled task");
            }
        }

        private async Task WaitCompletionAsync()
        {
            try
            {
                await _dataReceiver;
            }
            catch (TaskCanceledException)
            {
                Logger?.Invoke("Awaiting a canceled task");
            }
        }

        private void EnableKeepalives()
        {
            // issues with definitions: https://github.com/dotnet/sdk/issues/14540

            try
            {
#if NETCOREAPP3_1_OR_GREATER || NET6_0_OR_GREATER

                // NETCOREAPP3_1_OR_GREATER catches .NET 5.0

                _client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                _client.Client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveTime, _keepalive.TcpKeepAliveTime);
                _client.Client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveInterval, _keepalive.TcpKeepAliveInterval);

                // Windows 10 version 1703 or later

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                    && Environment.OSVersion.Version >= new Version(10, 0, 15063))
                {
                    _client.Client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveRetryCount, _keepalive.TcpKeepAliveRetryCount);
                }

#elif NETFRAMEWORK

                byte[] keepAlive = new byte[12];

                // Turn keepalive on
                Buffer.BlockCopy(BitConverter.GetBytes((uint)1), 0, keepAlive, 0, 4);

                // Set TCP keepalive time
                Buffer.BlockCopy(BitConverter.GetBytes((uint)_keepalive.TcpKeepAliveTimeMilliseconds), 0, keepAlive, 4, 4);

                // Set TCP keepalive interval
                Buffer.BlockCopy(BitConverter.GetBytes((uint)_keepalive.TcpKeepAliveIntervalMilliseconds), 0, keepAlive, 8, 4);

                // Set keepalive settings on the underlying Socket
                _client.Client.IOControl(IOControlCode.KeepAliveValues, keepAlive, null);

#elif NETSTANDARD

#endif
            }
            catch (Exception)
            {
            }
        }

        private async Task IdleServerMonitor()
        {
            while (!_token.IsCancellationRequested)
            {
                await Task.Delay(300, _token).ConfigureAwait(false);

                DateTime timeoutTime = _lastActivity.AddMilliseconds(100);

                if (DateTime.Now > timeoutTime)
                {
                    Logger?.Invoke($"disconnecting from {ServerIpPort} due to timeout");
                    _isConnected = false;
                    _isTimeout = true;
                    _tokenSource.Cancel(); // DataReceiver will fire events including dispose
                }
            }
        }

        private async Task ConnectedMonitor()
        {
            while (!_token.IsCancellationRequested)
            {
                await Task.Delay(1000, _token).ConfigureAwait(false);

                if (!_isConnected)
                    continue; //Just monitor connected clients

                if (!PollSocket())
                {
                    Logger?.Invoke($"disconnecting from {ServerIpPort} due to connection lost");
                    _isConnected = false;
                    _tokenSource.Cancel(); // DataReceiver will fire events including dispose
                }
            }
        }

        private bool PollSocket()
        {
            try
            {
                if (_client.Client == null || !_client.Client.Connected)
                    return false;

                if (!_client.Client.Poll(0, SelectMode.SelectRead))
                    return true;

                var buff = new byte[1];
                var clientSentData = _client.Client.Receive(buff, SocketFlags.Peek) != 0;
                return clientSentData; //False here though Poll() succeeded means we had a disconnect!
            }
            catch (SocketException ex)
            {
                Logger?.Invoke($"poll socket from {ServerIpPort} failed with ex = {ex}");
                return ex.SocketErrorCode == SocketError.TimedOut;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}