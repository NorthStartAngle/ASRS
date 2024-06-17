namespace ASRS.Component
{
    partial class PTLOperator
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
            this.lblPTLSwitch_Status = new System.Windows.Forms.Label();
            this.lblPTL_Switch = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblPTLSwitch_Status
            // 
            this.lblPTLSwitch_Status.Location = new System.Drawing.Point(234, 231);
            this.lblPTLSwitch_Status.Name = "lblPTLSwitch_Status";
            this.lblPTLSwitch_Status.Size = new System.Drawing.Size(446, 446);
            this.lblPTLSwitch_Status.TabIndex = 2;
            // 
            // lblPTL_Switch
            // 
            this.lblPTL_Switch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPTL_Switch.Font = new System.Drawing.Font("Microsoft YaHei", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPTL_Switch.Location = new System.Drawing.Point(102, 37);
            this.lblPTL_Switch.Name = "lblPTL_Switch";
            this.lblPTL_Switch.Size = new System.Drawing.Size(747, 71);
            this.lblPTL_Switch.TabIndex = 4;
            this.lblPTL_Switch.Text = "OUTBOUND PTL ID";
            this.lblPTL_Switch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PTLOperator
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.lblPTL_Switch);
            this.Controls.Add(this.lblPTLSwitch_Status);
            this.DoubleBuffered = true;
            this.Name = "PTLOperator";
            this.Size = new System.Drawing.Size(931, 700);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblPTLSwitch_Status;
        private System.Windows.Forms.Label lblPTL_Switch;
    }
}
