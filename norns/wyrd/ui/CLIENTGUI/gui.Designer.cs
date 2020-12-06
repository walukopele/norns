namespace CLIENTGUI
{
    partial class gui
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
            this.client_control1 = new skuld.client_control();
            this.SuspendLayout();
            // 
            // client_control1
            // 
            this.client_control1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.client_control1.Location = new System.Drawing.Point(0, 0);
            this.client_control1.Name = "client_control1";
            this.client_control1.Size = new System.Drawing.Size(872, 410);
            this.client_control1.TabIndex = 0;
            this.client_control1.Load += new System.EventHandler(this.client_control1_Load);
            // 
            // gui
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(872, 410);
            this.Controls.Add(this.client_control1);
            this.Name = "gui";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private skuld.client_control client_control1;
    }
}

