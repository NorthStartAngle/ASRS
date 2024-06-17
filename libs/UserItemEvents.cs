using LIBS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASRS.libs
{
    public class UserItemEvents
    {
        public event EventHandler<USER> UserItemChangeRequest;
        public event EventHandler<USER> UserItemRemoveRequest;
        public event EventHandler UserItemSelected;
        public event EventHandler UserItemDeselected;

        internal void HandleChangeRequest(object sender, USER args)
        {
            UserItemChangeRequest?.Invoke(sender, args);
        }

        internal void HandleRemoveRequest(object sender, USER args)
        {
            UserItemRemoveRequest?.Invoke(sender, args);
        }

        internal void HandleItemSelected(object sender)
        {
            UserItemSelected?.Invoke(sender,null);
        }

        internal void HandleItemLeaved(object sender)
        {
            UserItemDeselected?.Invoke(sender, null);
        }
    }
}
