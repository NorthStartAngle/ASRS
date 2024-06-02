using ASRS.libs;
using MetroFramework.Components;
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
    public partial class InboundOperator : MetroFramework.Controls.MetroUserControl
    {
        public event EventHandler<Object> stateChanged;

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
        }

        public void initialize()
        {

        }
    }
}
