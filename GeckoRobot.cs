using LIB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ASRS
{
    public class GeckoRobot : IDisposable
    {
        protected Connector con;
        
        private String _address;
        private int _port;

        private WMSG curMSG = null;
        private CancellationTokenSource connectTokenSource = null;

        public GeckoRobot(string address,int port)
        {
            _address = address;
            _port = port;
            con = new Connector(_address, _port);

            con.Events.Connected += Connected;
            con.Events.Disconnected += Disconnected;
            con.Events.DataReceived += DataReceived;
            con.Events.DataSent += Events_DataSent;
        }
             
        public void Dispose()
        {
            con?.Dispose();
            GC.SuppressFinalize(this);
        }

        void Connected(object sender, ConnectionEventArgs e)
        {
            Console.WriteLine($"*** Server {e.IpPort} connected");
        }

        void Disconnected(object sender, ConnectionEventArgs e)
        {
            Console.WriteLine($"*** Server {e.IpPort} disconnected");
        }

        private void Events_DataSent(object sender, DataSentEventArgs e)
        {
            Console.WriteLine($"*** {e.IpPort} sent data{e.BytesSent}");
        }

        void DataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine($"[{e.IpPort}] {Encoding.UTF8.GetString(e.Data.Array, 0, e.Data.Count)}");
            Object obj = Common.parseDATA(e.Data.Array);
            if(obj != null)
            {
                if(connectTokenSource != null)
                {
                    connectTokenSource.Cancel();
                }
            }
        }

        public async Task SendDataAsync(WMSG msg)
        {
            curMSG = msg;
            await con.SendAsync(curMSG.Create());
        }

        public void SendData(WMSG msg,int replyTimeout=100)
        {
            curMSG = msg;

            CancellationToken connectToken = connectTokenSource.Token;

            Task cancelTask = Task.Delay(replyTimeout, connectToken);
            cancelTask.Wait();
            if (cancelTask.IsCompleted)
            {
                connectTokenSource.Cancel();
                connectTokenSource.Dispose();
                
                SendData(curMSG);
            }
            else
            {
                connectTokenSource.Dispose();
                connectTokenSource = null;
            }
        }
    }
}
