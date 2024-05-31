using LIB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASRS.Component
{
    public enum SplashEventSubject
    {
        ready,
        completed,
        error,
        finished,
    }
    public class SplashEventArgs:EventArgs
    {
        public SplashEventArgs(SplashEventSubject reason = SplashEventSubject.ready, string content = "") { 
            Content = content;
            Reason = reason;
        }

        public string Content { get; }
        public SplashEventSubject Reason { get; } = SplashEventSubject.ready;

    }
    public partial class Splash : System.Windows.Forms.UserControl
    {
        public event EventHandler<SplashEventArgs> stateChanged;

        public  Splash()
        {
            InitializeComponent();

            Task.Run(async () => {
                await Task.Delay(3500);
                stateChanged?.Invoke(this, new SplashEventArgs(SplashEventSubject.ready));
            });
        }
    }
}
