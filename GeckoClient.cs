using ASRS.libs;
using LIBS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ASRS
{
    public class GeckoClient : IDisposable
    {
        private Connector Gecko = null;
        private CancellationTokenSource idleTokenSource = null;
        private CancellationToken _idletoken;

        private DateTime _lastActivity = DateTime.Now;
        private Task _idleServerMonitor = null;
        private CancellationTokenSource _resendTokenSource = null;
        private CancellationToken _resendtoken;

        private WMSG _lastSentRDS = null;
        private WTK _lastSentWTK = null;
        private RTS _lastReceivedRTS = null;
        private RTK _lastReceivedRTK = null;

        private GeckoEvents _events = new GeckoEvents();

        public GeckoClient() {
            
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {               

                if (idleTokenSource != null)
                {
                    if (!idleTokenSource.IsCancellationRequested)
                    {
                        idleTokenSource.Cancel();
                        idleTokenSource.Dispose();
                    }
                }

                if (_resendTokenSource != null)
                {
                    if (!_resendTokenSource.IsCancellationRequested)
                    {
                        _resendTokenSource.Cancel();
                        _resendTokenSource.Dispose();
                    }
                }

                if (Gecko != null)
                {
                    Gecko.Dispose();
                }
            }
        }
        
        public async void GeckoSetting()
        {
            if (Gecko != null && Gecko.IsConnected)
            {
                await Gecko.DisconnectAsync();
                Gecko.Dispose();
            }
            Gecko = new Connector("127.0.0.1", 8899);
 
            Gecko.Events.Connected += Gecko_Connected;
            Gecko.Events.DataReceived += Gecko_DataReceived;
            Gecko.Events.DataSent += Gecko_DataSent;
            Gecko.Events.Disconnected += Gecko_Disconnected;

            _ = Task.Run(() =>
            {
                try
                {
                    Gecko.Connect();
                }
                catch (Exception)
                {
                    _events.HandleStatusChanged(this, 100);
                }
            });
        }

        private void Gecko_Connected(object sender, ConnectionEventArgs e)
        {
            Task.Delay(500).ContinueWith(_ => {
                Invoke((Action)(() => {
                    _events.HandleConnected(this, e);
                }));
            });


            idleTokenSource = new CancellationTokenSource();
            _idletoken = idleTokenSource.Token;
            _idletoken.Register(() =>
            {
                //_idleServerMonitor.Dispose();
            });
            _idleServerMonitor = Task.Run(Gecko_IdleMonitor, _idletoken);
            _lastActivity = DateTime.Now;
        }

        private void Gecko_Disconnected(object sender, ConnectionEventArgs e)
        {
            if (idleTokenSource != null) { idleTokenSource.Cancel(); }

            if (_resendTokenSource != null) { _resendTokenSource.Cancel(); _resendTokenSource.Dispose(); }

            Invoke((Action)(() => {
                _events.HandleClientDisconnected(this, e);
            }));
        }

        private void Gecko_DataSend(WMSG msg)
        {
            if (!Gecko.IsConnected) return;
            if (msg.GetType().Name == "WMSG")
            {
                _lastSentRDS = msg;
            }
            else if (msg.GetType().Name == "WTK")
            {
                if (_resendTokenSource != null && !_resendtoken.IsCancellationRequested)
                {
                    _resendTokenSource.Cancel();
                    _resendTokenSource.Dispose();
                }

                _lastSentWTK = (WTK)msg;

                _resendTokenSource = new CancellationTokenSource();
                _resendtoken = _resendTokenSource.Token;
                Task.Delay(5000, _resendtoken).ContinueWith(_ =>
                {
                    if (!_resendtoken.IsCancellationRequested)
                    {
                        Gecko_DataSend(_lastSentWTK.once());
                    }
                }).ConfigureAwait(false);
            }
            else
            {
                return;
            }

            Gecko.Send(msg.Create());
        }

        private void Gecko_DataSent(object sender, DataSentEventArgs e)
        {
            _events.HandleDataSent(this, e);
        }

        private void Gecko_DataReceived(object sender, DataReceivedEventArgs e)
        {
            _events.HandleDataReceived(this, e);

            _lastActivity = DateTime.Now;
            if (e.Data.ToArray().Length == 21) //RTS
            {
                RTK _rtk = RTK.Parse(e.Data.ToArray());
                if (_rtk.msgId == _lastSentWTK.msgId && _rtk.taskId == _lastSentWTK.taskId)
                {
                    _lastReceivedRTK = _rtk;
                    _resendTokenSource.Cancel();
                    _events.HandleWorkDataSent(this, new GeckoRTKArgs(_lastSentWTK.dt, _rtk));
                }
            }
            else if (e.Data.ToArray().Length > 60) // RTK
            {
                RTS _rts = RTS.Parse(e.Data.ToArray());
                if (_rts.msgId == _lastSentRDS.msgId)
                {
                    _lastReceivedRTS = _rts;
                    _events.HandleStatusDataReceived(this, new GeckoRTSArgs(_lastSentRDS.dt, _rts));
                }
            }
            else // error
            {

            }
        }

        private bool Gecko_Storage_Done()
        {
            /*if (swapLocation.Length == 0)
            { 
                _ = inventorys.Find(x => x.getLocation_RowCol() == availableLocation).setTimeIN(DateTime.Now).setSKU(pendingProduct.SKU).setProductID(pendingProduct.ProductID).setFull(true).save(Manager.db);
            }
            else
            {
                ASRS_Inventory inventory =inventorys.Find(x => x.getLocation_RowCol() == availableLocation);
                inventorys.Find(x => x.getLocation_RowCol() == swapLocation).clone(inventory).setTimeIN(DateTime.Now).setFull(true).save(Manager.db);
                inventory.clone(pendingProduct).setTimeIN(DateTime.Now).setFull(true).save(Manager.db);

                inventorys.Find(x => x.getLocation_RowCol() == reservationLocation).setFull(false).save(Manager.db);
            }
*/
            return true;
        }

        private async Task Gecko_IdleMonitor()
        {
            while (!_idletoken.IsCancellationRequested)
            {
                await Task.Delay(1000, _idletoken).ConfigureAwait(false);

                DateTime timeoutTime = _lastActivity.AddMilliseconds(5000);

                if (DateTime.Now > timeoutTime)
                {
                    int s = 0;
                    if (_lastSentRDS == null)
                    {
                        Gecko_DataSend(new WMSG());                        
                        s = 0;
                    }
                    else if (_lastSentRDS.msgId == (_lastReceivedRTS == null ? 0 : _lastReceivedRTS.msgId))
                    {
                        Gecko_DataSend(new WMSG());
                        s = 0;
                    }
                    else
                    {
                        WMSG msg = new WMSG() { dt = _lastSentRDS.dt };
                        Gecko_DataSend(msg);
                        s = 1;
                    }

                    if (_lastSentWTK == null)
                    {
                        s += 5;
                    }
                    else if (_lastSentWTK.msgId == (_lastReceivedRTK == null ? 0 : _lastReceivedRTK.msgId))
                    {
                        s += 5;
                    }
                    else
                    {
                        s += 6;
                    }
                    _events.HandleStatusChanged(this, s);
                    _lastActivity = DateTime.Now;
                }
            }
        }
      
        public GeckoEvents Events
        {
            get
            {
                return _events;
            }
            set
            {
                if (value == null) _events = new GeckoEvents();
                else _events = value;
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
