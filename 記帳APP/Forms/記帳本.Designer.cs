namespace 記帳APP.Forms
{
    partial class 記帳本
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
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
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
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(938, 35);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(184, 47);
            this.button1.TabIndex = 3;
            this.button1.Text = "查詢";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridView
            // 
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Location = new System.Drawing.Point(56, 122);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RowHeadersWidth = 62;
            this.dataGridView.RowTemplate.Height = 31;
            this.dataGridView.Size = new System.Drawing.Size(1066, 591);
            this.dataGridView.TabIndex = 4;
            this.dataGridView.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellContentDoubleClick);
            // 
            // 記帳本
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1178, 844);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.userControl11);
            this.Name = "記帳本";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "記帳本";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Components.Navbar userControl11;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dataGridView;
    }
}