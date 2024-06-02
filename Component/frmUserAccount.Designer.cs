namespace ASRS.Component
{
    partial class frmUserAccount
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label lbl_password;
            System.Windows.Forms.Label lbl_name;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUserAccount));
            this.btnApply = new MaterialSkin.Controls.MaterialRaisedButton();
            this.btnDismiss = new MaterialSkin.Controls.MaterialRaisedButton();
            this.styleManager1 = new MetroFramework.Components.MetroStyleManager(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.txt_pwd = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.txt_name = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lbl_status = new System.Windows.Forms.Label();
            lbl_password = new System.Windows.Forms.Label();
            lbl_name = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.styleManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_password
            // 
            lbl_password.BackColor = System.Drawing.Color.Transparent;
            lbl_password.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lbl_password.ForeColor = System.Drawing.Color.PaleVioletRed;
            lbl_password.Image = global::ASRS.Properties.Resources.user;
            lbl_password.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            lbl_password.Location = new System.Drawing.Point(171, 428);
            lbl_password.Name = "lbl_password";
            lbl_password.Size = new System.Drawing.Size(142, 37);
            lbl_password.TabIndex = 14;
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
            lbl_name.Location = new System.Drawing.Point(171, 352);
            lbl_name.Name = "lbl_name";
            lbl_name.Size = new System.Drawing.Size(142, 37);
            lbl_name.TabIndex = 13;
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
            this.btnApply.Location = new System.Drawing.Point(528, 515);
            this.btnApply.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnApply.Name = "btnApply";
            this.btnApply.Primary = true;
            this.btnApply.Size = new System.Drawing.Size(67, 36);
            this.btnApply.TabIndex = 18;
            this.btnApply.Text = " Apply ";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.OnApply_clicked);
            // 
            // btnDismiss
            // 
            this.btnDismiss.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnDismiss.AutoSize = true;
            this.btnDismiss.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnDismiss.Depth = 0;
            this.btnDismiss.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDismiss.Icon = null;
            this.btnDismiss.Location = new System.Drawing.Point(655, 515);
            this.btnDismiss.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnDismiss.Name = "btnDismiss";
            this.btnDismiss.Primary = true;
            this.btnDismiss.Size = new System.Drawing.Size(78, 36);
            this.btnDismiss.TabIndex = 19;
            this.btnDismiss.Text = " Dismiss ";
            this.btnDismiss.UseVisualStyleBackColor = true;
            // 
            // styleManager1
            // 
            this.styleManager1.Owner = this;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("MS Reference Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.PaleVioletRed;
            this.label1.Location = new System.Drawing.Point(123, 230);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(535, 93);
            this.label1.TabIndex = 12;
            this.label1.Text = "Please login with your Authentication";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_pwd
            // 
            this.txt_pwd.Depth = 0;
            this.txt_pwd.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_pwd.ForeColor = System.Drawing.Color.Yellow;
            this.txt_pwd.Hint = "";
            this.txt_pwd.Location = new System.Drawing.Point(371, 432);
            this.txt_pwd.MaxLength = 20;
            this.txt_pwd.MouseState = MaterialSkin.MouseState.HOVER;
            this.txt_pwd.Name = "txt_pwd";
            this.txt_pwd.PasswordChar = '*';
            this.txt_pwd.SelectedText = "";
            this.txt_pwd.SelectionLength = 0;
            this.txt_pwd.SelectionStart = 0;
            this.txt_pwd.Size = new System.Drawing.Size(250, 23);
            this.txt_pwd.TabIndex = 17;
            this.txt_pwd.TabStop = false;
            this.txt_pwd.UseSystemPasswordChar = true;
            // 
            // txt_name
            // 
            this.txt_name.BackgroundImage = global::ASRS.Properties.Resources.textbox_frame1;
            this.txt_name.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.txt_name.Depth = 0;
            this.txt_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_name.ForeColor = System.Drawing.Color.Yellow;
            this.txt_name.Hint = "";
            this.txt_name.Location = new System.Drawing.Point(371, 357);
            this.txt_name.MaxLength = 20;
            this.txt_name.MouseState = MaterialSkin.MouseState.HOVER;
            this.txt_name.Name = "txt_name";
            this.txt_name.PasswordChar = '\0';
            this.txt_name.SelectedText = "";
            this.txt_name.SelectionLength = 0;
            this.txt_name.SelectionStart = 0;
            this.txt_name.Size = new System.Drawing.Size(250, 23);
            this.txt_name.TabIndex = 16;
            this.txt_name.TabStop = false;
            this.txt_name.UseSystemPasswordChar = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Location = new System.Drawing.Point(303, 20);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(202, 168);
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            // 
            // lbl_status
            // 
            this.lbl_status.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lbl_status.BackColor = System.Drawing.Color.Transparent;
            this.lbl_status.Font = new System.Drawing.Font("Myanmar Text", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_status.ForeColor = System.Drawing.Color.Peru;
            this.lbl_status.Location = new System.Drawing.Point(0, 736);
            this.lbl_status.Name = "lbl_status";
            this.lbl_status.Size = new System.Drawing.Size(815, 32);
            this.lbl_status.TabIndex = 20;
            this.lbl_status.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmUserAccount
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Controls.Add(this.lbl_status);
            this.Controls.Add(this.txt_name);
            this.Controls.Add(this.txt_pwd);
            this.Controls.Add(lbl_password);
            this.Controls.Add(lbl_name);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnDismiss);
            this.Controls.Add(this.btnApply);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "frmUserAccount";
            this.Padding = new System.Windows.Forms.Padding(20, 5, 5, 5);
            this.Size = new System.Drawing.Size(815, 772);
            this.Style = MetroFramework.MetroColorStyle.Orange;
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.UseCustomBackColor = true;
            this.UseCustomForeColor = true;
            ((System.ComponentModel.ISupportInitialize)(this.styleManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private MaterialSkin.Controls.MaterialRaisedButton btnApply;
        private MaterialSkin.Controls.MaterialRaisedButton btnDismiss;
        private MetroFramework.Components.MetroStyleManager styleManager1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private MaterialSkin.Controls.MaterialSingleLineTextField txt_pwd;
        private MaterialSkin.Controls.MaterialSingleLineTextField txt_name;
        private System.Windows.Forms.Label lbl_status;
    }
}
