using LIBS;
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
using static System.Net.Mime.MediaTypeNames;

namespace ASRS.Component
{
    public partial class Splash : System.Windows.Forms.UserControl
    {
        public event EventHandler<SplashEventArgs> stateChanged;

        private delegate void showMessageDelegate(string message);
        public  Splash()
        {
            InitializeComponent();

            Task.Run(async () => {
                await Task.Delay(2000);
                initialize();
            });
        }

        private void initialize()
        {
            stateChanged?.Invoke(this, new SplashEventArgs(SplashEventSubject.ready));
        }

        public void receiveEvent(SplashEventSubject e)
        {
            stateChanged?.Invoke(this, new SplashEventArgs(e));
        }
        
        public void showMessage(string message)
        {
            if(lblStatus.InvokeRequired)
            {
                showMessageDelegate d = new showMessageDelegate(showMessage);
                this.Invoke(d, new object[] { message });
            }
            else
            {
                lblStatus.Text = message;
            }

            /*Task.Run(async () => {
                await Task.Delay(2000);
                stateChanged?.Invoke(this, new SplashEventArgs(SplashEventSubject.completed));
            });*/
        }
    }
}
