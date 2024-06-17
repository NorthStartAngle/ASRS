using LIBS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASRS.libs
{
    public  class InbouundOperatorEvents
    {
        public event EventHandler<InboundStatus> inboundStatusChanged;


        internal void HandleStorageChanged(object sender, InboundStatus args)
        {
            inboundStatusChanged?.Invoke(sender, args);
        }

    }
}
