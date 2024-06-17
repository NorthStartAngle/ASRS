namespace ASRS.Component
{
    partial class userManager
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Label lbl_password;
            System.Windows.Forms.Label lbl_name;
            this.panUsers = new System.Windows.Forms.Panel();
            this.panUserDetails = new System.Windows.Forms.Panel();
            this.txt_name = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.txt_pwd = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.btnApply = new MaterialSkin.Controls.MaterialRaisedButton();
            lbl_password = new System.Windows.Forms.Label();
            lbl_name = new System.Windows.Forms.Label();
            this.panUserDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // panUsers
            // 
            this.panUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panUsers.AutoScroll = true;
            this.panUsers.Location = new System.Drawing.Point(95, 61);
            this.panUsers.Name = "panUsers";
            this.panUsers.Size = new System.Drawing.Size(579, 693);
            this.panUsers.TabIndex = 0;
            // 
            // panUserDetails
            // 
            this.panUserDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panUserDetails.Controls.Add(this.txt_name);
            this.panUserDetails.Controls.Add(this.txt_pwd);
            this.panUserDetails.Controls.Add(lbl_password);
            this.panUserDetails.Controls.Add(lbl_name);
            this.panUserDetails.Controls.Add(this.btnApply);
            this.panUserDetails.Location = new System.Drawing.Point(690, 61);
            this.panUserDetails.Name = "panUserDetails";
            this.panUserDetails.Size = new System.Drawing.Size(546, 693);
            this.panUserDetails.TabIndex = 1;
            // 
            // txt_name
            // 
            this.txt_name.BackgroundImage = global::ASRS.Properties.Resources.textbox_frame1;
            this.txt_name.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.txt_name.Depth = 0;
            this.txt_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_name.ForeColor = System.Drawing.Color.Yellow;
            this.txt_name.Hint = "";
            this.txt_name.Location = new System.Drawing.Point(248, 252);
            this.txt_name.MaxLength = 20;
            this.txt_name.MouseState = MaterialSkin.MouseState.HOVER;
            this.txt_name.Name = "txt_name";
            this.txt_name.PasswordChar = '\0';
            this.txt_name.SelectedText = "";
            this.txt_name.SelectionLength = 0;
            this.txt_name.SelectionStart = 0;
            this.txt_name.Size = new System.Drawing.Size(250, 23);
            this.txt_name.TabIndex = 21;
            this.txt_name.TabStop = false;
            this.txt_name.UseSystemPasswordChar = false;
            // 
            // txt_pwd
            // 
            this.txt_pwd.Depth = 0;
            this.txt_pwd.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_pwd.ForeColor = System.Drawing.Color.Yellow;
            this.txt_pwd.Hint = "";
            this.txt_pwd.Location = new System.Drawing.Point(248, 327);
            this.txt_pwd.MaxLength = 20;
            this.txt_pwd.MouseState = MaterialSkin.MouseState.HOVER;
            this.txt_pwd.Name = "txt_pwd";
            this.txt_pwd.PasswordChar = '*';
            this.txt_pwd.SelectedText = "";
            this.txt_pwd.SelectionLength = 0;
            this.txt_pwd.SelectionStart = 0;
            this.txt_pwd.Size = new System.Drawing.Size(250, 23);
            this.txt_pwd.TabIndex = 22;
            this.txt_pwd.TabStop = false;
            this.txt_pwd.UseSystemPasswordChar = true;
            // 
            // lbl_password
            // 
            lbl_password.BackColor = System.Drawing.Color.Transparent;
            lbl_password.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lbl_password.ForeColor = System.Drawing.Color.PaleVioletRed;
            lbl_password.Image = global::ASRS.Properties.Resources.locks;
            lbl_password.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            lbl_password.Location = new System.Drawing.Point(48, 323);
            lbl_password.Name = "lbl_password";
            lbl_password.Size = new System.Drawing.Size(142, 37);
            lbl_password.TabIndex = 20;
            lbl_password.Text = "password ";
            lbl_password.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_name
            // 
            lbl_name.BackColor = System.Drawing.Color.Transparent;
            lbl_name.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lbl_name.ForeColor = System.Drawing.Color.PaleVioletRed;
            lbl_name.Image = global::ASRS.Properties.Resources.user;
            lbl_name.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            lbl_name.Location = new System.Drawing.Point(48, 247);
            lbl_name.Name = "lbl_name";
            lbl_name.Size = new System.Drawing.Size(142, 37);
            lbl_name.TabIndex = 19;
            lbl_name.Text = "user name";
            lbl_name.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnApply
            // 
            this.btnApply.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnApply.AutoSize = true;
            this.btnApply.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnApply.Depth = 0;
            this.btnApply.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApply.Icon = null;
            this.btnApply.Location = new System.Drawing.Point(405, 410);
            this.btnApply.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnApply.Name = "btnApply";
            this.btnApply.Primary = true;
            this.btnApply.Size = new System.Drawing.Size(67, 36);
            this.btnApply.TabIndex = 23;
            this.btnApply.Text = " Apply ";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // userManager
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panUserDetails);
            this.Controls.Add(this.panUsers);
            this.Name = "userManager";
            this.Size = new System.Drawing.Size(1302, 819);
            this.panUserDetails.ResumeLayout(false);
            this.panUserDetails.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panUsers;
        private System.Windows.Forms.Panel panUserDetails;
        private MaterialSkin.Controls.MaterialSingleLineTextField txt_name;
        private MaterialSkin.Controls.MaterialSingleLineTextField txt_pwd;
        private MaterialSkin.Controls.MaterialRaisedButton btnApply;
    }
}
