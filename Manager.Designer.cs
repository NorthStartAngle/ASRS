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
            ((System.ComponentModel.ISupportInitialize)(this.managerStyle)).BeginInit();
            this.SuspendLayout();
            // 
            // managerStyle
            // 
            this.managerStyle.Owner = this;
            this.managerStyle.Style = MetroFramework.MetroColorStyle.Silver;
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
            this.bodyLayout.RowCount = 3;
            this.bodyLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.bodyLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.bodyLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.bodyLayout.Size = new System.Drawing.Size(593, 382);
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
            this.ClientSize = new System.Drawing.Size(593, 462);
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
            this.ResumeLayout(false);

        }

        #endregion

        public MetroFramework.Components.MetroStyleManager managerStyle;
        private System.Windows.Forms.TableLayoutPanel bodyLayout;
    }
}