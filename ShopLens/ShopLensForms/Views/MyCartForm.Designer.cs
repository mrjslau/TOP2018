namespace ShopLensForms
{
    partial class MyCartForm
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
            this.ItemsInMyCart_label = new System.Windows.Forms.Label();
            this.MyCart_listBox = new System.Windows.Forms.ListBox();
            this.Close_btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ItemsInMyCart_label
            // 
            this.ItemsInMyCart_label.AutoSize = true;
            this.ItemsInMyCart_label.Location = new System.Drawing.Point(13, 13);
            this.ItemsInMyCart_label.Name = "ItemsInMyCart_label";
            this.ItemsInMyCart_label.Size = new System.Drawing.Size(105, 13);
            this.ItemsInMyCart_label.TabIndex = 0;
            this.ItemsInMyCart_label.Text = "ITEMS IN MY CART";
            // 
            // MyCart_listBox
            // 
            this.MyCart_listBox.FormattingEnabled = true;
            this.MyCart_listBox.Items.AddRange(new object[] {
            "Tomato"});
            this.MyCart_listBox.Location = new System.Drawing.Point(16, 30);
            this.MyCart_listBox.Name = "MyCart_listBox";
            this.MyCart_listBox.Size = new System.Drawing.Size(175, 225);
            this.MyCart_listBox.TabIndex = 1;
            // 
            // Close_btn
            // 
            this.Close_btn.Location = new System.Drawing.Point(216, 200);
            this.Close_btn.Name = "Close_btn";
            this.Close_btn.Size = new System.Drawing.Size(141, 55);
            this.Close_btn.TabIndex = 2;
            this.Close_btn.Text = "Close";
            this.Close_btn.UseVisualStyleBackColor = true;
            this.Close_btn.Click += new System.EventHandler(this.Close_btn_Click);
            // 
            // MyCartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(381, 274);
            this.Controls.Add(this.Close_btn);
            this.Controls.Add(this.MyCart_listBox);
            this.Controls.Add(this.ItemsInMyCart_label);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MyCartForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MyCartForm";
            this.Load += new System.EventHandler(this.MyCartForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ItemsInMyCart_label;
        private System.Windows.Forms.ListBox MyCart_listBox;
        private System.Windows.Forms.Button Close_btn;
    }
}