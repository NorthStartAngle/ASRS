namespace ASRS.Component
{
    partial class Manager
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Manager));
            this.managerStyle = new MetroFramework.Components.MetroStyleManager(this.components);
            this.bodyLayout = new System.Windows.Forms.TableLayoutPanel();
            this.panStatus = new System.Windows.Forms.Panel();
            this.picGeckoIndicator = new System.Windows.Forms.PictureBox();
            this.picZPAIndicator = new System.Windows.Forms.PictureBox();
            this.lblTradeMark = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.managerStyle)).BeginInit();
            this.panStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picGeckoIndicator)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picZPAIndicator)).BeginInit();
            this.SuspendLayout();
            // 
            // managerStyle
            // 
            this.managerStyle.Owner = this;
            this.managerStyle.Style = MetroFramework.MetroColorStyle.Lime;
            this.managerStyle.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // bodyLayout
            // 
            this.bodyLayout.BackColor = System.Drawing.Color.White;
            this.bodyLayout.ColumnCount = 3;
            this.bodyLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.bodyLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 400F));
            this.bodyLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.bodyLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bodyLayout.Location = new System.Drawing.Point(0, 80);
            this.bodyLayout.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.bodyLayout.Name = "bodyLayout";
            this.bodyLayout.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.bodyLayout.RowCount = 3;
            this.bodyLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.bodyLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.bodyLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.bodyLayout.Size = new System.Drawing.Size(779, 562);
            this.bodyLayout.TabIndex = 1;
            // 
            // panStatus
            // 
            this.panStatus.BackColor = System.Drawing.Color.White;
            this.panStatus.Controls.Add(this.picGeckoIndicator);
            this.panStatus.Controls.Add(this.picZPAIndicator);
            this.panStatus.Controls.Add(this.lblTradeMark);
            this.panStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panStatus.Location = new System.Drawing.Point(0, 602);
            this.panStatus.MaximumSize = new System.Drawing.Size(2000, 40);
            this.panStatus.MinimumSize = new System.Drawing.Size(200, 40);
            this.panStatus.Name = "panStatus";
            this.panStatus.Size = new System.Drawing.Size(779, 40);
            this.panStatus.TabIndex = 2;
            // 
            // picGeckoIndicator
            // 
            this.picGeckoIndicator.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picGeckoIndicator.BackColor = System.Drawing.Color.Transparent;
            this.picGeckoIndicator.Image = global::ASRS.Properties.Resources.connection_1;
            this.picGeckoIndicator.Location = new System.Drawing.Point(669, 5);
            this.picGeckoIndicator.Margin = new System.Windows.Forms.Padding(0);
            this.picGeckoIndicator.Name = "picGeckoIndicator";
            this.picGeckoIndicator.Size = new System.Drawing.Size(30, 30);
            this.picGeckoIndicator.TabIndex = 2;
            this.picGeckoIndicator.TabStop = false;
            // 
            // picZPAIndicator
            // 
            this.picZPAIndicator.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picZPAIndicator.Image = global::ASRS.Properties.Resources.connection_2;
            this.picZPAIndicator.Location = new System.Drawing.Point(562, 5);
            this.picZPAIndicator.Margin = new System.Windows.Forms.Padding(0);
            this.picZPAIndicator.Name = "picZPAIndicator";
            this.picZPAIndicator.Size = new System.Drawing.Size(30, 30);
            this.picZPAIndicator.TabIndex = 1;
            this.picZPAIndicator.TabStop = false;
            // 
            // lblTradeMark
            // 
            this.lblTradeMark.AutoSize = true;
            this.lblTradeMark.BackColor = System.Drawing.Color.Transparent;
            this.lblTradeMark.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTradeMark.ForeColor = System.Drawing.Color.Coral;
            this.lblTradeMark.Location = new System.Drawing.Point(9, 5);
            this.lblTradeMark.Name = "lblTradeMark";
            this.lblTradeMark.Size = new System.Drawing.Size(299, 22);
            this.lblTradeMark.TabIndex = 0;
            this.lblTradeMark.Text = "Copyright@ 2024- ASRS Canada Company";
            this.lblTradeMark.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Manager
            // 
            this.ApplyImageInvert = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BackMaxSize = 20;
            this.ClientSize = new System.Drawing.Size(779, 642);
            this.Controls.Add(this.panStatus);
            this.Controls.Add(this.bodyLayout);
            this.Font = new System.Drawing.Font("Poor Richard", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Manager";
            this.Padding = new System.Windows.Forms.Padding(0, 80, 0, 0);
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.AeroShadow;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Style = MetroFramework.MetroColorStyle.Brown;
            this.Text = "ASRS Manager";
            this.TransparencyKey = System.Drawing.Color.DarkMagenta;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.managerStyle)).EndInit();
            this.panStatus.ResumeLayout(false);
            this.panStatus.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picGeckoIndicator)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picZPAIndicator)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public MetroFramework.Components.MetroStyleManager managerStyle;
        private System.Windows.Forms.TableLayoutPanel bodyLayout;
        private System.Windows.Forms.Panel panStatus;
        private System.Windows.Forms.Label lblTradeMark;
        private System.Windows.Forms.PictureBox picGeckoIndicator;
        private System.Windows.Forms.PictureBox picZPAIndicator;
    }
}