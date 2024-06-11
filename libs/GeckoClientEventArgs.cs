using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIBS
{
    public class GeckoRTSArgs : EventArgs
    {
        internal GeckoRTSArgs(DateTime dt, RTS content ) {
            Content = content;
            sDt = dt;
        }

        public RTS Content { get; }

        public DateTime sDt { get; }
    }

    public class GeckoRTKArgs : EventArgs
    {
        internal GeckoRTKArgs(DateTime dt, RTK content)
        {
            Content = content;
            sDt = dt;
        }

        public RTK Content { get; }

        public DateTime sDt { get; }
    }
}
