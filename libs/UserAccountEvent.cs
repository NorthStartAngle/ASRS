using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASRS.libs
{
    public enum UserAccountEventSubject
    {
        ready,
        completed,
        error,
        finished,
        progress,
    }

    public class UserAccountEventArg : EventArgs
    {
        public UserAccountEventArg(UserAccountEventSubject reason = UserAccountEventSubject.ready, string content = "")
        {
            Content = content;
            Reason = reason;
        }

        public string Content { get; }
        public UserAccountEventSubject Reason { get; } = UserAccountEventSubject.ready;
    }
}
