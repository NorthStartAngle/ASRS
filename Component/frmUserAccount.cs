using LIB;
using MaterialSkin;
using MaterialSkin.Controls;
using MetroFramework.Components;
using MetroFramework.Forms;
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
    public partial class frmUserAccount : MetroFramework.Controls.MetroUserControl
    {
        public frmUserAccount(MetroStyleManager styler)
        {
            InitializeComponent();

            if(styler != null )
            {                
                styleManager1.Style = styler.Style;
                //styleManager1.Theme = styler.Theme;

                this.StyleManager = styleManager1;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
