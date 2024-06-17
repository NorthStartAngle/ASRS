using LIBS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASRS.libs
{
    public  class OutboundOperatorEvents
    {
        public event EventHandler<OutboundStatus> outboundStatusChanged;


        internal void HandleStorageChanged(object sender, OutboundStatus args)
        {
            outboundStatusChanged?.Invoke(sender, args);
        }
    }
}
