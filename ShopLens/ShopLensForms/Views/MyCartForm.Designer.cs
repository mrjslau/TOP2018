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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MyCartForm));
            this.ItemsInMyCart_label = new System.Windows.Forms.Label();
            this.MyCart_listBox = new System.Windows.Forms.ListBox();
            this.Close_btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ItemsInMyCart_label
            // 
            resources.ApplyResources(this.ItemsInMyCart_label, "ItemsInMyCart_label");
            this.ItemsInMyCart_label.Name = "ItemsInMyCart_label";
            // 
            // MyCart_listBox
            // 
            resources.ApplyResources(this.MyCart_listBox, "MyCart_listBox");
            this.MyCart_listBox.FormattingEnabled = true;
            this.MyCart_listBox.Items.AddRange(new object[] {
            resources.GetString("MyCart_listBox.Items")});
            this.MyCart_listBox.Name = "MyCart_listBox";
            // 
            // Close_btn
            // 
            resources.ApplyResources(this.Close_btn, "Close_btn");
            this.Close_btn.Name = "Close_btn";
            this.Close_btn.UseVisualStyleBackColor = true;
            this.Close_btn.Click += new System.EventHandler(this.Close_btn_Click);
            // 
            // MyCartForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Controls.Add(this.Close_btn);
            this.Controls.Add(this.MyCart_listBox);
            this.Controls.Add(this.ItemsInMyCart_label);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MyCartForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ItemsInMyCart_label;
        private System.Windows.Forms.ListBox MyCart_listBox;
        private System.Windows.Forms.Button Close_btn;
    }
}