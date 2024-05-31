using MaterialSkin;
using MaterialSkin.Controls;
using MetroFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
using ASRS.libs;
using Microsoft.Vbe.Interop.Forms;
using System.Runtime.Remoting.Channels;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ASRS.Component
{
    public partial class Manager : MetroFramework.Forms.MetroForm
    {
        private Splash spash = null;
        private frmUserAccount _userAccountDlg = null;
        private Database db = null;

        private delegate void screenSwitchingeDelegrate(System.Windows.Forms.Control from);
        screenSwitchingeDelegrate screenSwitch;

        public Manager()
        {
            InitializeComponent();

            screenSwitch = new screenSwitchingeDelegrate(layoutForm);

            this.StyleManager = this.managerStyle;

            spash = new Splash();
            spash.stateChanged += stateChanged;

            _userAccountDlg = new frmUserAccount(this.managerStyle);

            layoutForm((System.Windows.Forms.Control)spash);
        }

        public void Initialize()
        {
            if(db == null)
            {
                db = new Database();
            }

            string m = "";
            if(db.connect())
            {
                m = "Database connection was established!";
            }
            else
            {
                m = "Database connection was failed!";
            }

            Task.Run(async () => {
                await Task.Delay(1000);
                spash.showMessage(m);
            });
        }

        private void stateChanged(object sender,EventArgs e)
        {
            if(sender.GetType().Name == "Splash")
            {
                SplashEventArgs _e = (SplashEventArgs)e;
                switch (_e.Reason)
                {
                    case SplashEventSubject.ready:
                        Initialize();
                        break;
                    case SplashEventSubject.completed:
                         
                        spash.stateChanged -= stateChanged;
                        

                        if (bodyLayout.InvokeRequired)
                        {                           
                            Invoke(screenSwitch, (System.Windows.Forms.Control)_userAccountDlg);
                        }
                        else
                        {
                            layoutForm((System.Windows.Forms.Control)_userAccountDlg);
                        }

                        
                        break;
                    case SplashEventSubject.error:
                        break;
                    case SplashEventSubject.finished:
                        break;
                    default:
                        break;
                }
            }
        }

        protected void layoutForm(System.Windows.Forms.Control form)
        {
            if (form != null) {
                Size sz = form.Size;

                if (bodyLayout.Controls.Count > 0)
                {
                    bodyLayout.Controls.RemoveAt(0);
                }

                bodyLayout.Controls.Add(form, 1, 1);

                bodyLayout.RowStyles[1].SizeType = SizeType.Absolute;
                bodyLayout.RowStyles[1].Height = sz.Height;

                bodyLayout.ColumnStyles[1].SizeType = SizeType.Absolute;
                bodyLayout.ColumnStyles[1].Width = sz.Width;
            }
        }
    }
}
