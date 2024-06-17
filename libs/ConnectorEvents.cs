using System;
using System.Collections.Generic;

namespace LIBS
{
    public class MSGArgs : EventArgs
    {
        internal MSGArgs(MSG content, ResponseMSG response)
        {
            Content = content;
            Respoonse = response;
        }

        public MSG Content { get; }
        public ResponseMSG Respoonse { get; }
    }

    public class Events : ConnectorEvents
    {
        public event EventHandler<RTS> StatusChanged;
		public event EventHandler<RTK> WTK_RecvResult;
		

        internal void HandleRecvWTK(object sender, RTK args)
        {
            WTK_RecvResult?.Invoke(sender, args);
        }
		
		internal void HandleStatusChanged(object sender, RTS args)
        {
            StatusChanged?.Invoke(sender, args);
        }	

    }

    /// <summary>
    /// SimpleTcp client events.
    /// </summary>
    public class ConnectorEvents
    {
        #region Public-Members

        /// <summary>
        /// Event to call when the connection is established.
        /// </summary>
        public event EventHandler<ConnectionEventArgs> Connected;

        /// <summary>
        /// Event to call when the connection is destroyed.
        /// </summary>
        public event EventHandler<ConnectionEventArgs> Disconnected;

        /// <summary>
        /// Event to call when byte data has become available from the server.
        /// </summary>
        public event EventHandler<DataReceivedEventArgs> DataReceived;

        /// <summary>
        /// Event to call when byte data has been sent to the server.
        /// </summary>
        public event EventHandler<DataSentEventArgs> DataSent;

		public event EventHandler<Exception> ErrorRecepted;
		
        #endregion

        #region Constructors-and-Factories

        /// <summary>
        /// Instantiate the object.
        /// </summary>
        public ConnectorEvents()
        {

        }

        #endregion

        #region Public-Methods

        internal void HandleConnected(object sender, ConnectionEventArgs args)
        {
            Connected?.Invoke(sender, args);
        }

        internal void HandleClientDisconnected(object sender, ConnectionEventArgs args)
        {
            Disconnected?.Invoke(sender, args);
        }

        internal void HandleDataReceived(object sender, DataReceivedEventArgs args)
        {
            DataReceived?.Invoke(sender, args);
        }

        internal void HandleDataSent(object sender, DataSentEventArgs args)
        {
            DataSent?.Invoke(sender, args);
        }
		
		internal void HandleErrorOccured(object sender, Exception args)
        {
            ErrorRecepted?.Invoke(sender, args);
        }

        #endregion
    }
}
