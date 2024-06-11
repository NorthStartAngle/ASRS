using LIBS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASRS.libs
{
    public class GeckoEvents : ConnectorEvents
    {
        public event EventHandler<GeckoRTSArgs> DataReceivedStatus;
        public event EventHandler<GeckoRTKArgs> DataReceivedWorkStatus;
        public event EventHandler<int> StatusChanged;

        public GeckoEvents()
        {

        }
        internal void HandleStatusDataReceived(object sender, GeckoRTSArgs args)
        {
            DataReceivedStatus?.Invoke(sender, args);
        }

        internal void HandleStatusChanged(object sender, int args)
        {
            StatusChanged?.Invoke(sender, args);
        }

        internal void HandleWorkDataSent(object sender, GeckoRTKArgs args)
        {
            DataReceivedWorkStatus?.Invoke(sender, args);
        }
    }
}
