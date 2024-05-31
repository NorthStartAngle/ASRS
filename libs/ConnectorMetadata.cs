using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ASRS.libs
{
        internal class ConnectorMetadata : IDisposable
        {
            #region Public-Members

            internal TcpClient Client
            {
                get { return _tcpClient; }
            }

            internal NetworkStream NetworkStream
            {
                get { return _networkStream; }
            }

            internal string IpPort
            {
                get { return _ipPort; }
            }

            internal SemaphoreSlim SendLock = new SemaphoreSlim(1, 1);
            internal SemaphoreSlim ReceiveLock = new SemaphoreSlim(1, 1);

            internal CancellationTokenSource TokenSource { get; set; }

            internal CancellationToken Token { get; set; }

            #endregion

            #region Private-Members

            private TcpClient _tcpClient = null;
            private NetworkStream _networkStream = null;
            private SslStream _sslStream = null;
            private string _ipPort = null;

            #endregion

            #region Constructors-and-Factories

            internal ConnectorMetadata(System.Net.Sockets.TcpClient tcp)
            {
                if (tcp == null) throw new ArgumentNullException(nameof(tcp));

                _tcpClient = tcp;
                _networkStream = tcp.GetStream();
                _ipPort = tcp.Client.RemoteEndPoint.ToString();
                TokenSource = new CancellationTokenSource();
                Token = TokenSource.Token;
            }

            #endregion

            #region Public-Methods

            public void Dispose()
            {
                if (TokenSource != null)
                {
                    if (!TokenSource.IsCancellationRequested)
                    {
                        TokenSource.Cancel();
                        TokenSource.Dispose();
                    }
                }

                if (_networkStream != null)
                {
                    _networkStream.Close();
                }

                if (_tcpClient != null)
                {
                    _tcpClient.Close();
                    _tcpClient.Dispose();
                }

                SendLock.Dispose();
                ReceiveLock.Dispose();
            }

            #endregion
        }
}
