using LIBS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASRS.Component
{
    public partial class PTLOperator : UserControl
    {
        DateTime pressedDt = DateTime.Now;
        int pressedBayID = -1;
        bool on_off = false;
        int ptl_flash_counter = 0;
        
        private string originalLocation;
        System.Windows.Forms.Timer PTL_Flash_timer = new System.Windows.Forms.Timer();

        public PTLOperator()
        {
            InitializeComponent();

            PTL_Flash_timer.Interval = 1000;
            PTL_Flash_timer.Tick += PTL_Flash;
        }      


        public void PTLSwitch_Pressed(int BayID)
        {
            Invoke((Action)(() =>
            {
                lblPTL_Switch.Text = BayID.ToString();
                if (pressedBayID == BayID && (DateTime.Now - pressedDt).TotalSeconds < 10)
                {
                    PTL_LIGHT_OFF(pressedBayID);
                    PTL_Flash_timer.Stop();
                    return;
                }

                pressedBayID = BayID;
                PTL_Bay ptl = Manager.AppOwner.ptls.Find(x => x.BayID == BayID.ToString());
                if (ptl != null)
                {
                    var inventory = Manager.AppOwner.inventorys.FindAll(x => x.getSKU() == ptl.SKU_Assigned).OrderBy(I => I.getTimeIN()).FirstOrDefault();

                    if (inventory != null)
                    {
                        originalLocation = inventory.getLocation_RowCol();
                        PTL_Flash_timer.Stop();
                        PTL_LIGHT_ON(BayID, 1, 20);
                    }
                    else
                    {
                        ptl_flash_counter = 0;
                        on_off = false;

                        PTL_Flash_timer.Start();
                    }
                }
            }));
        }

        private void PTL_Flash(object sender, EventArgs e)
        {
            if (ptl_flash_counter++ < 20)
            {
                on_off = !on_off;
                if (on_off)
                    PTL_LIGHT_ON(pressedBayID, 2, 1);
                else
                    PTL_LIGHT_OFF(pressedBayID);

            }
            else
            {
                PTL_Flash_timer.Stop();
            }
        }

        public void Sensor_Lane_FULL()
        {
            //back box to the originalLocation
        }

        private void PTL_LIGHT_ON(int BayID, int Color, int delay)
        {
            /*if (Color == 1)
                lblPTLSwitch_Status.Image = global::ASRS.Properties.Resources.green_led;
            else if(Color == 2)
                lblPTLSwitch_Status.Image = global::ASRS.Properties.Resources.red_led;

            Invalidate();*/
        }

        private void PTL_LIGHT_OFF(int BayID)
        {
            /*lblPTLSwitch_Status.Image = global::ASRS.Properties.Resources.black_led;

            Invalidate();*/
        }
    }
}
