using LIB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASRS.libs
{
    public enum DialogEventReason
    {
        None,
        showing,
        close
    }
    public class DialogEventArgs : EventArgs
    {
        internal DialogEventArgs(string content, DialogEventReason reason = DialogEventReason.None)
        {
            Content = content;
            Reason = reason;
        }

        /// <summary>
        /// The IP address and port number of the connected peer socket.
        /// </summary>
        public string Content { get; }

        /// <summary>
        /// The reason for the disconnection, if any.
        /// </summary>
        public DialogEventReason Reason { get; } = DialogEventReason.None;
    }
}
