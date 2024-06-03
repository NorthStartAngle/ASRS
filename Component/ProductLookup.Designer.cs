namespace ASRS.Component
{
    partial class frmProductLookup
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
            MaterialSkin.Controls.MaterialLabel materialLabel1;
            MaterialSkin.Controls.MaterialLabel materialLabel3;
            MaterialSkin.Controls.MaterialLabel lbl_SKU;
            MaterialSkin.Controls.MaterialLabel lbl_PID;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProductLookup));
            this.materialLabel2 = new MaterialSkin.Controls.MaterialLabel();
            this.btn_Verify = new MaterialSkin.Controls.MaterialRaisedButton();
            this.btn_Shelve = new MaterialSkin.Controls.MaterialRaisedButton();
            this.metroTextBox1 = new MetroFramework.Controls.MetroTextBox();
            materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            materialLabel3 = new MaterialSkin.Controls.MaterialLabel();
            lbl_SKU = new MaterialSkin.Controls.MaterialLabel();
            lbl_PID = new MaterialSkin.Controls.MaterialLabel();
            this.SuspendLayout();
            // 
            // materialLabel1
            // 
            materialLabel1.BackColor = System.Drawing.Color.Transparent;
            materialLabel1.Depth = 0;
            materialLabel1.Font = new System.Drawing.Font("Roboto", 11F);
            materialLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            materialLabel1.Location = new System.Drawing.Point(109, 139);
            materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            materialLabel1.Name = "materialLabel1";
            materialLabel1.Size = new System.Drawing.Size(38, 29);
            materialLabel1.TabIndex = 0;
            materialLabel1.Text = "SKU";
            // 
            // materialLabel3
            // 
            materialLabel3.BackColor = System.Drawing.Color.Transparent;
            materialLabel3.Depth = 0;
            materialLabel3.Font = new System.Drawing.Font("Roboto", 11F);
            materialLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            materialLabel3.Location = new System.Drawing.Point(228, 192);
            materialLabel3.MouseState = MaterialSkin.MouseState.HOVER;
            materialLabel3.Name = "materialLabel3";
            materialLabel3.Size = new System.Drawing.Size(149, 29);
            materialLabel3.TabIndex = 2;
            materialLabel3.Text = "BarCode";
            materialLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_SKU
            // 
            lbl_SKU.BackColor = System.Drawing.Color.WhiteSmoke;
            lbl_SKU.Depth = 0;
            lbl_SKU.Font = new System.Drawing.Font("Roboto", 11F);
            lbl_SKU.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            lbl_SKU.Location = new System.Drawing.Point(75, 103);
            lbl_SKU.MouseState = MaterialSkin.MouseState.HOVER;
            lbl_SKU.Name = "lbl_SKU";
            lbl_SKU.Size = new System.Drawing.Size(107, 36);
            lbl_SKU.TabIndex = 5;
            lbl_SKU.Text = "A08";
            lbl_SKU.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_PID
            // 
            lbl_PID.BackColor = System.Drawing.Color.WhiteSmoke;
            lbl_PID.Depth = 0;
            lbl_PID.Font = new System.Drawing.Font("Roboto", 11F);
            lbl_PID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            lbl_PID.Location = new System.Drawing.Point(418, 103);
            lbl_PID.MouseState = MaterialSkin.MouseState.HOVER;
            lbl_PID.Name = "lbl_PID";
            lbl_PID.Size = new System.Drawing.Size(107, 36);
            lbl_PID.TabIndex = 6;
            lbl_PID.Text = "75541";
            lbl_PID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // materialLabel2
            // 
            this.materialLabel2.BackColor = System.Drawing.Color.Transparent;
            this.materialLabel2.Depth = 0;
            this.materialLabel2.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel2.Location = new System.Drawing.Point(454, 139);
            this.materialLabel2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel2.Name = "materialLabel2";
            this.materialLabel2.Size = new System.Drawing.Size(39, 29);
            this.materialLabel2.TabIndex = 1;
            this.materialLabel2.Text = "PID";
            // 
            // btn_Verify
            // 
            this.btn_Verify.AutoSize = true;
            this.btn_Verify.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btn_Verify.Depth = 0;
            this.btn_Verify.Font = new System.Drawing.Font("MS Outlook", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btn_Verify.Icon = null;
            this.btn_Verify.Location = new System.Drawing.Point(157, 268);
            this.btn_Verify.MouseState = MaterialSkin.MouseState.HOVER;
            this.btn_Verify.Name = "btn_Verify";
            this.btn_Verify.Primary = true;
            this.btn_Verify.Size = new System.Drawing.Size(69, 36);
            this.btn_Verify.TabIndex = 3;
            this.btn_Verify.Text = " VERIFY ";
            this.btn_Verify.UseMnemonic = false;
            this.btn_Verify.UseVisualStyleBackColor = false;
            // 
            // btn_Shelve
            // 
            this.btn_Shelve.AutoSize = true;
            this.btn_Shelve.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btn_Shelve.Depth = 0;
            this.btn_Shelve.Font = new System.Drawing.Font("MS Outlook", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btn_Shelve.Icon = null;
            this.btn_Shelve.Location = new System.Drawing.Point(368, 268);
            this.btn_Shelve.MouseState = MaterialSkin.MouseState.HOVER;
            this.btn_Shelve.Name = "btn_Shelve";
            this.btn_Shelve.Primary = true;
            this.btn_Shelve.Size = new System.Drawing.Size(74, 36);
            this.btn_Shelve.TabIndex = 4;
            this.btn_Shelve.Text = " SHELVE  ";
            this.btn_Shelve.UseMnemonic = false;
            this.btn_Shelve.UseVisualStyleBackColor = false;
            // 
            // metroTextBox1
            // 
            // 
            // 
            // 
            this.metroTextBox1.CustomButton.Image = null;
            this.metroTextBox1.CustomButton.Location = new System.Drawing.Point(121, 2);
            this.metroTextBox1.CustomButton.Name = "";
            this.metroTextBox1.CustomButton.Size = new System.Drawing.Size(35, 35);
            this.metroTextBox1.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBox1.CustomButton.TabIndex = 1;
            this.metroTextBox1.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBox1.CustomButton.UseSelectable = true;
            this.metroTextBox1.CustomButton.Visible = false;
            this.metroTextBox1.FontSize = MetroFramework.MetroTextBoxSize.Tall;
            this.metroTextBox1.FontWeight = MetroFramework.MetroTextBoxWeight.Bold;
            this.metroTextBox1.Lines = new string[0];
            this.metroTextBox1.Location = new System.Drawing.Point(218, 149);
            this.metroTextBox1.MaxLength = 20;
            this.metroTextBox1.Name = "metroTextBox1";
            this.metroTextBox1.PasswordChar = '\0';
            this.metroTextBox1.PromptText = "11223655";
            this.metroTextBox1.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBox1.SelectedText = "";
            this.metroTextBox1.SelectionLength = 0;
            this.metroTextBox1.SelectionStart = 0;
            this.metroTextBox1.ShortcutsEnabled = true;
            this.metroTextBox1.Size = new System.Drawing.Size(159, 40);
            this.metroTextBox1.TabIndex = 7;
            this.metroTextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.metroTextBox1.UseSelectable = true;
            this.metroTextBox1.WaterMark = "11223655";
            this.metroTextBox1.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBox1.WaterMarkFont = new System.Drawing.Font("Segoe UI", 14F);
            // 
            // frmProductLookup
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(595, 333);
            this.Controls.Add(this.metroTextBox1);
            this.Controls.Add(lbl_PID);
            this.Controls.Add(lbl_SKU);
            this.Controls.Add(this.btn_Shelve);
            this.Controls.Add(this.btn_Verify);
            this.Controls.Add(materialLabel3);
            this.Controls.Add(this.materialLabel2);
            this.Controls.Add(materialLabel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmProductLookup";
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.DropShadow;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Style = MetroFramework.MetroColorStyle.Magenta;
            this.Text = "INBOUND Product LookUp";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmProductLookup_FormClosing);
            this.Shown += new System.EventHandler(this.frmProductLookup_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private MaterialSkin.Controls.MaterialLabel materialLabel2;
        private MaterialSkin.Controls.MaterialRaisedButton btn_Verify;
        private MaterialSkin.Controls.MaterialRaisedButton btn_Shelve;
        private MetroFramework.Controls.MetroTextBox metroTextBox1;
    }
}
