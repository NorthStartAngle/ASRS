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
        
        public  GeckoClient gecko = null;
       
        public List<ProductLookup> stProduct = new List<ProductLookup>();
        public List<ASRS_Inventory> inventorys = new List<ASRS_Inventory>();
        public List<PTL_Bay> ptls = new List<PTL_Bay>();

        private delegate void screenSwitchingeDelegrate(System.Windows.Forms.UserControl from);
        
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

            layoutForm(spash);
            changeTitle("Initializing");
            AppOwner = this;

            picGeckoIndicator.Visible = false;
            picZPAIndicator.Visible = false;

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

        public void stateChanged(object sender,EventArgs e)
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

                        spash.stateChanged -= stateChanged;

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
                        spash.stateChanged -= stateChanged;
                        break;
                    default:
                        break;
                }
            }
            else if (sender.GetType().Name == "InboundOperator")
            {
                ProductLookupEventArgs _e = (ProductLookupEventArgs)e;
                switch (_e.Reason)
                {
                    case ProductLookupEventReason.Verified:
                        {
                            // Seeking the available storage location
                            //Inbound_CheckOpenStorages(_e.Content);
                        }
                        break;
                    case ProductLookupEventReason.Shelve:
                        ((InboundOperator)sender).events -= stateChanged;
                        
                        //OnShelve();
                        break;
                    default:
                        break;
                }
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
            if(Setting.instance.LoginUser.access_level == 1)
            {
                picGeckoIndicator.Visible = true;
                picZPAIndicator.Visible = true;
                picGeckoIndicator.Enabled = false;
                picZPAIndicator.Enabled = false;

                Action del =() =>
                {
                    _opInbound = new InboundOperator();
                    _opInbound.events += stateChanged;

                    layoutForm(_opInbound);
                    changeTitle($"{Setting.instance.LoginUser.username}");

                    Task.Run(async () => { await Task.Delay(3000); _opInbound.OnWakeUpSensor1_ON(); });
                };
                Invoke(del);

            }
            else if(Setting.instance.LoginUser.access_level == 2)
            {
                picGeckoIndicator.Visible = true;
                picGeckoIndicator.Enabled = false;
                picZPAIndicator.Visible = false;                

                Invoke((Action)(() => {
                    _PTLOperator = new PTLOperator();
                    layoutForm(_PTLOperator);
                    changeTitle($"{Setting.instance.LoginUser.username}");

                    Task.Run(async () => { await Task.Delay(3000); _PTLOperator.PTLSwitch_Pressed(1); });
                }));

            }

            GeckoSetting();
        }

/// <summary>
/// Gecko 
/// </summary>
/// 
        private  void GeckoSetting()
        {
            gecko = new GeckoClient();
            gecko.Events.Connected += Gecko_Connected;
            gecko.Events.Disconnected += Gecko_Disconnected;
            gecko.Events.DataSent += Gecko_DataSent;
            gecko.Events.DataReceived += Events_DataReceived;
            gecko.Events.DataReceivedStatus += Events_DataReceivedStatus;
            gecko.Events.DataReceivedWorkStatus += Events_DataReceivedWorkStatus;
            gecko.Events.StatusChanged += Events_StatusChanged;

            gecko.GeckoSetting();
        }

        private void Events_DataReceived(object sender, LIBS.DataReceivedEventArgs e)
        {
            
        }

        private void Events_StatusChanged(object sender, int e)
        {
            /*if (status == 5) { content = "Normal"; }
            if (status == 6) { content = "Status Error"; }
            if (status == 7) { content = "Status/Work Error"; }
            if (status == 100) { content = "Disconnected"; MessageBox.Show("Gecko disconnected"); }*/
            string content;
            switch (e)
            {
                case 5:
                    content = "Working...";
                    break;
                case 6:
                    content = "Status/Error";
                    break;
                case 7:
                    content = "Working/Error";
                    break;
                case 100:
                    content = "Disconnected";
                    break;
                default:
                    content = "Unknown";
                    break;
            }
            showTooltip(content);
        }

        private void Events_DataReceivedWorkStatus(object sender, GeckoRTKArgs e)
        {
            
        }

        private void Events_DataReceivedStatus(object sender, GeckoRTSArgs e)
        {
            
        }

        private void Gecko_Connected(object sender, ConnectionEventArgs e)
        {
            showTooltip("Connected");
        }

        private void Gecko_Disconnected(object sender, ConnectionEventArgs e)
        {
            showTooltip("Disconnected");
        }

        private void Gecko_DataSent(object sender, DataSentEventArgs e)
        {
            
        }

        private void showTooltip(string s)
        {
            Invoke((Action)(() => {
                System.Windows.Forms.ToolTip toolTip1 = new System.Windows.Forms.ToolTip();

                toolTip1.AutoPopDelay = 2000;
                toolTip1.InitialDelay = 50;
                toolTip1.ReshowDelay = 50;
                toolTip1.IsBalloon = true;
                // Force the ToolTip text to be displayed whether or not the form is active.
                toolTip1.ShowAlways = true;

                // Set up the ToolTip text for the Button and Checkbox.
                toolTip1.Show(s,picGeckoIndicator,40,-50,1000);
            }));           
        }

    }
}
