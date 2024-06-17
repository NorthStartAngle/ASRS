using System;
using LIBS;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Diagnostics.Tracing;

namespace ASRS
{
    public class GeckoClient : IDisposable
    {
        private Connector _conn = null;

        Task _idleMonitor = null;
        CancellationTokenSource idleTokenSource = null;
        CancellationToken _idletoken;

        RDS _statusMSG = null;
        WTK _workMSG = null;
        Events _dispatcher = new Events();
        DateTime _lastSentTime = DateTime.Now;
        DateTime _lastReceivedTime;

        bool isInterrupted = false;

        public GeckoClient() { }

        public async void connectGecko()
        {
            _workMSG = null;

            if (_conn != null && _conn.IsConnected)
            {
                await _conn.DisconnectAsync();
                _conn.Dispose();
            }
            _conn = new Connector("127.0.0.1", 8899);

            _conn.Events.Connected += Connected;
            _conn.Events.DataReceived += DataReceived;
            _conn.Events.DataSent += DataSent;
            _conn.Events.Disconnected += Disconnected;
            _conn.Events.ErrorRecepted += ErrorOccured;

            _ = Task.Run(() =>
            {
                try
                {
                    _conn.Connect();
                }
                catch (Exception ex)
                {
                    _dispatcher.HandleErrorOccured(this, ex);
                }
            });
        }

        private void ErrorOccured(object sender, Exception e)
        {
            _dispatcher.HandleErrorOccured(this, e);
        }

        private void Connected(object sender, ConnectionEventArgs e)
        {
            idleTokenSource = new CancellationTokenSource();
            _idletoken = idleTokenSource.Token;
            _idletoken.Register(() =>
            {
                try
                {
                    _idleMonitor.Dispose();
                }
                catch (Exception)
                {}
            });

            _statusMSG = new RDS();
            _idleMonitor = Task.Run(_IdleMonitor, _idletoken);
            _dispatcher.HandleConnected(sender, e);
        }

        public void setWTK(WTK msg)
        {
            if (msg == null) return;
            if(_workMSG == null || _workMSG.status == Status.Completed || _workMSG.status == Status.Error)
            {
                _workMSG = new WTK(msg);
                _workMSG.dt = DateTime.Now;
                _workMSG.status = Status.Accepting;
            }          
        }

        public bool isWTKAvaiable()
        {
            if (_workMSG == null || _workMSG.status == Status.Completed || _workMSG.status == Status.Error)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private async Task _IdleMonitor()
        {
            while (!_idletoken.IsCancellationRequested)
            {
                await Task.Delay(100, _idletoken).ConfigureAwait(false);
                if(!isInterrupted)
                {
                    DateTime dt1,dt2;
                    if(_statusMSG != null)
                    {
                        if(_statusMSG.status == Status.Accepting)
                        {
                            dt1 = _statusMSG.dt;
                        }else
                        {
                            dt1 = _statusMSG.dt.AddMilliseconds(_statusMSG.Interval);
                        }

                        if(_lastSentTime.AddMilliseconds(_statusMSG.consecutiveInterval) > dt1)
                        {
                            dt1 = DateTime.Now.AddDays(1);
                        }
                    }
                    else
                    {
                        dt1 = DateTime.Now.AddDays(1);
                    }

                    if (_workMSG != null)
                    {

                        if (_workMSG.status == Status.Accepting)
                        {
                            dt2 = _workMSG.dt;

                        }
                        else if(_workMSG.status == Status.Accept)
                        {
                            dt2 = _workMSG.dt.AddMilliseconds(_workMSG.Interval);
                        }
                        else
                        {
                            dt2 = DateTime.Now.AddDays(1);
                        }
                    }
                    else
                    {
                        dt2 = DateTime.Now.AddDays(1);
                    }

                    if(dt1 < dt2)
                    {
                        if (DateTime.Now >= dt1)
                        {
                            _conn.Send(_statusMSG.NewID().SetStatus(Status.Accept).Update(DateTime.Now).ToString());
                            _lastSentTime = DateTime.Now;
                        }        
                    }else
                    {
                        if (DateTime.Now >= dt2)
                        {
                            _conn.Send(_workMSG.NewID().SetStatus(Status.Accept).Update(DateTime.Now).ToString());
                            _lastSentTime = DateTime.Now;
                        }
                    }
                }
            }
        }

        private void DataReceived(object sender, DataReceivedEventArgs e)
        {
            byte[] data = e.Data.ToArray();
            ResponseMSG res = ResponseMSG.Parse(data);
            if(res.isCorrect)
            {
                _lastReceivedTime = DateTime.Now;

                if (_statusMSG != null)
                {
                    if(res.msgId == _statusMSG.msgId)
                    {
                        RTS rts = RTS.Parse(data);
                        if(rts.isCorrect)
                        {
                            
                            _dispatcher.HandleStatusChanged(this, rts);
                        }

                        return;
                    }
                }
                if(_workMSG != null)
                {
                    if (res.msgId == _workMSG.msgId)
                    {
                        RTK rtk = RTK.Parse(data);
                        if (rtk.isCorrect)
                        {
                            if(rtk.recvResult == 0)
                            {
                                _workMSG.status = Status.Pending;
                            }
                            else
                            {
                                _workMSG.status = Status.Error;
                            }
                            _workMSG = null;
                            _dispatcher.HandleRecvWTK(this, rtk);
                        }

                        return;
                    }
                }
            }
        }

        private void DataSent(object sender, DataSentEventArgs e)
        {
            _dispatcher.HandleDataSent(this, e);    
        }

        private void Disconnected(object sender, ConnectionEventArgs e)
        {
            try
            {
                _conn.Dispose();
                _dispatcher.HandleClientDisconnected(this, e);
            }
            catch (Exception)
            {}
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Connector Conn
        {
            get
            {
                return _conn;
            }
        }
        
        public Events dispath
        {
            get => _dispatcher;
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {

                if (_conn != null)
                {
                    _conn.Dispose();
                }
            }
        }

        static void Invoke(Action action)
        {
            action();
        }

        // Method to invoke an Action with one parameter
        static void Invoke(Action<string> action, string parameter)
        {
            action(parameter);
        }

        // Overloaded method to invoke an Action with two parameters
        static void Invoke(Action<int, string> action, int number, string text)
        {
            action(number, text);
        }
    }
}
