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
using Microsoft.Vbe.Interop.Forms;
using System.Runtime.Remoting.Channels;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ASRS.Properties;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Media;
using System.Security.Cryptography;
using System.Globalization;
using LIBS;
using System.Threading;
using System.Net.Sockets;
using static MaterialSkin.Controls.MaterialForm;
using System.Diagnostics;

namespace ASRS.Component
{
    public partial class Manager : MetroFramework.Forms.MetroForm
    {
        //Using Screen
        private Splash spash = null;
        private frmUserAccount _userAccountDlg = null;
        private InboundOperator _opInbound = null;
        private PTLOperator _PTLOperator = null;
        public static Manager AppOwner = null;
        screenSwitchingeDelegrate screenSwitch;

        public static dbAccess db = null;
        
        public GeckoClient gecko = null;
        Events _events = new Events();

        public List<ProductLookup> stProduct = new List<ProductLookup>();
        public List<ASRS_Inventory> inventorys = new List<ASRS_Inventory>();
        public List<PTL_Bay> ptls = new List<PTL_Bay>();
        public SubSystem subSystem = null;

        private CancellationTokenSource TooltipCT = new CancellationTokenSource();

        private delegate void screenSwitchingeDelegrate(System.Windows.Forms.UserControl from);
        
        public Manager()
        {
            InitializeComponent();

            _ = new Setting();

            screenSwitch = new screenSwitchingeDelegrate(layoutForm);

            this.StyleManager = this.managerStyle;

            spash = new Splash();
            spash.stateChanged += childEventReception;

            _userAccountDlg = new frmUserAccount(this.managerStyle);
            _userAccountDlg.stateChanged += childEventReception;

            layoutForm(spash);
            changeTitle("Initializing");
            AppOwner = this;

            picGeckoIndicator.Visible = false;
            picZPAIndicator.Visible = false;
            picPTL.Visible  = false;
        }

        private void Initialize()
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

