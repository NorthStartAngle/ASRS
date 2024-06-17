
using ASRS.libs;
using LIBS;
using MetroFramework.Components;
using Microsoft.Vbe.Interop.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static LIBS.Common;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ASRS.Component
{
    public partial class InboundOperator : MetroFramework.Controls.MetroUserControl
    {
        public InbouundOperatorEvents dispatcher;

        private string availableLocation = "";
        private string swapLocation = "";
        private string reservationLocation1 = "";
        private string reservationLocation2 = "";
        private ProductLookup pendingProduct;
        Task _idleMSGProcessor = null;
        CancellationTokenSource idleTokenSource = null;
        CancellationToken _idletoken;

        public List<MSG_GROUP> instructions = new List<MSG_GROUP>();

        System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();
        public InboundOperator(MetroStyleManager styler = null)
        {
            InitializeComponent();

            myTimer.Tick += MyTimer_Tick;
            myTimer.Interval = 1000;
            myTimer.Start();

            this.TabStop = true;

            if (styler != null )
            {
                MetroStyleManager style = new MetroStyleManager();
                style.Owner = this;

                style.Style = styler.Style;
                this.StyleManager = style;
            }


            lbl_Time.Text = DateTime.Now.ToString("h:mm tt MMMM d,yyyy");

            lstBarCodes.Items.Clear();

            lstBarCodes.Visible = false;
            showActivate(false);


            lstBarCodes.Items.AddRange((string[])Manager.AppOwner.stProduct.Select(s => s.Barcodes).ToArray());

            idleTokenSource = new CancellationTokenSource();
            _idletoken = idleTokenSource.Token;
            _idletoken.Register(() =>
            {
                try
                {
                    _idleMSGProcessor.Dispose();
                }
                catch (Exception)
                {

                }
            });
            _idleMSGProcessor = Task.Run(IdleProcessing, _idletoken);
        }

        ~InboundOperator()
        {
            try
            {
                idleTokenSource.Cancel();
            }
            catch (Exception)
            {

            }
        }
        private void MyTimer_Tick(object sender, EventArgs e)
        {
            lbl_Time.Text = DateTime.Now.ToString("h:mm tt MMMM d,yyyy");
        }

        public void setMode(bool isVerify = true)
        {
            btnVerify.Enabled = isVerify;
            btnShelve.Enabled = !isVerify;
            //btnManual.Enabled = isVerify;

            txtBarcode.Enabled = isVerify;
            lstBarCodes.Visible = false;

            if (isVerify )
            {
                txtBarcode.Text = "";
                txtBarcode.Focus();                
            }                
        }

        public void showActivate(bool flag = false,int availables =0)
        {
            lblTitle.Visible = flag;
            lblProductPreview.Visible = flag;
            panLookup.Visible = flag;
            lblAvailable.Visible = flag;

            if(flag)
            {
                lblAvailable.Text = $"Opend Storage(s) : {availables}";
            }
        }

        public void Verified()
        {
            panLookup.Enabled = true;
            setMode(false);
        }

        public void UnVerified(int status = 0)
        {
            string strMSG = "";
            switch(status)
            {
                case 0:
                    strMSG = "No space location";
                    break;
                case -1:
                    strMSG = "Double deep configure is missing";
                    break;
                case -2:
                    strMSG = "Reservation locations are full";
                    break;
                default:
                    break;
            }

            MessageBox.Show(strMSG, "Operator failed!");
        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            var pds = Manager.AppOwner.stProduct.Where(item => item.Barcodes.Contains(txtBarcode.Text));

            if (pds.Count() == 1)
            {
                ProductLookup pl = pds.First<ProductLookup>();
                lblSKU.Text = $"SKU : {pl.SKU}";
                lblPID.Text = $"PID : {pl.ProductID}";
                lblProductPreview.Image = System.Drawing.Image.FromFile(pl.Image);

                if (lblProductPreview.Image != null)
                {
                    lblProductPreview.Text = "";
                }
                else
                {
                    lblProductPreview.Text = "No Preview";
                }
                
                //panLookup.Enabled = false;
                Inbound_CheckOpenStorages(pl);
                //events?.Invoke(this, new ProductLookupEventArgs(pl, ProductLookupEventReason.Verified));
            }
            else
            {
                lblSKU.Text = "";
                lblPID.Text = "";
            }
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            lstBarCodes.Visible = !lstBarCodes.Visible;
        }

        private void lstBarCodes_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBarcode.Text = lstBarCodes.SelectedItem.ToString();
            lstBarCodes.Visible = false;
        }

        private void lstBarCodes_Leave(object sender, EventArgs e)
        {
            lstBarCodes.Visible = false;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            lstBarCodes.Visible = !lstBarCodes.Visible;
            if(lstBarCodes.Visible)
            {
                this.ActiveControl = lstBarCodes;
            }
        }       

        private void btnShelve_Click(object sender, EventArgs e)
        {
            //events?.Invoke(this, new ProductLookupEventArgs(null, ProductLookupEventReason.Shelve));
        }

        private void OnShelve()
        {
            MoveProduct();
        }

        private void Inbound_CheckOpenStorages(ProductLookup _product)
        {
            availableLocation = "";
            swapLocation = "";
            reservationLocation1 = "";
            reservationLocation2 = "";
            pendingProduct = _product;

            int status = 0;
            int index = 0;
            do
            {
                if (!Manager.AppOwner.inventorys[index].getReserveEmpty() & !Manager.AppOwner.inventorys[index].getFull())
                {
                    if (!Manager.AppOwner.inventorys[index].getDoubleDeep())// when open storage location is not doubledeep
                    {
                        availableLocation = Manager.AppOwner.inventorys[index].getLocation_RowCol();
                        status = 1;
                        break;
                    }
                    var s = Manager.AppOwner.inventorys[index].getLocation_RowCol();
                    var s1 = s.Substring(s.LastIndexOf('\\') + 1);
                    if (s1 == "2")// when open storage location is 2nd location in doubledeep
                    {
                        availableLocation = Manager.AppOwner.inventorys[index].getLocation_RowCol();
                        status = 2;
                        break;
                    }
                    if ((index + 1 < Manager.AppOwner.inventorys.Count))
                    {
                        if (!Manager.AppOwner.inventorys[index + 1].getReserveEmpty() & !Manager.AppOwner.inventorys[index + 1].getFull())
                        {
                            availableLocation = Manager.AppOwner.inventorys[index + 1].getLocation_RowCol(); status = 3; break;
                        }
                        else if (!Manager.AppOwner.inventorys[index + 1].getReserveEmpty())
                        {
                            if (pendingProduct.SKU != Manager.AppOwner.inventorys[index + 1].getSKU())
                            {
                                availableLocation = Manager.AppOwner.inventorys[index].getLocation_RowCol();
                                status = 4;
                                break;
                            }
                            else
                            {
                                //ASRS_Inventory reserve_location = Manager.AppOwner.inventorys.FindAll(x => x.getReserveEmpty() && !x.getFull()).First<ASRS_Inventory>();

                                List<ASRS_Inventory> reserves = Manager.AppOwner.inventorys.FindAll(x => x.getReserveEmpty() && !x.getFull()).Take(2).ToList();
                                if(reserves!= null)
                                {
                                    status = -2; break;
                                }

                                availableLocation = Manager.AppOwner.inventorys[index + 1].getLocation_RowCol();
                                swapLocation = Manager.AppOwner.inventorys[index].getLocation_RowCol();
                                reservationLocation1 = reserves[0].getLocation_RowCol();
                                reservationLocation2 = reserves[1].getLocation_RowCol();
                                status = 5;
                                break;
                            }
                        }
                    }
                    else
                    {
                        status = -1;
                    }
                }
                index++;
            } while (index < Manager.AppOwner.inventorys.Count);

            if (status > 0)
            {
                Invoke((Action)(() => {
                    Verified();
                    if (status == 5)
                    {
                        _ = Manager.AppOwner.inventorys.Find(x => x.getLocation_RowCol() == swapLocation).setFull(true).save(Manager.db);
                        _ = Manager.AppOwner.inventorys.Find(x => x.getLocation_RowCol() == reservationLocation1).setFull(true).save(Manager.db);
                        _ = Manager.AppOwner.inventorys.Find(x => x.getLocation_RowCol() == reservationLocation2).setFull(true).save(Manager.db);
                    }
                    else
                    {
                        _ = Manager.AppOwner.inventorys.Find(x => x.getLocation_RowCol() == availableLocation).setFull(true).save(Manager.db);
                    }
                }));
            }
            else
            {
                // error while seeking the storage location, error details is status variable
                // status =-2 : Reservation location is full, status =-1: Doubledeep configure is missing
                Invoke((Action)(() => {
                    UnVerified(status);
                }));
            }
        }

        private void MoveProduct()
        {

        }

        // Event Handlers from Conveyor Sensors
        public void OnWakeUpSensor1_ON()
        {
            int availablePos = Manager.AppOwner.inventorys.FindAll(x => !x.getReserveEmpty() && !x.getFull()).Count();
            if (availablePos > 0)
            {
                Invoke((Action)(() => {
                    showActivate(true, availablePos);
                    setMode(true);
                }));

                /*this.Invoke((Action<string>)(message =>
                {
                    statusLabel.Text = message;
                }), "Task Completed!");*/

            }
        }

        public void Gecko_StatusChanged(object sender, RTS e)
        {
            MSG_GROUP curMSGGROUP = instructions.Find(i => i.status == 2);
            if (curMSGGROUP == null) return;

            MSG curMSG = curMSGGROUP.Find(i => ((WTK)i).taskId == e.taskId);
            if (curMSG == null) return;

            if(e.taskStatus ==4)//completed
            {
                curMSGGROUP.Remove(curMSG);
                
                if (curMSGGROUP.Count() == 0)
                {

                    instructions.Remove(curMSGGROUP);
                }
            }

            dispatcher?.HandleStorageChanged(this, new InboundStatus(curMSGGROUP.product, e.taskStatus+1, curMSGGROUP.totalMSG, curMSGGROUP.Count()));
        }

        public void Gecko_RecvWTK(object sender, RTK e)
        {
            Monitor.Enter(instructions);
            MSG_GROUP curMSGGROUP = instructions.Find(i => i.status == 1);
            MSG curMSG = curMSGGROUP.Find(i => ((WTK)i).taskId == e.taskId);

            //curMSGGROUP.Remove(curMSG);

            if (e.recvResult> 0)
            {
                curMSGGROUP.status = 3; //Error while direct to Gecko

                dispatcher?.HandleStorageChanged(this, new InboundStatus(curMSGGROUP.product, 10, curMSGGROUP.totalMSG, curMSGGROUP.Count()));
                instructions.Remove(curMSGGROUP);
            }
            else
            {
                curMSGGROUP.status = 2;
                dispatcher?.HandleStorageChanged(this, new InboundStatus(curMSGGROUP.product, 1, curMSGGROUP.totalMSG, curMSGGROUP.Count()));
            }
            Monitor.Exit(instructions);
        }

        public void OnEndofConveyorSensor2_ON()
        {
            Monitor.Enter(instructions);
            MSG_GROUP mg = new MSG_GROUP();
            mg.product = pendingProduct;

            if (swapLocation == null)
            {
                WTK w = createWTK(Manager.AppOwner.subSystem.ZPA, Common.ConvertToPos(availableLocation));

                mg.Add(w);mg.totalMSG = 1;
            }
            else
            {
                WTK w1 = createWTK(Manager.AppOwner.subSystem.ZPA, Common.ConvertToPos(reservationLocation1));
                WTK w2 = createWTK(Common.ConvertToPos(availableLocation), Common.ConvertToPos(reservationLocation2));
                WTK w3 = createWTK(Common.ConvertToPos(reservationLocation1), Common.ConvertToPos(availableLocation));
                WTK w4 = createWTK(Common.ConvertToPos(reservationLocation2), Common.ConvertToPos(swapLocation));

                mg.Add(w1); mg.Add(w2); mg.Add(w3); mg.Add(w4); mg.totalMSG = mg.Count();
            }

            instructions.Add(mg);

            Monitor.Exit(instructions);
        }

        private async void IdleProcessing()
        {
            while(!_idletoken.IsCancellationRequested)
            {
                await Task.Delay(100, _idletoken).ConfigureAwait(false);
                if(Manager.AppOwner.gecko.isWTKAvaiable())
                {
                    if (instructions.FindAll(item => item.status > 0).Count() == 0)
                    {
                        foreach (MSG_GROUP mg in instructions)
                        {
                            if (mg.Count > 0)
                            {
                                mg.status = 1;
                                Manager.AppOwner.gecko.setWTK((WTK)mg[0]);
                                break;
                            }
                        }
                    }
                }                                
            }
        }

        /*
        public void On_RTK(Object sender, GeckoRTKArgs e)
        {
            WTK statusInfo = Manager.AppOwner.gecko._lastSentWTK;
            
            switch (e.Content.recvResult)
            {
                case 0:
                    {
                        Common.Pos _pos = new Common.Pos() { col = statusInfo.toCol,row =statusInfo.toRow ,deep = statusInfo.toDepth};
                        var location = Common.ConvertFromPos(_pos);

                        WTK instruction = new WTK();
                        ushort[] param;

                        if (location == availableLocation)
                        {

                        }else if(location == reservationLocation)
                        {
                            Common.Pos fromPOS = Common.ConvertToPos(swapLocation);
                            Common.Pos ToPOS = Common.ConvertToPos(reservationLocation);

                            param = new ushort[] { (ushort)statusInfo.taskMode, (ushort)fromPOS.row, (ushort)fromPOS.col, (ushort)statusInfo.fromColOffsetDir, (ushort)statusInfo.fromColOffset, (ushort)statusInfo.fromLayer, (ushort)statusInfo.fromDepthMax, (ushort)statusInfo.fromDepth, (ushort)ToPOS.row, (ushort)ToPOS.col, (ushort)statusInfo.toColOffsetDir, (ushort)statusInfo.toColOffset, (ushort)statusInfo.toLayer, (ushort)statusInfo.toDepthMax, (ushort)1, (ushort)statusInfo.boxLength, (ushort)statusInfo.boxWidth, (ushort)statusInfo.taskReserved5, (ushort)statusInfo.taskReserved4, (ushort)statusInfo.boxHeight };
                        }
                        else if(location == swapLocation)
                        {

                        }
                    }
                    break;
                case 1:
                    break;
                case 2:
                    break;
                default:
                    break;
            }
    }*/

        private WTK createWTK(Pos toDest)
        {
            return createWTK(new Pos()
            {
                row = 0,
                col = 0,
                deep = 1
            }, toDest);
        }

        private WTK createWTK(Pos fromSrc, Pos toDest)
        {
            WTK wtk = new WTK()
            {
                taskMode = 1,
                taskId = Common.unique2(),
                fromRow = fromSrc.row,
                fromCol = fromSrc.col,
                fromColOffsetDir = 0,
                fromColOffset = 0,
                fromLayer = 1,
                fromDepthMax = 0,
                fromDepth = fromSrc.deep,
                toRow = toDest.row,
                toCol = toDest.col,
                toColOffsetDir = 0,
                toColOffset = 0,
                toLayer = 1,
                toDepthMax = 2,
                toDepth = toDest.deep,
                boxLength = 18,
                boxWidth = 36,
                taskReserved5 = 0,
                taskReserved4 = 0,
                boxHeight = 44,
                dt = DateTime.Now,
            };

            return wtk;
        }
    }
}
