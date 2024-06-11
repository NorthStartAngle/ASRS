
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
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ASRS.Component
{
    public partial class InboundOperator : MetroFramework.Controls.MetroUserControl
    {
        public event EventHandler<ProductLookupEventArgs> events;

        private string availableLocation = "";
        private string swapLocation = "";
        private string reservationLocation = "";
        private ProductLookup pendingProduct;

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

        public void OnEndofConveyorSensor2_ON()
        {
            //Save product by Gecko

        }
        private void MoveProduct()
        {

        }
        private void Inbound_CheckOpenStorages(ProductLookup _product)
        {
            availableLocation = "";
            swapLocation = "";
            reservationLocation = "";
            pendingProduct = _product;

            int status = 0;
            int index = 0;
            do
            {
                if (!Manager.AppOwner.inventorys[index].getReserveEmpty() & !Manager.AppOwner.inventorys[index].getFull())
                {
                    if (!Manager.AppOwner.inventorys[index].getDoubleDeep())
                    {
                        availableLocation = Manager.AppOwner.inventorys[index].getLocation_RowCol();
                        status = 1;
                        break;
                    }
                    var s = Manager.AppOwner.inventorys[index].getLocation_RowCol();
                    var s1 = s.Substring(s.LastIndexOf('\\') + 1);
                    if (s1 == "2")
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
                                ASRS_Inventory reserve_location = Manager.AppOwner.inventorys.FindAll(x => x.getReserveEmpty() && !x.getFull()).First<ASRS_Inventory>();
                                if (reserve_location != null)
                                {
                                    status = -2; break;
                                }
                                availableLocation = Manager.AppOwner.inventorys[index + 1].getLocation_RowCol();
                                swapLocation = Manager.AppOwner.inventorys[index].getLocation_RowCol();
                                reservationLocation = reserve_location.getLocation_RowCol();
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
                        _ = Manager.AppOwner.inventorys.Find(x => x.getLocation_RowCol() == reservationLocation).setFull(true).save(Manager.db);
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

        private void OnShelve()
        {
            MoveProduct();
        }
    }
}