                Manager.db.RunQueryWithCallBack("select * from ASRS_Inventory  order by Location_RowCol", (OleDbDataReader reader) =>
                {
                    if (reader == null)
                    {
                        return;
                    }

                    try
                    {
                        if (reader.HasRows)
                        {
                            inventorys.Clear();
                            while (reader.Read())
                            {
                                ASRS_Inventory inventory = new ASRS_Inventory();
                                inventory.setLocation_RowCol(SafeGetMethods.SafeGetString(reader, 0)).setSKU(SafeGetMethods.SafeGetString(reader, 1)).setProductID(SafeGetMethods.SafeGetString(reader, 2)).setDoubleDeep(SafeGetMethods.SafeGetBool(reader, 4)).setReserveEmpty(SafeGetMethods.SafeGetBool(reader, 5)).setFull(SafeGetMethods.SafeGetBool(reader, 6));

                                DateTime parsedDate;
                                try
                                {
                                    parsedDate = DateTime.ParseExact(SafeGetMethods.SafeGetString(reader, 3), "YYYY-MM-DD HH:mm", CultureInfo.InvariantCulture);
                                    inventory.setTimeIN(parsedDate);
                                }
                                catch (Exception)
                                {
                                    inventory.setTimeIN(DateTime.MinValue);
                                }

                                inventorys.Add(inventory);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }

                    reader?.Close();
                });

                Manager.db.RunQueryWithCallBack("select * from PTL_Bay  order by BayID", (OleDbDataReader reader) =>
                {
                    if (reader == null)
                    {
                        return;
                    }

                    try
                    {

                        if (reader.HasRows)
                        {
                            ptls.Clear();
                            while (reader.Read())
                            {
                                PTL_Bay ptl = new PTL_Bay()
                                {
                                    BayID = SafeGetMethods.SafeGetString(reader,0),
                                    SKU_Assigned = SafeGetMethods.SafeGetString(reader,1)
                                };


                                ptls.Add(ptl);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }

                    reader?.Close();
                });

                Manager.db.RunQueryWithCallBack("select * from ProductLookup order by ID", (OleDbDataReader reader) =>
                {
                    if (reader == null)
                    {
                        return;
                    }

                    try
                    {
                        if (reader.HasRows)
                        {
                            subSystem = new SubSystem();
                            while (reader.Read())
                            {
                                subSystem.ID = SafeGetMethods.SafeGetInt(reader, 0);

                                for(int i = 0; i<12; i++)
                                {
                                    subSystem.lanes[i].row = Convert.ToInt32(SafeGetMethods.SafeGetString(reader, 1+i*2));
                                    subSystem.lanes[i].col = Convert.ToChar(SafeGetMethods.SafeGetString(reader, 2 + i * 2).ToUpper()) - 65;
                                }
                                subSystem.ZPA.row = Convert.ToInt32(SafeGetMethods.SafeGetString(reader,25));
                                subSystem.ZPA.col = Convert.ToChar(SafeGetMethods.SafeGetString(reader,26).ToUpper()) - 65;
                                subSystem.offset.row = SafeGetMethods.SafeGetInt(reader, 27);
                                subSystem.offset.col = SafeGetMethods.SafeGetInt(reader, 28);
                            }
                        }
                    }
                    catch (Exception)
                    {

                    }

                    reader?.Close();
                });

                Manager.db.RunQueryWithCallBack("select * from System order by ID", (OleDbDataReader reader) =>
                {
                    if (reader == null)
                    {
                        return;
                    }

                    try
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int id = SafeGetMethods.SafeGetInt(reader, 0);
                                string pid = SafeGetMethods.SafeGetString(reader, 1);
                                string img = SafeGetMethods.SafeGetString(reader, 2);
                                string sku = SafeGetMethods.SafeGetString(reader, 3);
                                string bar = SafeGetMethods.SafeGetString(reader, 4);

                                string[] bars = bar.Split(',');
                                foreach (string _bar in bars)
                                {
                                    var barcode = _bar.Replace("'", "");

                                    ProductLookup productLookup = new ProductLookup()
                                    {
                                        ID = id,
                                        ProductID = pid,
                                        Image = img,
                                        SKU = sku,
                                        Barcodes = barcode
                                    };

                                    stProduct.Add(productLookup);

                                }
                            }
                        }
                    }
                    catch (Exception)
                    {

                    }

                    reader?.Close();
                });
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
                Invoke((Action)(() => {
                    Text = $"ASRS -{title}";
                }));
            }
            else
            {
                Text = $"ASRS -{title}";
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

        public void childEventReception(object sender,EventArgs e)
        {
            if (sender.GetType().Name == "Splash")
            {
                SplashEventArgs _e = (SplashEventArgs)e;
                switch (_e.Reason)
                {
                    case SplashEventSubject.ready:
                        Initialize();

                        break;
                    case SplashEventSubject.completed:

                        spash.stateChanged -= childEventReception;

                        Invoke((Action)(() => {
                            layoutForm(_userAccountDlg);
                        }));

                        _userAccountDlg.initialize();

                        break;
                    case SplashEventSubject.error:
                        break;
                    case SplashEventSubject.finished:
                        break;
                    default:
                        break;
                }
            }
            else if (sender.GetType().Name == "frmUserAccount")
            {
                UserAccountEventArg _e = (UserAccountEventArg)e;
                switch (_e.Reason)
                {
                    case UserAccountEventSubject.ready:
                        changeTitle("Logining");
                        break;
                    case UserAccountEventSubject.apply:
                        OnUserLogin(_e.pendingUser);
                        break;
                    case UserAccountEventSubject.error:
                        break;
                    case UserAccountEventSubject.finish:
                        spash.stateChanged -= childEventReception;
                        break;
                    default:
                        break;
                }
            }
            else if (sender.GetType().Name == "InboundOperator")
            {
                /*ProductLookupEventArgs _e = (ProductLookupEventArgs)e;
                switch (_e.Reason)
                {
                    case ProductLookupEventReason.Verified:
                        {
                            // Seeking the available storage location
                            //Inbound_CheckOpenStorages(_e.Content);
                        }
                        break;
                    case ProductLookupEventReason.Shelve:
                        ((InboundOperator)sender).events -= childEventReception;
                        
                        //OnShelve();
                        break;
                    default:
                        break;
                }*/
            }
        }
        
        protected void layoutForm(System.Windows.Forms.UserControl form)
        {
            if (form != null) {
                Size sz = form.Size;

                if (bodyLayout.Controls.Count > 0)
                {
                    bodyLayout.Controls.RemoveAt(0);
                }

                bodyLayout.Controls.Add(form, 1, 1);

                //bodyLayout.RowStyles[1].SizeType = SizeType.Absolute;
                bodyLayout.RowStyles[1].Height = sz.Height;

                bodyLayout.ColumnStyles[1].SizeType = SizeType.Absolute;
                bodyLayout.ColumnStyles[1].Width = sz.Width;
            }
        }

        private void OnUserLogin(USER _login)
        {
            Setting.instance.LoginUser = _login;

            if (Setting.instance.LoginUser.access_level == 2)
            {
                picGeckoIndicator.Visible = true;
                picZPAIndicator.Visible = true;
                picPTL.Visible = false;
                //picGeckoIndicator.Enabled = false;
                //picZPAIndicator.Enabled = false;

                Action del =() =>
                {
                    _opInbound = new InboundOperator();
                    _opInbound.dispatcher.inboundStatusChanged += InboundStatusChanged;

                    _events.WTK_RecvResult += _opInbound.Gecko_RecvWTK;
                    _events.StatusChanged += _opInbound.Gecko_StatusChanged;

                    layoutForm(_opInbound);
                    changeTitle($"{Setting.instance.LoginUser.username}");

                    Task.Run(async () => { await Task.Delay(3000); _opInbound.OnWakeUpSensor1_ON(); });
                };
                Invoke(del);

            }
            else if(Setting.instance.LoginUser.access_level == 3)
            {
                picGeckoIndicator.Visible = true;
                //picGeckoIndicator.Enabled = false;
                picZPAIndicator.Visible = false;
                picPTL.Visible = true;

                Invoke((Action)(() => {
                    _PTLOperator = new PTLOperator();
                    layoutForm(_PTLOperator);
                    changeTitle($"{Setting.instance.LoginUser.username}");

                    _events.WTK_RecvResult += _PTLOperator.Gecko_RecvWTK;
                    _events.StatusChanged += _PTLOperator.Gecko_StatusChanged;

                    Task.Run(async () => { await Task.Delay(3000); _PTLOperator.PTLSwitch_Pressed(2); });
                }));
            }
            GeckoSetting();
        }

        private void InboundStatusChanged(object sender, InboundStatus e)
        {
            showTooltip($"SKU={e.SKU} storaging is processing {e.status} of {e.curProcess}/{e.subProcess}");
        }

        /// <summary>
        /// Gecko 
        /// </summary>
        /// 
        private  void GeckoSetting()
        {
            gecko = new GeckoClient();
            gecko.dispath.Connected += Gecko_Connected;
            gecko.dispath.Disconnected += Gecko_Disconnected;
            gecko.dispath.ErrorRecepted += Gecko_ErrorRecepted;
            gecko.dispath.WTK_RecvResult += Gecko_WTK_RecvResult;
            gecko.dispath.StatusChanged += Gecko_StatusChanged;
            gecko.connectGecko();
        }

        private void Gecko_StatusChanged(object sender, RTS e)
        {
            _events?.HandleStatusChanged(sender,e);
        }

        private void Gecko_WTK_RecvResult(object sender, RTK e)
        {
            _events?.HandleRecvWTK(sender,e);    
        }

        private void Gecko_ErrorRecepted(object sender, Exception e)
        {
            showTooltip("Error");
            Invoke((Action)(() => {
                picGeckoIndicator.Enabled = false;
            }));
        }
        
        private void Gecko_Connected(object sender, ConnectionEventArgs e)
        {
            showTooltip("Connected");
        }

        private void Gecko_Disconnected(object sender, ConnectionEventArgs e)
        {
            showTooltip("Disconnected");

            Invoke((Action)(() => {
                picGeckoIndicator.Enabled = false;
            }));
        }

        private void showTooltip(string s)
        {
            TooltipCT.Cancel();
            TooltipCT = new CancellationTokenSource();

            Invoke((Action)(async () => {
                lblGeckoStatus.Text = s;
                try
                {
                    await Task.Delay(3000, TooltipCT.Token);

                    // Clear the label text if the delay was not cancelled
                    lblGeckoStatus.Text = string.Empty;
                }
                catch (TaskCanceledException)
                {
                    // The task was cancelled, so do nothing
                }
                /*System.Windows.Forms.ToolTip toolTip1 = new System.Windows.Forms.ToolTip();

                toolTip1.AutoPopDelay = 2000;
                toolTip1.InitialDelay = 50;
                toolTip1.ReshowDelay = 50;
                toolTip1.IsBalloon = true;
                // Force the ToolTip text to be displayed whether or not the form is active.
                toolTip1.ShowAlways = true;

                // Set up the ToolTip text for the Button and Checkbox.
                toolTip1.Show(s,picGeckoIndicator,40,-50,1000);*/
            }));           
        }

    }
}
