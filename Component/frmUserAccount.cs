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
                await Task.Delay(500);
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

                        if (ct == 0)
                        {
                            ZeroUsers(false, "There is no registered user"); return;
                        }
                        //stateChanged?.Invoke(this, new UserAccountEventArg(UserAccountEventSubject.ready));
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
                this.Invoke(new MethodInvoker(delegate () {lbl_status.Text = p; }));
            }
            else
            {
                lbl_status.Text = p;
            }
        }

        private void OnApply_clicked(object sender, EventArgs e)
        {
            string strQuery = $"select * from user_list where username ='{txt_name.Text.Trim()}' and password='{txt_pwd.Text.Trim()}'";

            Manager.db.RunQueryWithCallBack(strQuery, (OleDbDataReader reader) =>
            {
                if (reader == null)
                {
                    ZeroUsers(false, "Operator didn't failed while database manipulated"); return;
                }

                try
                {
                    if(reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int id = SafeGetMethods.SafeGetInt(reader, 0);
                            string username = SafeGetMethods.SafeGetString(reader, 1);
                            string password = SafeGetMethods.SafeGetString(reader, 2);
                            int access_level = (int)SafeGetMethods.SafeGetInt(reader, 3);

                            ZeroUsers(true, "Please wait while login");

                            USER user = new USER();
                            user.ID = id;
                            user.username = username;
                            user.password = password;
                            user.access_level = access_level;

                            stateChanged?.Invoke(this, new UserAccountEventArg(UserAccountEventSubject.apply,"",user));
                            break;
                        }
                    }
                    else
                    {
                        ZeroUsers(true, "The input user information don't be correctly, Please re-input!");
                        txt_name.Clear();
                        txt_pwd.Clear();
                        txt_name.Focus();
                    }                    
                }
                catch (Exception)
                {

                }

                reader?.Close();
            });
        }
    }
}
