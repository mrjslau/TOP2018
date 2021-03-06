﻿namespace ShopLensForms
{
    partial class ShoppingListForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShoppingListForm));
            this.MyList_listBox = new System.Windows.Forms.ListBox();
            this.ItemsToBuy_label = new System.Windows.Forms.Label();
            this.Add_btn = new System.Windows.Forms.Button();
            this.ItemToAdd_textBox = new System.Windows.Forms.TextBox();
            this.Close_btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // MyList_listBox
            // 
            resources.ApplyResources(this.MyList_listBox, "MyList_listBox");
            this.MyList_listBox.FormattingEnabled = true;
            this.MyList_listBox.Name = "MyList_listBox";
            // 
            // ItemsToBuy_label
            // 
            resources.ApplyResources(this.ItemsToBuy_label, "ItemsToBuy_label");
            this.ItemsToBuy_label.Name = "ItemsToBuy_label";
            // 
            // Add_btn
            // 
            resources.ApplyResources(this.Add_btn, "Add_btn");
            this.Add_btn.Name = "Add_btn";
            this.Add_btn.UseVisualStyleBackColor = true;
            this.Add_btn.Click += new System.EventHandler(this.Add_btn_Click);
            // 
            // ItemToAdd_textBox
            // 
            resources.ApplyResources(this.ItemToAdd_textBox, "ItemToAdd_textBox");
            this.ItemToAdd_textBox.Name = "ItemToAdd_textBox";
            // 
            // Close_btn
            // 
            resources.ApplyResources(this.Close_btn, "Close_btn");
            this.Close_btn.Name = "Close_btn";
            this.Close_btn.UseVisualStyleBackColor = true;
            this.Close_btn.Click += new System.EventHandler(this.Close_btn_Click);
            // 
            // ShoppingListForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Controls.Add(this.Close_btn);
            this.Controls.Add(this.ItemToAdd_textBox);
            this.Controls.Add(this.Add_btn);
            this.Controls.Add(this.ItemsToBuy_label);
            this.Controls.Add(this.MyList_listBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ShoppingListForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label ItemsToBuy_label;
        private System.Windows.Forms.Button Add_btn;
        private System.Windows.Forms.Button Close_btn;
        public System.Windows.Forms.TextBox ItemToAdd_textBox;
        public System.Windows.Forms.ListBox MyList_listBox;
    }
}