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
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lbl_zpa = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lbl_gecko = new System.Windows.Forms.ToolStripStatusLabel();
            this.managerStyle = new MetroFramework.Components.MetroStyleManager(this.components);
            this.bodyLayout = new System.Windows.Forms.TableLayoutPanel();
            this.statusBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.managerStyle)).BeginInit();
            this.SuspendLayout();
            // 
            // statusBar
            // 
            this.statusBar.AutoSize = false;
            this.statusBar.BackColor = System.Drawing.Color.White;
            this.statusBar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("statusBar.BackgroundImage")));
            this.statusBar.Font = new System.Drawing.Font("Segoe UI Emoji", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusBar.GripMargin = new System.Windows.Forms.Padding(1, 5, 1, 5);
            this.statusBar.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.lbl_zpa,
            this.toolStripStatusLabel2,
            this.lbl_gecko});
            this.statusBar.Location = new System.Drawing.Point(0, 602);
            this.statusBar.MaximumSize = new System.Drawing.Size(0, 40);
            this.statusBar.MinimumSize = new System.Drawing.Size(0, 40);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(779, 40);
            this.statusBar.Stretch = false;
            this.statusBar.TabIndex = 2;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.AutoSize = false;
            this.toolStripStatusLabel1.BackColor = System.Drawing.Color.Transparent;
            this.toolStripStatusLabel1.Font = new System.Drawing.Font("Tw Cen MT", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.Purple;
            this.toolStripStatusLabel1.Margin = new System.Windows.Forms.Padding(10, 5, 5, 2);
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(50, 33);
            this.toolStripStatusLabel1.Text = "ZPA";
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolStripStatusLabel1.Visible = false;
            // 
            // lbl_zpa
            // 
            this.lbl_zpa.AutoSize = false;
            this.lbl_zpa.Image = global::ASRS.Properties.Resources.r_0_0;
            this.lbl_zpa.ImageTransparentColor = System.Drawing.Color.White;
            this.lbl_zpa.Margin = new System.Windows.Forms.Padding(8);
            this.lbl_zpa.Name = "lbl_zpa";
            this.lbl_zpa.Size = new System.Drawing.Size(24, 24);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.AutoSize = false;
            this.toolStripStatusLabel2.BackColor = System.Drawing.Color.Transparent;
            this.toolStripStatusLabel2.Font = new System.Drawing.Font("Trebuchet MS", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.toolStripStatusLabel2.Margin = new System.Windows.Forms.Padding(0, 5, 5, 2);
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(80, 33);
            this.toolStripStatusLabel2.Text = "Gecko";
            this.toolStripStatusLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolStripStatusLabel2.Visible = false;
            // 
            // lbl_gecko
            // 
            this.lbl_gecko.AutoSize = false;
            this.lbl_gecko.Image = global::ASRS.Properties.Resources.r_1_0;
            this.lbl_gecko.Margin = new System.Windows.Forms.Padding(8);
            this.lbl_gecko.Name = "lbl_gecko";
            this.lbl_gecko.Size = new System.Drawing.Size(50, 24);
            // 
            // managerStyle
            // 
            this.managerStyle.Owner = this;
            this.managerStyle.Style = MetroFramework.MetroColorStyle.Lime;
            this.managerStyle.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // bodyLayout
            // 
            this.bodyLayout.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bodyLayout.BackColor = System.Drawing.Color.White;
            this.bodyLayout.ColumnCount = 3;
            this.bodyLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.bodyLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 400F));
            this.bodyLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.bodyLayout.Location = new System.Drawing.Point(0, 80);
            this.bodyLayout.Margin = new System.Windows.Forms.Padding(0, 10, 0, 40);
            this.bodyLayout.Name = "bodyLayout";
            this.bodyLayout.Padding = new System.Windows.Forms.Padding(0, 0, 0, 45);
            this.bodyLayout.RowCount = 3;
            this.bodyLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.bodyLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.bodyLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.bodyLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.bodyLayout.Size = new System.Drawing.Size(779, 562);
            this.bodyLayout.TabIndex = 1;
            // 
            // Manager
            // 
            this.ApplyImageInvert = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.BackLocation = MetroFramework.Forms.BackLocation.TopRight;
            this.BackMaxSize = 20;
            this.ClientSize = new System.Drawing.Size(779, 642);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.bodyLayout);
            this.DoubleBuffered = false;
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
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.managerStyle)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public MetroFramework.Components.MetroStyleManager managerStyle;
        private System.Windows.Forms.TableLayoutPanel bodyLayout;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel lbl_zpa;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel lbl_gecko;
        private System.Windows.Forms.StatusStrip statusBar;
    }
}