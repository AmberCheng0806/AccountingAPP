namespace 記帳APP.Forms
{
    partial class 圖表分析
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.dateTimePickerEnd = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.searchBtn = new System.Windows.Forms.Button();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.userControl11 = new 記帳APP.Components.Navbar();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Location = new System.Drawing.Point(67, 83);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(501, 610);
            this.flowLayoutPanel1.TabIndex = 15;
            // 
            // dateTimePickerEnd
            // 
            this.dateTimePickerEnd.Location = new System.Drawing.Point(369, 15);
            this.dateTimePickerEnd.Name = "dateTimePickerEnd";
            this.dateTimePickerEnd.Size = new System.Drawing.Size(200, 29);
            this.dateTimePickerEnd.TabIndex = 13;
            // 
            // dateTimePicker
            // 
            this.dateTimePicker.Location = new System.Drawing.Point(74, 15);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.Size = new System.Drawing.Size(200, 29);
            this.dateTimePicker.TabIndex = 14;
            // 
            // searchBtn
            // 
            this.searchBtn.Location = new System.Drawing.Point(1006, 15);
            this.searchBtn.Name = "searchBtn";
            this.searchBtn.Size = new System.Drawing.Size(184, 47);
            this.searchBtn.TabIndex = 11;
            this.searchBtn.Text = "查詢";
            this.searchBtn.UseVisualStyleBackColor = true;
            this.searchBtn.Click += new System.EventHandler(this.SearchBtn_Click);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Location = new System.Drawing.Point(628, 83);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(562, 609);
            this.flowLayoutPanel2.TabIndex = 16;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(605, 15);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(186, 26);
            this.comboBox1.TabIndex = 17;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Location = new System.Drawing.Point(809, 15);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(182, 30);
            this.flowLayoutPanel3.TabIndex = 18;
            // 
            // userControl11
            // 
            this.userControl11.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControl11.Location = new System.Drawing.Point(0, 706);
            this.userControl11.Name = "userControl11";
            this.userControl11.Size = new System.Drawing.Size(1221, 102);
            this.userControl11.TabIndex = 1;
            // 
            // 圖表分析
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1221, 808);
            this.Controls.Add(this.flowLayoutPanel3);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.flowLayoutPanel2);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.dateTimePickerEnd);
            this.Controls.Add(this.dateTimePicker);
            this.Controls.Add(this.searchBtn);
            this.Controls.Add(this.userControl11);
            this.Name = "圖表分析";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "圖表分析";
            this.Load += new System.EventHandler(this.圖表分析_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Components.Navbar userControl11;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.DateTimePicker dateTimePickerEnd;
        private System.Windows.Forms.DateTimePicker dateTimePicker;
        private System.Windows.Forms.Button searchBtn;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
    }
}