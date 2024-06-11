using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIBS
{
    public enum ProductLookupEventReason
    {
        None,
        Verified,
        Shelve
    }
    public class ProductLookupEventArgs : EventArgs
    {
        internal ProductLookupEventArgs(ProductLookup content, ProductLookupEventReason reason = ProductLookupEventReason.None)
        {
            Content = content;
            Reason = reason;
        }

        /// <summary>
        /// The IP address and port number of the connected peer socket.
        /// </summary>
        public ProductLookup Content { get; }

        /// <summary>
        /// The reason for the disconnection, if any.
        /// </summary>
        public ProductLookupEventReason Reason { get; } = ProductLookupEventReason.None;
    }
}
