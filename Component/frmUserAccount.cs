using ASRS.libs;
using LIB;
using MaterialSkin;
using MaterialSkin.Controls;
using MetroFramework.Components;
using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASRS.Component
{
    public partial class frmUserAccount : MetroFramework.Controls.MetroUserControl
    {
        public event EventHandler<UserAccountEventArg> stateChanged;

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

        public void initialize()
        {
            Task.Run(async () => {
                await Task.Delay(1000);
                stateChanged?.Invoke(this, new UserAccountEventArg(UserAccountEventSubject.ready));
                getUserIsExisted();
            });
        }

        private void getUserIsExisted()
        {
            Manager.db.RunQueryWithCallBack("select count(*) from user_list", (OleDbDataReader reader) =>
            {
                if (reader == null)
                {
                    ZeroUsers(false, "No registered user"); return;
                }

                try
                {
                    while (reader.Read())
                    {
                        int ct = SafeGetMethods.SafeGetInt(reader, 0);
                        ZeroUsers(ct > 0, "");
                    }
                }
                catch (Exception)
                {

                }

                reader?.Close();
            });
        }

        public void ZeroUsers(bool status,string p="..")
        {
            if(lbl_status.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate () { txt_name.Enabled = txt_pwd.Enabled = status; lbl_status.Text = p; }));
            }
            else
            {
                txt_name.Enabled = txt_pwd.Enabled = status; lbl_status.Text = p;
            }
        }
    }
}
