using ASRS.libs;
using MetroFramework.Components;
using Microsoft.Vbe.Interop.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASRS.Component
{
    public partial class InboundOperator : MetroFramework.Controls.MetroUserControl
    {
        public event EventHandler<Object> stateChanged;
        System.Windows.Forms.Timer timer;

        public int OnTimerTick { get; }

        int ct = 0;
        public InboundOperator(MetroStyleManager styler = null)
        {
            InitializeComponent();

            if(styler != null )
            {
                MetroStyleManager style = new MetroStyleManager();
                style.Owner = this;

                style.Style = styler.Style;
                this.StyleManager = style;
            }

            lbl_Time.Text = DateTime.Now.ToString("h:mm tt MMMM d,yyyy");

            timer = new System.Windows.Forms.Timer();
            timer.Tick += onTimerTick;
            timer.Interval = 1000;
            timer.Start();
            picZPA.Enabled = false;
        }

        public void initialize()
        {

        }

        private void onTimerTick(object sender, EventArgs e)
        {
            lbl_Time.Text = DateTime.Now.ToString("h:mm tt MMMM d,yyyy");

            
            ct += 1;
            if(ct>2)
            {
                picZPA.Enabled = true;
            }
            if( ct>5 )
            {
                ct = -1000;
                OnWakeUpSensor1_ON();
            }
        }

        //ZPA conveyor notification
        private void OnWakeUpSensor1_ON()
        {
            frmProductLookup pl = new frmProductLookup();
            pl.Owner = Manager.AppOwner;
            pl.ShowDialog();
        }
    }
}
