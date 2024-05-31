namespace ASRS.Component
{
    partial class Splash
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
            this.lblWaiting = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblWaiting
            // 
            this.lblWaiting.AutoEllipsis = true;
            this.lblWaiting.Image = global::ASRS.Properties.Resources.waiting;
            this.lblWaiting.Location = new System.Drawing.Point(233, 80);
            this.lblWaiting.Name = "lblWaiting";
            this.lblWaiting.Size = new System.Drawing.Size(460, 460);
            this.lblWaiting.TabIndex = 0;
            this.lblWaiting.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // Splash
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(10)))), ((int)(((byte)(10)))));
            this.Controls.Add(this.lblWaiting);
            this.Name = "Splash";
            this.Size = new System.Drawing.Size(947, 643);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblWaiting;
    }
}