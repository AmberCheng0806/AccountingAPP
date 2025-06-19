namespace 記帳APP.Forms
{
    partial class 帳戶分析
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
            this.userControl11 = new 記帳APP.Components.Navbar();
            this.SuspendLayout();
            // 
            // userControl11
            // 
            this.userControl11.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControl11.Location = new System.Drawing.Point(0, 742);
            this.userControl11.Name = "userControl11";
            this.userControl11.Size = new System.Drawing.Size(1178, 102);
            this.userControl11.TabIndex = 1;
            // 
            // 帳戶分析
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1178, 844);
            this.Controls.Add(this.userControl11);
            this.Name = "帳戶分析";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "帳戶";
            this.Load += new System.EventHandler(this.帳戶分析_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Components.Navbar userControl11;
    }
}