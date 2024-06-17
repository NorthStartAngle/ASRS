using LIBS;
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
    public partial class userManager : UserControl
    {
        UserItem curUI = null;
        public userManager()
        {
            InitializeComponent();
        }

        private void loadUsers()
        {
            panUsers.Controls.Clear();

            Manager.db.RunQueryWithCallBack("select count(*) from USER_LOOKUP", (OleDbDataReader reader) =>
            {
                if (reader == null)
                {
                    panUserDetails.Visible = false;
                    panUsers.Visible = false;
                    return;
                }

                try
                {
                    int ct = 0;
                    while (reader.Read())
                    {
                        int id = SafeGetMethods.SafeGetInt(reader, 0);
                        string username = SafeGetMethods.SafeGetString(reader, 1);
                        string password = SafeGetMethods.SafeGetString(reader, 2);
                        int access_level = (int)SafeGetMethods.SafeGetInt(reader, 3);


                        USER user = new USER();
                        user.ID = id;
                        user.username = username;
                        user.password = password;
                        user.access_level = access_level;

                        UserItem  userItem = new UserItem(user);
                        if(ct == 0)
                        {
                            userItem.SetBounds(10, ct * 110 + 10, panUsers.Width-20, 110);
                        }
                        else
                        {
                            userItem.SetBounds(10, ct * 110 + 10 + 10*(ct-1), panUsers.Width-20, 110);
                        }

                        userItem.dispatcher.UserItemChangeRequest += User_ChangeRequest;
                        userItem.dispatcher.UserItemRemoveRequest += User_RemoveRequest;
                        userItem.dispatcher.UserItemSelected += User_Selected;
                        userItem.dispatcher.UserItemDeselected += User_Deselected;
                        panUsers.Controls.Add(userItem);
                    }
                }
                catch (Exception)
                {

                }

                reader?.Close();
            });
        }

        private void User_Deselected(object sender, EventArgs e)
        {
            panUserDetails.Visible = false;
        }

        private void User_Selected(object sender, EventArgs e)
        {
            curUI = (UserItem)sender;
            txt_name.Text = curUI.userInfo.username;
            txt_pwd.Text = curUI.userInfo.password;
        }

        private void User_RemoveRequest(object sender, USER e)
        {
            panUsers.Controls.Remove((Control)sender);
        }

        private void User_ChangeRequest(object sender, USER e)
        {
            panUserDetails.Visible = true;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if(curUI != null)
            {
                USER user = new USER()
                {
                    ID = curUI.userInfo.ID,
                    username = txt_name.Text,
                    password = txt_pwd.Text,
                    access_level = curUI.userInfo.access_level,
                    avatar = curUI.userInfo.avatar,
                };
                
                curUI.userInfo = user;
                curUI.userInfo.save(Manager.db);
                panUserDetails.Visible=false;
            }
        }
    }
}
