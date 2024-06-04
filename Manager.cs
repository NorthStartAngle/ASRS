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
using ASRS.Properties;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Media;

namespace ASRS.Component
{
    public partial class Manager : MetroFramework.Forms.MetroForm
    {
        private Splash spash = null;
        private frmUserAccount _userAccountDlg = null;
        private InboundOperator _opInbound = null;
        public static dbAccess db = null;
        public static Form AppOwner = null;

        private delegate void screenSwitchingeDelegrate(System.Windows.Forms.Control from);
        screenSwitchingeDelegrate screenSwitch;

        public Manager()
        {
            InitializeComponent();

            _ = new Setting();

            screenSwitch = new screenSwitchingeDelegrate(layoutForm);

            this.StyleManager = this.managerStyle;

            spash = new Splash();
            spash.stateChanged += stateChanged;

            _userAccountDlg = new frmUserAccount(this.managerStyle);
            _userAccountDlg.stateChanged += stateChanged;

            layoutForm((System.Windows.Forms.Control)spash);
            changeTitle("Initializing");
            statusBar.Visible = false;

            AppOwner = this;
        }

        public void Initialize()
        {
            if(db == null)
            {
                db = new dbAccess();
            }

            string m = "";

            try
            {
                db.connect(Setting.instance.conString);
                m = "Database connection was established!";
            }
            catch(Exception ex)
            {
                m = "Database connection was failed!\n" + ex.ToString();
            }

            Task.Run(async () =>
            {
                await Task.Delay(1000);
                spash.showMessage(m);

                await Task.Delay(1000);
                spash.receiveEvent(SplashEventSubject.completed);
            });
        }

        private void changeTitle(string title)
        {          
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate ()
                {
                    this.Text = $"ASRS -{title}";
                    
                    //System.Windows.Forms.Application.DoEvents();
                });
            }
            else
            {
                this.Text = $"ASRS -{title}";
            }

            this.Invalidate();
        }

        private void soundPlay()
        {
            SoundPlayer simpleSound = new SoundPlayer(@"c:\Windows\Media\chimes.wav");
            simpleSound.Play();
        }

        public void setBackGround()
        {
            
        }

        public void stateChanged(object sender,EventArgs e)
        {
            if(e.GetType().Name == "DialogEventArgs")
            {
                DialogEventArgs _e = (DialogEventArgs)e;
                switch (_e.Reason)
                {
                    case DialogEventReason.showing:
                        bodyLayout.BackColor = Color.FromArgb(240,220,220,220);
                        break;
                    case DialogEventReason.close:
                        bodyLayout.BackColor = Color.White;
                        break;
                    default:
                        break;

                }
            }
            else if(sender.GetType().Name == "Splash")
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

                        _userAccountDlg.initialize();


                        break;
                    case SplashEventSubject.error:
                        break;
                    case SplashEventSubject.finished:
                        break;
                    default:
                        break;
                }
            }else if(sender.GetType().Name == "frmUserAccount")
            {
                UserAccountEventArg _e = (UserAccountEventArg)e;
                switch (_e.Reason)
                {
                    case UserAccountEventSubject.ready:
                        changeTitle("Login");
                        break;
                    case UserAccountEventSubject.apply:
                        Setting.instance.LoginUser = _e.pendingUser;

                        if (bodyLayout.InvokeRequired)
                        {
                            //Invoke(screenSwitch, (System.Windows.Forms.Control)_userAccountDlg);
                            this.Invoke(new MethodInvoker(
                                delegate () {
                                    _opInbound = new InboundOperator();
                                    _opInbound.stateChanged += _opInbound_stateChanged;

                                    layoutForm((System.Windows.Forms.Control)_opInbound);
                                    //statusBar.Visible = true;
                                    changeTitle($" <{Setting.instance.LoginUser.username}> WorkSpace");
                                })
                            );

                        }
                        else
                        {
                            _opInbound = new InboundOperator();
                            _opInbound.stateChanged += _opInbound_stateChanged;

                            layoutForm((System.Windows.Forms.Control)_opInbound);
                            //statusBar.Visible = true;
                            changeTitle($" <{Setting.instance.LoginUser.username}> WorkSpace");
                        }
                        break;
                    case UserAccountEventSubject.error:
                        break;
                    case UserAccountEventSubject.finish:
                        spash.stateChanged -= stateChanged;
                        break;
                    default:
                        break;
                }
            } 
        }
        private void _opInbound_stateChanged(object sender, object e)
        {
            
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
