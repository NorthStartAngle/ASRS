using LIBS;

namespace ASRS.Component
{
    partial class InboundOperator
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblProductPreview = new System.Windows.Forms.Label();
            this.lblAvailable = new System.Windows.Forms.Label();
            this.panLookup = new LIBS.SelectablePanel();
            this.btnShelve = new System.Windows.Forms.Button();
            this.lblPID = new MaterialSkin.Controls.MaterialLabel();
            this.lblSKU = new MaterialSkin.Controls.MaterialLabel();
            this.btnVerify = new System.Windows.Forms.Button();
            this.lstBarCodes = new System.Windows.Forms.ListBox();
            this.panel2 = new LIBS.SelectablePanel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBarcode = new System.Windows.Forms.TextBox();
            this.lbl_Time = new System.Windows.Forms.Label();
            this.panLookup.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTitle.Font = new System.Drawing.Font("Microsoft YaHei", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(493, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(747, 71);
            this.lblTitle.TabIndex = 3;
            this.lblTitle.Text = "INBOUND Product LookUp";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblProductPreview
            // 
            this.lblProductPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProductPreview.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProductPreview.Location = new System.Drawing.Point(597, 628);
            this.lblProductPreview.Name = "lblProductPreview";
            this.lblProductPreview.Size = new System.Drawing.Size(577, 233);
            this.lblProductPreview.TabIndex = 5;
            this.lblProductPreview.Text = "No Preview";
            this.lblProductPreview.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAvailable
            // 
            this.lblAvailable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAvailable.Font = new System.Drawing.Font("Microsoft YaHei", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvailable.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblAvailable.Location = new System.Drawing.Point(684, 91);
            this.lblAvailable.Name = "lblAvailable";
            this.lblAvailable.Size = new System.Drawing.Size(545, 41);
            this.lblAvailable.TabIndex = 6;
            this.lblAvailable.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panLookup
            // 
            this.panLookup.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panLookup.BackgroundImage = global::ASRS.Properties.Resources.textbox_frame1;
            this.panLookup.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panLookup.Controls.Add(this.btnShelve);
            this.panLookup.Controls.Add(this.lblPID);
            this.panLookup.Controls.Add(this.lblSKU);
            this.panLookup.Controls.Add(this.btnVerify);
            this.panLookup.Controls.Add(this.lstBarCodes);
            this.panLookup.Controls.Add(this.panel2);
            this.panLookup.Location = new System.Drawing.Point(493, 142);
            this.panLookup.MaximumSize = new System.Drawing.Size(747, 477);
            this.panLookup.MinimumSize = new System.Drawing.Size(747, 477);
            this.panLookup.Name = "panLookup";
            this.panLookup.Size = new System.Drawing.Size(747, 477);
            this.panLookup.TabIndex = 4;
            this.panLookup.TabStop = true;
            // 
            // btnShelve
            // 
            this.btnShelve.BackColor = System.Drawing.Color.Transparent;
            this.btnShelve.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnShelve.FlatAppearance.BorderSize = 2;
            this.btnShelve.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShelve.Font = new System.Drawing.Font("Microsoft YaHei UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShelve.Image = global::ASRS.Properties.Resources.log_in_sharp_icon;
            this.btnShelve.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnShelve.Location = new System.Drawing.Point(514, 371);
            this.btnShelve.Name = "btnShelve";
            this.btnShelve.Size = new System.Drawing.Size(176, 63);
            this.btnShelve.TabIndex = 18;
            this.btnShelve.Text = "Shelve";
            this.btnShelve.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnShelve.UseVisualStyleBackColor = false;
            this.btnShelve.Click += new System.EventHandler(this.btnShelve_Click);
            // 
            // lblPID
            // 
            this.lblPID.BackColor = System.Drawing.Color.Transparent;
            this.lblPID.Depth = 0;
            this.lblPID.Font = new System.Drawing.Font("Roboto", 11F);
            this.lblPID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblPID.Location = new System.Drawing.Point(430, 57);
            this.lblPID.MouseState = MaterialSkin.MouseState.HOVER;
            this.lblPID.Name = "lblPID";
            this.lblPID.Size = new System.Drawing.Size(215, 35);
            this.lblPID.TabIndex = 17;
            this.lblPID.Text = "PID";
            this.lblPID.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblSKU
            // 
            this.lblSKU.BackColor = System.Drawing.Color.Transparent;
            this.lblSKU.Depth = 0;
            this.lblSKU.Font = new System.Drawing.Font("Roboto", 11F);
            this.lblSKU.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblSKU.Location = new System.Drawing.Point(82, 57);
            this.lblSKU.MouseState = MaterialSkin.MouseState.HOVER;
            this.lblSKU.Name = "lblSKU";
            this.lblSKU.Size = new System.Drawing.Size(234, 35);
            this.lblSKU.TabIndex = 16;
            this.lblSKU.Text = "SKU";
            // 
            // btnVerify
            // 
            this.btnVerify.BackColor = System.Drawing.Color.Transparent;
            this.btnVerify.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnVerify.FlatAppearance.BorderSize = 2;
            this.btnVerify.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVerify.Font = new System.Drawing.Font("Microsoft YaHei UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVerify.Image = global::ASRS.Properties.Resources.icons8_verified_48;
            this.btnVerify.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnVerify.Location = new System.Drawing.Point(318, 371);
            this.btnVerify.Name = "btnVerify";
            this.btnVerify.Size = new System.Drawing.Size(176, 63);
            this.btnVerify.TabIndex = 15;
            this.btnVerify.Text = "Verify";
            this.btnVerify.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnVerify.UseVisualStyleBackColor = false;
            this.btnVerify.Click += new System.EventHandler(this.btnVerify_Click);
            // 
            // lstBarCodes
            // 
            this.lstBarCodes.BackColor = System.Drawing.Color.White;
            this.lstBarCodes.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lstBarCodes.Font = new System.Drawing.Font("Nirmala UI", 35F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstBarCodes.FormattingEnabled = true;
            this.lstBarCodes.ItemHeight = 62;
            this.lstBarCodes.Items.AddRange(new object[] {
            "1111111111",
            "1111111111",
            "3333333333",
            "44444444",
            "555555555"});
            this.lstBarCodes.Location = new System.Drawing.Point(233, 200);
            this.lstBarCodes.Margin = new System.Windows.Forms.Padding(5);
            this.lstBarCodes.MaximumSize = new System.Drawing.Size(500, 250);
            this.lstBarCodes.Name = "lstBarCodes";
            this.lstBarCodes.Size = new System.Drawing.Size(296, 190);
            this.lstBarCodes.TabIndex = 14;
            this.lstBarCodes.SelectedIndexChanged += new System.EventHandler(this.lstBarCodes_SelectedIndexChanged);
            this.lstBarCodes.Leave += new System.EventHandler(this.lstBarCodes_Leave);
            // 
            // panel2
            // 
            this.panel2.BackgroundImage = global::ASRS.Properties.Resources.textbox_border_2;
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.txtBarcode);
            this.panel2.Location = new System.Drawing.Point(171, 118);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(388, 80);
            this.panel2.TabIndex = 0;
            this.panel2.TabStop = true;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(17, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 54);
            this.label1.TabIndex = 19;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // txtBarcode
            // 
            this.txtBarcode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBarcode.BackColor = System.Drawing.Color.White;
            this.txtBarcode.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtBarcode.Font = new System.Drawing.Font("Nirmala UI", 35F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBarcode.Location = new System.Drawing.Point(63, 10);
            this.txtBarcode.MaxLength = 12;
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.Size = new System.Drawing.Size(282, 63);
            this.txtBarcode.TabIndex = 13;
            this.txtBarcode.Text = "343423432";
            // 
            // lbl_Time
            // 
            this.lbl_Time.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_Time.BackColor = System.Drawing.Color.Transparent;
            this.lbl_Time.Font = new System.Drawing.Font("Bahnschrift SemiBold Condensed", 40F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Time.Location = new System.Drawing.Point(1212, 0);
            this.lbl_Time.Name = "lbl_Time";
            this.lbl_Time.Size = new System.Drawing.Size(529, 74);
            this.lbl_Time.TabIndex = 2;
            this.lbl_Time.Text = "2323";
            this.lbl_Time.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // InboundOperator
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Controls.Add(this.lblAvailable);
            this.Controls.Add(this.lblProductPreview);
            this.Controls.Add(this.panLookup);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lbl_Time);
            this.DoubleBuffered = true;
            this.Name = "InboundOperator";
            this.Padding = new System.Windows.Forms.Padding(20);
            this.Size = new System.Drawing.Size(1741, 909);
            this.Style = MetroFramework.MetroColorStyle.Silver;
            this.panLookup.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblTitle;
        private SelectablePanel panLookup;
        private SelectablePanel panel2;
        private System.Windows.Forms.TextBox txtBarcode;
        private System.Windows.Forms.ListBox lstBarCodes;
        private System.Windows.Forms.Button btnVerify;
        private System.Windows.Forms.Button btnShelve;
        private System.Windows.Forms.Label lblProductPreview;
        private MaterialSkin.Controls.MaterialLabel lblPID;
        private MaterialSkin.Controls.MaterialLabel lblSKU;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblAvailable;
        private System.Windows.Forms.Label lbl_Time;
    }
}
