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
using LIB;
using Microsoft.Vbe.Interop.Forms;
using System.Runtime.Remoting.Channels;

namespace ASRS.Component
{
    public partial class Manager : MetroFramework.Forms.MetroForm
    {
        Splash spash = null;
        frmUserAccount _userAccountDlg = null;
        public Manager()
        {
            InitializeComponent();

            this.StyleManager = this.managerStyle;

            spash = new Splash();
            spash.stateChanged += stateChanged;

            layoutForm((System.Windows.Forms.Control)spash);
        }

        public void Initialize()
        {
            _userAccountDlg = new frmUserAccount(this.StyleManager);
            layoutForm((System.Windows.Forms.Control)_userAccountDlg);
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
                        _userAccountDlg = new frmUserAccount(this.managerStyle);
                        layoutForm((System.Windows.Forms.Control)_userAccountDlg);
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

                if(bodyLayout.Controls.Count > 0) {
                    this.bodyLayout.Controls.RemoveAt(0);
                }
                
                this.bodyLayout.Controls.Add(form, 1, 1);

                this.bodyLayout.RowStyles[1].SizeType = SizeType.Absolute;
                this.bodyLayout.RowStyles[1].Height = sz.Height;
                //this.bodyLayout.RowStyles[0].SizeType = SizeType.AutoSize;
                //this.bodyLayout.RowStyles[2].SizeType = SizeType.AutoSize;

                this.bodyLayout.ColumnStyles[1].SizeType = SizeType.Absolute;
                this.bodyLayout.ColumnStyles[1].Width = sz.Width;
                //this.bodyLayout.ColumnStyles[0].SizeType = SizeType.AutoSize;
                //this.bodyLayout.ColumnStyles[2].SizeType = SizeType.AutoSize;

                //this.bodyLayout.BackColor = Common.convertStyleToColor((int)this.StyleManager.Style,Color.White);
                this.bodyLayout.Invalidate();
                //metroPanel1.Controls.Add( _userAccountDlg );
            }
        }
    }
}
