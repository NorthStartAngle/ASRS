using LIBS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASRS
{
    public class Supervisor
    {
        private ConnectorManager conn;

        public Supervisor() {
            conn = new ConnectorManager("127.0.0.1:9000");

            conn.Events.ClientConnected += ClientConnected;
            conn.Events.ClientDisconnected += ClientDisconnected;
            conn.Events.DataReceived += DataReceived;

            conn.Start();

            conn.Send("[ClientIp:Port]", "Hello, world!");
        }

        public void ClientConnected(object sender, ConnectionEventArgs e)
        {
            Console.WriteLine($"[{e.IpPort}] client connected");
        }

        public void ClientDisconnected(object sender, ConnectionEventArgs e)
        {
            Console.WriteLine($"[{e.IpPort}] client disconnected: {e.Reason}");
        }

        public void DataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine($"[{e.IpPort}]: {Encoding.UTF8.GetString(e.Data.Array, 0, e.Data.Count)}");
        }
    }
}
