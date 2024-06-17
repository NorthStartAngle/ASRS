using ASRS.libs;
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
    public partial class UserItem : UserControl
    {
        public UserItemEvents dispatcher;

        USER _user;
        public UserItem()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.Selectable, true);
        }

        public UserItem(USER user) : this()
        {
            userInfo = user;
        }

        public USER userInfo
        {

            get { return _user; }
            set { _user = value; loadInfo(); }
        }

        private void loadInfo()
        {
            if (userInfo != null)
            {
                lblCaption.Text = userInfo.username;
                if (userInfo.avatar != null)
                {
                    try
                    {
                        lblAvatar.Image = Image.FromFile(userInfo.avatar);
                    }
                    catch (Exception)
                    {}                    
                }
                switch (userInfo.access_level)
                {
                    case 1:
                        lblAvatar.Text = "admin";
                        break;
                    case 2:
                        lblAvatar.Text = "inbound operator";
                        break;
                    case 3:
                        lblAvatar.Text = "outbound operator";
                        break;
                    default:
                        lblAvatar.Text = "unknown";
                        break;
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.Focus();
            base.OnMouseDown(e);
        }

        protected override bool IsInputKey(Keys keyData)
        {
            if (keyData == Keys.Up || keyData == Keys.Down) return true;
            if (keyData == Keys.Left || keyData == Keys.Right) return true;
            return base.IsInputKey(keyData);
        }

        protected override void OnEnter(EventArgs e)
        {
            this.Invalidate();
            base.OnEnter(e);
        }
        protected override void OnLeave(EventArgs e)
        {
            this.Invalidate();
            base.OnLeave(e);
        }
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            if (this.Focused)
            {
                var rc = this.ClientRectangle;
                rc.Inflate(-2, -2);
                ControlPaint.DrawFocusRectangle(pe.Graphics, rc);
            }
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            if (_user == null) return;
            dispatcher?.HandleChangeRequest(this, _user);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (_user == null) return;
            dispatcher?.HandleRemoveRequest(this,_user);
        }

        private void UserItem_Leave(object sender, EventArgs e)
        {
            if (_user == null) return;
            dispatcher?.HandleItemLeaved(this);
        }

        private void UserItem_Enter(object sender, EventArgs e)
        {
            if (_user == null) return;
            dispatcher?.HandleItemSelected(this);
        }
    }
}
