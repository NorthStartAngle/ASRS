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
            this.lbl_Time = new System.Windows.Forms.Label();
            this.picZPA = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picZPA)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_Time
            // 
            this.lbl_Time.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_Time.Font = new System.Drawing.Font("Bahnschrift SemiBold Condensed", 65F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Time.Location = new System.Drawing.Point(845, 566);
            this.lbl_Time.Name = "lbl_Time";
            this.lbl_Time.Size = new System.Drawing.Size(875, 120);
            this.lbl_Time.TabIndex = 2;
            this.lbl_Time.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // picZPA
            // 
            this.picZPA.Image = global::ASRS.Properties.Resources.Arrows_bar;
            this.picZPA.Location = new System.Drawing.Point(43, 43);
            this.picZPA.Name = "picZPA";
            this.picZPA.Size = new System.Drawing.Size(140, 20);
            this.picZPA.TabIndex = 3;
            this.picZPA.TabStop = false;
            // 
            // InboundOperator
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImage = global::ASRS.Properties.Resources.back_1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Controls.Add(this.picZPA);
            this.Controls.Add(this.lbl_Time);
            this.DoubleBuffered = true;
            this.Name = "InboundOperator";
            this.Padding = new System.Windows.Forms.Padding(20);
            this.Size = new System.Drawing.Size(1720, 706);
            ((System.ComponentModel.ISupportInitialize)(this.picZPA)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lbl_Time;
        private System.Windows.Forms.PictureBox picZPA;
    }
}
