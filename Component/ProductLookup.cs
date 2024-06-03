using ASRS.libs;
using LIB;
using MaterialSkin;
using MaterialSkin.Controls;
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
    public partial class frmProductLookup : MetroForm
    {
        public event EventHandler<DialogEventArgs> status;
        public frmProductLookup()
        {
            InitializeComponent();

            /*MaterialSkinManager materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;

            // Configure color schema
            materialSkinManager.ColorScheme = new ColorScheme(
                Primary.Blue400, Primary.Blue500,
                Primary.Blue500, Accent.LightBlue200,
                TextShade.WHITE
            );*/
        }

        private void frmProductLookup_Shown(object sender, EventArgs e)
        {
            Manager _owner = (Manager)Manager.AppOwner;
            _owner.stateChanged(this,new DialogEventArgs("",DialogEventReason.showing));
        }

        private void frmProductLookup_FormClosing(object sender, FormClosingEventArgs e)
        {
            Manager _owner = (Manager)Manager.AppOwner;
            _owner.stateChanged(this, new DialogEventArgs("", DialogEventReason.close));
        }
    }
}
