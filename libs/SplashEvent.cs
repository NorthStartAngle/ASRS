using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIBS
{
    public enum SplashEventSubject
    {
        ready,
        completed,
        error,
        finished,
    }
    public class SplashEventArgs : EventArgs
    {
        public SplashEventArgs(SplashEventSubject reason = SplashEventSubject.ready, string content = "")
        {
            Content = content;
            Reason = reason;
        }

        public string Content { get; }
        public SplashEventSubject Reason { get; } = SplashEventSubject.ready;

    }
}
