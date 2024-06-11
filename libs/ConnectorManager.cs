using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Security;
using System.Net.Sockets;
using System.Net;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LIBS
{
    public class ConnectorManager : IDisposable
    {
        #region Public-Members

        public bool IsListening
        {
            get
            {
                return _isListening;
            }
        }

        public ConnectorManagerEvents Events
        {
            get
            {
                return _events;
            }
            set
            {
                if (value == null) _events = new ConnectorManagerEvents();
                else _events = value;
            }
        }

        public int Connections
        {
            get
            {
                return _clients.Count;
            }
        }

        public IPAddress IpAddress
        {
            get
            {
                return _ipAddress;
            }
        }

        public EndPoint Endpoint
        {
            get
            {
                return _listener == null ? null : ((IPEndPoint)_listener.LocalEndpoint);
            }
        }

        public int Port
        {
            get
            {
                return _listener == null ? 0 : ((IPEndPoint)_listener.LocalEndpoint).Port;
            }
        }

        public Action<string> Logger = null;

        #endregion

        #region Private-Members

        private readonly string _header = "[SimpleTcp.Server] ";
        private ConnectorManagerEvents _events = new ConnectorManagerEvents();

        private readonly string _listenerIp = null;
        private readonly IPAddress _ipAddress = null;
        private readonly int _port = 0;
        private readonly bool _ssl = false;

        private readonly ConcurrentDictionary<string, ConnectorMetadata> _clients = new ConcurrentDictionary<string, ConnectorMetadata>();
        private readonly ConcurrentDictionary<string, DateTime> _clientsLastSeen = new ConcurrentDictionary<string, DateTime>();
        private readonly ConcurrentDictionary<string, DateTime> _clientsKicked = new ConcurrentDictionary<string, DateTime>();
        private readonly ConcurrentDictionary<string, DateTime> _clientsTimedout = new ConcurrentDictionary<string, DateTime>();

        private TcpListener _listener = null;
        private bool _isListening = false;

        private CancellationTokenSource _tokenSource = new CancellationTokenSource();
        private CancellationToken _token;
        private CancellationTokenSource _listenerTokenSource = new CancellationTokenSource();
        private CancellationToken _listenerToken;
        private Task _acceptConnections = null;
        private Task _idleClientMonitor = null;

        #endregion

        #region Constructors-and-Factories

        /// <param name="ipPort">The IP:port of the server.</param> 
        public ConnectorManager(string ipPort)
        {
            if (string.IsNullOrEmpty(ipPort)) throw new ArgumentNullException(nameof(ipPort));

            Common.ParseIpPort(ipPort, out _listenerIp, out _port);

            if (_port < 0) throw new ArgumentException("Port must be zero or greater.");
            if (string.IsNullOrEmpty(_listenerIp))
            {
                _ipAddress = IPAddress.Loopback;
                _listenerIp = _ipAddress.ToString();
            }
            else if (_listenerIp == "*" || _listenerIp == "+")
            {
                _ipAddress = IPAddress.Any;
            }
            else
            {
                if (!IPAddress.TryParse(_listenerIp, out _ipAddress))
                {
                    _ipAddress = Dns.GetHostEntry(_listenerIp).AddressList[0];
                    _listenerIp = _ipAddress.ToString();
                }
            }

            _isListening = false;
            _token = _tokenSource.Token;
        }

        public ConnectorManager(string listenerIp, int port)
        {
            if (port < 0) throw new ArgumentException("Port must be zero or greater.");

            _listenerIp = listenerIp;
            _port = port;

            if (string.IsNullOrEmpty(_listenerIp))
            {
                _ipAddress = IPAddress.Loopback;
                _listenerIp = _ipAddress.ToString();
            }
            else if (_listenerIp == "*" || _listenerIp == "+")
            {
                _ipAddress = IPAddress.Any;
                _listenerIp = listenerIp;
            }
            else
            {
                if (!IPAddress.TryParse(_listenerIp, out _ipAddress))
                {
                    _ipAddress = Dns.GetHostEntry(listenerIp).AddressList[0];
                    _listenerIp = _ipAddress.ToString();
                }
            }

            _isListening = false;
            _token = _tokenSource.Token;

            Logger = Console.WriteLine;
        }

        #endregion

        #region Public-Methods

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Start()
        {
            if (_isListening) throw new InvalidOperationException("ConnectorManager is already running.");

            _listener = new TcpListener(_ipAddress, _port);
            _listener.Server.NoDelay = true;
            _listener.Start();
            _isListening = true;

            _tokenSource = new CancellationTokenSource();
            _token = _tokenSource.Token;
            _listenerTokenSource = new CancellationTokenSource();
            _listenerToken = _listenerTokenSource.Token;

            if (_idleClientMonitor == null)
            {
                _idleClientMonitor = Task.Run(() => IdleClientMonitor(), _token);
            }

            _acceptConnections = Task.Run(() => AcceptConnections(), _listenerToken);
        }

        public Task StartAsync()
        {
            if (_isListening) throw new InvalidOperationException("ConnectorManager is already running.");

            _listener = new TcpListener(_ipAddress, _port);

            _listener.Start();
            _isListening = true;

            _tokenSource = new CancellationTokenSource();
            _token = _tokenSource.Token;
            _listenerTokenSource = new CancellationTokenSource();
            _listenerToken = _listenerTokenSource.Token;

            if (_idleClientMonitor == null)
            {
                _idleClientMonitor = Task.Run(() => IdleClientMonitor(), _token);
            }

            _acceptConnections = Task.Run(() => AcceptConnections(), _listenerToken);
            return _acceptConnections;
        }

        public void Stop()
        {
            if (!_isListening) throw new InvalidOperationException("ConnectorManager is not running.");

            _isListening = false;
            _listener.Stop();
            _listenerTokenSource.Cancel();
            _acceptConnections.Wait();
            _acceptConnections = null;

            Logger?.Invoke($"{_header}stopped");
        }

        public IEnumerable<string> GetClients()
        {
            List<string> clients = new List<string>(_clients.Keys);
            return clients;
        }

        public bool IsConnected(string ipPort)
        {
            if (string.IsNullOrEmpty(ipPort)) throw new ArgumentNullException(nameof(ipPort));

            return (_clients.TryGetValue(ipPort, out _));
        }

        public void Send(string ipPort, string data)
        {
            if (string.IsNullOrEmpty(ipPort)) throw new ArgumentNullException(nameof(ipPort));
            if (string.IsNullOrEmpty(data)) throw new ArgumentNullException(nameof(data));
            byte[] bytes = Encoding.UTF8.GetBytes(data);

            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(bytes, 0, bytes.Length);
                ms.Seek(0, SeekOrigin.Begin);
                SendInternal(ipPort, bytes.Length, ms);
            }
        }

        public void Send(string ipPort, byte[] data)
        {
            if (string.IsNullOrEmpty(ipPort)) throw new ArgumentNullException(nameof(ipPort));
            if (data == null || data.Length < 1) throw new ArgumentNullException(nameof(data));

            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(data, 0, data.Length);
                ms.Seek(0, SeekOrigin.Begin);
                SendInternal(ipPort, data.Length, ms);
            }
        }

        public void Send(string ipPort, long contentLength, Stream stream)
        {
            if (string.IsNullOrEmpty(ipPort)) throw new ArgumentNullException(nameof(ipPort));
            if (contentLength < 1) return;
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (!stream.CanRead) throw new InvalidOperationException("Cannot read from supplied stream.");

            SendInternal(ipPort, contentLength, stream);
        }

        public async Task SendAsync(string ipPort, string data, CancellationToken token = default)
        {
            if (string.IsNullOrEmpty(ipPort)) throw new ArgumentNullException(nameof(ipPort));
            if (string.IsNullOrEmpty(data)) throw new ArgumentNullException(nameof(data));
            if (token == default(CancellationToken)) token = _token;

            byte[] bytes = Encoding.UTF8.GetBytes(data);
            using (MemoryStream ms = new MemoryStream())
            {
                await ms.WriteAsync(bytes, 0, bytes.Length, token).ConfigureAwait(false);
                ms.Seek(0, SeekOrigin.Begin);
                await SendInternalAsync(ipPort, bytes.Length, ms, token).ConfigureAwait(false);
            }
        }

        public async Task SendAsync(string ipPort, byte[] data, CancellationToken token = default)
        {
            if (string.IsNullOrEmpty(ipPort)) throw new ArgumentNullException(nameof(ipPort));
            if (data == null || data.Length < 1) throw new ArgumentNullException(nameof(data));
            if (token == default(CancellationToken)) token = _token;

            using (MemoryStream ms = new MemoryStream())
            {
                await ms.WriteAsync(data, 0, data.Length, token).ConfigureAwait(false);
                ms.Seek(0, SeekOrigin.Begin);
                await SendInternalAsync(ipPort, data.Length, ms, token).ConfigureAwait(false);
            }
        }

        public async Task SendAsync(string ipPort, long contentLength, Stream stream, CancellationToken token = default)
        {
            if (string.IsNullOrEmpty(ipPort)) throw new ArgumentNullException(nameof(ipPort));
            if (contentLength < 1) return;
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (!stream.CanRead) throw new InvalidOperationException("Cannot read from supplied stream.");
            if (token == default(CancellationToken)) token = _token;

            await SendInternalAsync(ipPort, contentLength, stream, token).ConfigureAwait(false);
        }

        public void DisconnectClient(string ipPort)
        {
            if (string.IsNullOrEmpty(ipPort)) throw new ArgumentNullException(nameof(ipPort));

            if (!_clients.TryGetValue(ipPort, out ConnectorMetadata client))
            {
                Logger?.Invoke($"{_header}unable to find client: {ipPort}");
            }
            else
            {
                if (!_clientsTimedout.ContainsKey(ipPort))
                {
                    Logger?.Invoke($"{_header}kicking: {ipPort}");
                    _clientsKicked.TryAdd(ipPort, DateTime.Now);
                }
            }

            if (client != null)
            {
                if (!client.TokenSource.IsCancellationRequested)
                {
                    client.TokenSource.Cancel();
                    Logger?.Invoke($"{_header}requesting disposal of: {ipPort}");
                }

                client.Dispose();
            }
        }

        #endregion

        #region Private-Methods

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    if (_clients != null && _clients.Count > 0)
                    {
                        foreach (KeyValuePair<string, ConnectorMetadata> curr in _clients)
                        {
                            curr.Value.Dispose();
                            Logger?.Invoke($"{_header}disconnected client: {curr.Key}");
                        }
                    }

                    if (_tokenSource != null)
                    {
                        if (!_tokenSource.IsCancellationRequested)
                        {
                            _tokenSource.Cancel();
                        }

                        _tokenSource.Dispose();
                    }

                    if (_listener != null && _listener.Server != null)
                    {
                        _listener.Server.Close();
                        _listener.Server.Dispose();
                    }

                    if (_listener != null)
                    {
                        _listener.Stop();
                    }
                }
                catch (Exception e)
                {
                    Logger?.Invoke($"{_header}dispose exception:{Environment.NewLine}{e}{Environment.NewLine}");
                }

                _isListening = false;

                Logger?.Invoke($"{_header}disposed");
            }
        }

        private bool IsClientConnected(TcpClient client)
        {
            if (client == null) return false;

            var state = IPGlobalProperties.GetIPGlobalProperties()
                .GetActiveTcpConnections()
                    .FirstOrDefault(x =>
                        x.LocalEndPoint.Equals(client.Client.LocalEndPoint)
                        && x.RemoteEndPoint.Equals(client.Client.RemoteEndPoint));

            if (state == default(TcpConnectionInformation)
                || state.State == TcpState.Unknown
                || state.State == TcpState.FinWait1
                || state.State == TcpState.FinWait2
                || state.State == TcpState.Closed
                || state.State == TcpState.Closing
                || state.State == TcpState.CloseWait)
            {
                return false;
            }

            if ((client.Client.Poll(0, SelectMode.SelectWrite)) && (!client.Client.Poll(0, SelectMode.SelectError)))
            {
                byte[] buffer = new byte[1];
                if (client.Client.Receive(buffer, SocketFlags.Peek) == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            return false;
        }

        private async Task AcceptConnections()
        {
            while (!_listenerToken.IsCancellationRequested)
            {
                ConnectorMetadata client = null;

                try
                {
                    #region Check-for-Maximum-Connections

                    if (!_isListening && (_clients.Count >= 4096))
                    {
                        Task.Delay(100).Wait();
                        continue;
                    }
                    else if (!_isListening)
                    {
                        _listener.Start();
                        _isListening = true;
                    }

                    #endregion

                    TcpClient tcpClient = await _listener.AcceptTcpClientAsync().ConfigureAwait(false);
                    string clientIpPort = tcpClient.Client.RemoteEndPoint.ToString();

                    string clientIp = null;
                    int clientPort = 0;
                    Common.ParseIpPort(clientIpPort, out clientIp, out clientPort);

                    client = new ConnectorMetadata(tcpClient);

                    _clients.TryAdd(clientIpPort, client);
                    _clientsLastSeen.TryAdd(clientIpPort, DateTime.Now);
                    Logger?.Invoke($"{_header}starting data receiver for: {clientIpPort}");
                    _events.HandleClientConnected(this, new ConnectionEventArgs(clientIpPort));


                    CancellationTokenSource linkedCts = CancellationTokenSource.CreateLinkedTokenSource(client.Token, _token);
                    Task unawaited = Task.Run(() => DataReceiver(client), linkedCts.Token);

                    #region Check-for-Maximum-Connections

                    if (_clients.Count >= 4096)
                    {
                        Logger?.Invoke($"{_header}maximum connections {4096} met (currently {_clients.Count} connections), pausing");
                        _isListening = false;
                        _listener.Stop();
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    if (ex is TaskCanceledException
                        || ex is OperationCanceledException
                        || ex is ObjectDisposedException
                        || ex is InvalidOperationException)
                    {
                        _isListening = false;
                        if (client != null) client.Dispose();
                        Logger?.Invoke($"{_header}stopped listening");
                        break;
                    }
                    else
                    {
                        if (client != null) client.Dispose();
                        Logger?.Invoke($"{_header}exception while awaiting connections: {ex}");
                        continue;
                    }
                }
            }

            _isListening = false;
        }

        private async Task DataReceiver(ConnectorMetadata client)
        {
            string ipPort = client.IpPort;
            Logger?.Invoke($"{_header}data receiver started for client {ipPort}");

            CancellationTokenSource linkedCts = CancellationTokenSource.CreateLinkedTokenSource(_token, client.Token);

            while (true)
            {
                try
                {
                    if (!IsClientConnected(client.Client))
                    {
                        Logger?.Invoke($"{_header}client {ipPort} disconnected");
                        break;
                    }

                    if (client.Token.IsCancellationRequested)
                    {
                        Logger?.Invoke($"{_header}cancellation requested (data receiver for client {ipPort})");
                        break;
                    }

                    var data = await DataReadAsync(client, linkedCts.Token).ConfigureAwait(false);
                    if (data == null)
                    {
                        await Task.Delay(10, linkedCts.Token).ConfigureAwait(false);
                        continue;
                    }

                    Action action = () => _events.HandleDataReceived(this, new DataReceivedEventArgs(ipPort, data));
                    if (true)
                    {
                        _ = Task.Run(action, linkedCts.Token);
                    }
                    else
                    {
                        action.Invoke();
                    }

                    UpdateClientLastSeen(client.IpPort);
                }
                catch (IOException)
                {
                    Logger?.Invoke($"{_header}data receiver canceled, peer disconnected [{ipPort}]");
                    break;
                }
                catch (SocketException)
                {
                    Logger?.Invoke($"{_header}data receiver canceled, peer disconnected [{ipPort}]");
                    break;
                }
                catch (TaskCanceledException)
                {
                    Logger?.Invoke($"{_header}data receiver task canceled [{ipPort}]");
                    break;
                }
                catch (ObjectDisposedException)
                {
                    Logger?.Invoke($"{_header}data receiver canceled due to disposal [{ipPort}]");
                    break;
                }
                catch (Exception e)
                {
                    Logger?.Invoke($"{_header}data receiver exception [{ipPort}]:{Environment.NewLine}{e}{Environment.NewLine}");
                    break;
                }
            }

            Logger?.Invoke($"{_header}data receiver terminated for client {ipPort}");

            if (_clientsKicked.ContainsKey(ipPort))
            {
                _events.HandleClientDisconnected(this, new ConnectionEventArgs(ipPort, DisconnectReason.Kicked));
            }
            else if (_clientsTimedout.ContainsKey(client.IpPort))
            {
                _events.HandleClientDisconnected(this, new ConnectionEventArgs(ipPort, DisconnectReason.Timeout));
            }
            else
            {
                _events.HandleClientDisconnected(this, new ConnectionEventArgs(ipPort, DisconnectReason.Normal));
            }

            _clients.TryRemove(ipPort, out _);
            _clientsLastSeen.TryRemove(ipPort, out _);
            _clientsKicked.TryRemove(ipPort, out _);
            _clientsTimedout.TryRemove(ipPort, out _);

            if (client != null) client.Dispose();
        }

        private async Task<ArraySegment<byte>> DataReadAsync(ConnectorMetadata client, CancellationToken token)
        {
            byte[] buffer = new byte[65536];
            int read = 0;

            using (MemoryStream ms = new MemoryStream())
            {
                while (true)
                {
                    read = await client.NetworkStream.ReadAsync(buffer, 0, buffer.Length, token).ConfigureAwait(false);

                    if (read > 0)
                    {
                        await ms.WriteAsync(buffer, 0, read, token).ConfigureAwait(false);
                        return new ArraySegment<byte>(ms.GetBuffer(), 0, (int)ms.Length);
                    }
                    else
                    {
                        throw new SocketException();
                    }
                }
            }
        }

        private async Task IdleClientMonitor()
        {
            while (!_token.IsCancellationRequested)
            {
                await Task.Delay(5000, _token).ConfigureAwait(false);

                try
                {
                    DateTime idleTimestamp = DateTime.Now.AddMilliseconds(-1 * 5000);

                    foreach (KeyValuePair<string, DateTime> curr in _clientsLastSeen)
                    {
                        if (curr.Value < idleTimestamp)
                        {
                            _clientsTimedout.TryAdd(curr.Key, DateTime.Now);
                            Logger?.Invoke($"{_header}disconnecting {curr.Key} due to timeout");
                            DisconnectClient(curr.Key);
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger?.Invoke($"{_header}monitor exception: {e}");
                }
            }
        }

        private void UpdateClientLastSeen(string ipPort)
        {
            if (_clientsLastSeen.ContainsKey(ipPort))
            {
                _clientsLastSeen.TryRemove(ipPort, out _);
            }

            _clientsLastSeen.TryAdd(ipPort, DateTime.Now);
        }

        private void SendInternal(string ipPort, long contentLength, Stream stream)
        {
            if (!_clients.TryGetValue(ipPort, out ConnectorMetadata client)) return;
            if (client == null) return;

            long bytesRemaining = contentLength;
            int bytesRead = 0;
            byte[] buffer = new byte[65536];

            try
            {
                client.SendLock.Wait();

                while (bytesRemaining > 0)
                {
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead > 0)
                    {
                        client.NetworkStream.Write(buffer, 0, bytesRead);

                        bytesRemaining -= bytesRead;
                    }
                }

                client.NetworkStream.Flush();
                _events.HandleDataSent(this, new DataSentEventArgs(ipPort, contentLength));
            }
            finally
            {
                if (client != null) client.SendLock.Release();
            }
        }

        private async Task SendInternalAsync(string ipPort, long contentLength, Stream stream, CancellationToken token)
        {
            ConnectorMetadata client = null;

            try
            {
                if (!_clients.TryGetValue(ipPort, out client)) return;
                if (client == null) return;

                long bytesRemaining = contentLength;
                int bytesRead = 0;
                byte[] buffer = new byte[65536];

                await client.SendLock.WaitAsync(token).ConfigureAwait(false);

                while (bytesRemaining > 0)
                {
                    bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, token).ConfigureAwait(false);
                    if (bytesRead > 0)
                    {
                        await client.NetworkStream.WriteAsync(buffer, 0, bytesRead, token).ConfigureAwait(false);

                        bytesRemaining -= bytesRead;
                    }
                }

                await client.NetworkStream.FlushAsync(token).ConfigureAwait(false);
                _events.HandleDataSent(this, new DataSentEventArgs(ipPort, contentLength));
            }
            catch (TaskCanceledException)
            {

            }
            catch (OperationCanceledException)
            {

            }
            finally
            {
                if (client != null) client.SendLock.Release();
            }
        }

        private void EnableKeepalives()
        {
            try
            {
#if NETCOREAPP3_1_OR_GREATER || NET6_0_OR_GREATER

                // NETCOREAPP3_1_OR_GREATER catches .NET 5.0

                _listener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                _listener.Server.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveTime, _keepalive.TcpKeepAliveTime);
                _listener.Server.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveInterval, _keepalive.TcpKeepAliveInterval);
                _listener.Server.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveRetryCount, _keepalive.TcpKeepAliveRetryCount);

#elif NETFRAMEWORK

            byte[] keepAlive = new byte[12];

            // Turn keepalive on
            Buffer.BlockCopy(BitConverter.GetBytes((uint)1), 0, keepAlive, 0, 4);

            // Set TCP keepalive time
            Buffer.BlockCopy(BitConverter.GetBytes((uint)_keepalive.TcpKeepAliveTimeMilliseconds), 0, keepAlive, 4, 4);

            // Set TCP keepalive interval
            Buffer.BlockCopy(BitConverter.GetBytes((uint)_keepalive.TcpKeepAliveIntervalMilliseconds), 0, keepAlive, 8, 4);

            // Set keepalive settings on the underlying Socket
            _listener.Server.IOControl(IOControlCode.KeepAliveValues, keepAlive, null);

#elif NETSTANDARD

#endif
            }
            catch (Exception)
            {
                Logger?.Invoke($"{_header}keepalives not supported on this platform, disabled");
            }
        }

        private void EnableKeepalives(TcpClient client)
        {
            try
            {
#if NETCOREAPP3_1_OR_GREATER || NET6_0_OR_GREATER

                // NETCOREAPP3_1_OR_GREATER catches .NET 5.0

                client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                client.Client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveTime, _keepalive.TcpKeepAliveTime);
                client.Client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveInterval, _keepalive.TcpKeepAliveInterval);

                // Windows 10 version 1703 or later

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                    && Environment.OSVersion.Version >= new Version(10, 0, 15063))
                {
                    client.Client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveRetryCount, _keepalive.TcpKeepAliveRetryCount);
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
                client.Client.IOControl(IOControlCode.KeepAliveValues, keepAlive, null);

#elif NETSTANDARD

#endif
            }
            catch (Exception)
            {
                Logger?.Invoke($"{_header}keepalives not supported on this platform, disabled");
            }
        }

        #endregion
    }
}
