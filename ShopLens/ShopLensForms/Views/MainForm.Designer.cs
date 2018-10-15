using System;
using System.Windows.Forms;

namespace ShopLensForms
{
    partial class ShopLens
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShopLens));
            this.live_video = new System.Windows.Forms.PictureBox();
            this.WhatIsThis_btn = new System.Windows.Forms.Button();
            this.webcam_combobox = new System.Windows.Forms.ComboBox();
            this.start_btn = new System.Windows.Forms.Button();
            this.exit_btn = new System.Windows.Forms.Button();
            this.menu_panel = new System.Windows.Forms.Panel();
            this.MyCart_btn = new System.Windows.Forms.Button();
            this.MyList_btn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.live_video)).BeginInit();
            this.menu_panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // live_video
            // 
            resources.ApplyResources(this.live_video, "live_video");
            this.live_video.Name = "live_video";
            this.live_video.TabStop = false;
            // 
            // WhatIsThis_btn
            // 
            resources.ApplyResources(this.WhatIsThis_btn, "WhatIsThis_btn");
            this.WhatIsThis_btn.Name = "WhatIsThis_btn";
            this.WhatIsThis_btn.UseVisualStyleBackColor = true;
            this.WhatIsThis_btn.Click += new System.EventHandler(this.WhatIsThis_btn_Click);
            // 
            // webcam_combobox
            // 
            resources.ApplyResources(this.webcam_combobox, "webcam_combobox");
            this.webcam_combobox.FormattingEnabled = true;
            this.webcam_combobox.Name = "webcam_combobox";
            // 
            // start_btn
            // 
            resources.ApplyResources(this.start_btn, "start_btn");
            this.start_btn.Name = "start_btn";
            this.start_btn.UseVisualStyleBackColor = true;
            this.start_btn.Click += new System.EventHandler(this.Start_btn_Click);
            // 
            // exit_btn
            // 
            resources.ApplyResources(this.exit_btn, "exit_btn");
            this.exit_btn.Name = "exit_btn";
            this.exit_btn.UseVisualStyleBackColor = true;
            this.exit_btn.Click += new System.EventHandler(this.Exit_btn_Click);
            // 
            // menu_panel
            // 
            resources.ApplyResources(this.menu_panel, "menu_panel");
            this.menu_panel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.menu_panel.Controls.Add(this.MyCart_btn);
            this.menu_panel.Controls.Add(this.MyList_btn);
            this.menu_panel.Controls.Add(this.start_btn);
            this.menu_panel.Controls.Add(this.webcam_combobox);
            this.menu_panel.Controls.Add(this.exit_btn);
            this.menu_panel.Controls.Add(this.WhatIsThis_btn);
            this.menu_panel.Name = "menu_panel";
            // 
            // MyCart_btn
            // 
            resources.ApplyResources(this.MyCart_btn, "MyCart_btn");
            this.MyCart_btn.Name = "MyCart_btn";
            this.MyCart_btn.UseVisualStyleBackColor = true;
            this.MyCart_btn.Click += new System.EventHandler(this.MyCart_btn_Click);
            // 
            // MyList_btn
            // 
            resources.ApplyResources(this.MyList_btn, "MyList_btn");
            this.MyList_btn.Name = "MyList_btn";
            this.MyList_btn.UseVisualStyleBackColor = true;
            this.MyList_btn.Click += new System.EventHandler(this.MyList_btn_Click);
            // 
            // ShopLens
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.menu_panel);
            this.Controls.Add(this.live_video);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ShopLens";
            ((System.ComponentModel.ISupportInitialize)(this.live_video)).EndInit();
            this.menu_panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void MainWindow_Paint(object sender, PaintEventArgs e)
        {
            
        }

        #endregion
        private System.Windows.Forms.Button WhatIsThis_btn;
        private System.Windows.Forms.ComboBox webcam_combobox;
        private System.Windows.Forms.Button start_btn;
        private PictureBox live_video;
        private Button exit_btn;
        private Panel menu_panel;
        private Button MyList_btn;
        private Button MyCart_btn;
    }
}

