using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIBS
{
    public enum UserAccountEventSubject
    {
        ready,
        error,
        apply,
        finish,
    }

    public class UserAccountEventArg : EventArgs
    {
        public UserAccountEventArg(UserAccountEventSubject reason = UserAccountEventSubject.ready, string content = "", USER PendingUser = null)
        {
            Content = content;
            Reason = reason;
            pendingUser = PendingUser;
        }

        public string Content { get; }
        public UserAccountEventSubject Reason { get; } = UserAccountEventSubject.ready;

        public USER pendingUser;
    }
}
