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
            this.live_video.Location = new System.Drawing.Point(234, 26);
            this.live_video.Name = "live_video";
            this.live_video.Size = new System.Drawing.Size(545, 395);
            this.live_video.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.live_video.TabIndex = 2;
            this.live_video.TabStop = false;
            // 
            // WhatIsThis_btn
            // 
            this.WhatIsThis_btn.Location = new System.Drawing.Point(14, 75);
            this.WhatIsThis_btn.Name = "WhatIsThis_btn";
            this.WhatIsThis_btn.Size = new System.Drawing.Size(190, 30);
            this.WhatIsThis_btn.TabIndex = 4;
            this.WhatIsThis_btn.Text = "WHAT IS THIS?";
            this.WhatIsThis_btn.UseVisualStyleBackColor = true;
            this.WhatIsThis_btn.Click += new System.EventHandler(this.WhatIsThis_btn_Click);
            // 
            // webcam_combobox
            // 
            this.webcam_combobox.FormattingEnabled = true;
            this.webcam_combobox.Location = new System.Drawing.Point(14, 48);
            this.webcam_combobox.Name = "webcam_combobox";
            this.webcam_combobox.Size = new System.Drawing.Size(190, 21);
            this.webcam_combobox.TabIndex = 2;
            // 
            // start_btn
            // 
            this.start_btn.Location = new System.Drawing.Point(14, 12);
            this.start_btn.Name = "start_btn";
            this.start_btn.Size = new System.Drawing.Size(190, 30);
            this.start_btn.TabIndex = 0;
            this.start_btn.Text = "START";
            this.start_btn.UseVisualStyleBackColor = true;
            this.start_btn.Click += new System.EventHandler(this.Start_btn_Click);
            // 
            // exit_btn
            // 
            this.exit_btn.Location = new System.Drawing.Point(14, 391);
            this.exit_btn.Name = "exit_btn";
            this.exit_btn.Size = new System.Drawing.Size(190, 30);
            this.exit_btn.TabIndex = 7;
            this.exit_btn.Text = "EXIT";
            this.exit_btn.UseVisualStyleBackColor = true;
            this.exit_btn.Click += new System.EventHandler(this.Exit_btn_Click);
            // 
            // menu_panel
            // 
            this.menu_panel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.menu_panel.Controls.Add(this.MyCart_btn);
            this.menu_panel.Controls.Add(this.MyList_btn);
            this.menu_panel.Controls.Add(this.start_btn);
            this.menu_panel.Controls.Add(this.webcam_combobox);
            this.menu_panel.Controls.Add(this.exit_btn);
            this.menu_panel.Controls.Add(this.WhatIsThis_btn);
            this.menu_panel.Location = new System.Drawing.Point(-2, 0);
            this.menu_panel.Name = "menu_panel";
            this.menu_panel.Size = new System.Drawing.Size(216, 451);
            this.menu_panel.TabIndex = 8;
            // 
            // MyCart_btn
            // 
            this.MyCart_btn.Location = new System.Drawing.Point(14, 148);
            this.MyCart_btn.Name = "MyCart_btn";
            this.MyCart_btn.Size = new System.Drawing.Size(190, 31);
            this.MyCart_btn.TabIndex = 9;
            this.MyCart_btn.Text = "MY CART";
            this.MyCart_btn.UseVisualStyleBackColor = true;
            this.MyCart_btn.Click += new System.EventHandler(this.MyCart_btn_Click);
            // 
            // MyList_btn
            // 
            this.MyList_btn.Location = new System.Drawing.Point(14, 112);
            this.MyList_btn.Name = "MyList_btn";
            this.MyList_btn.Size = new System.Drawing.Size(190, 30);
            this.MyList_btn.TabIndex = 8;
            this.MyList_btn.Text = "MY LIST";
            this.MyList_btn.UseVisualStyleBackColor = true;
            this.MyList_btn.Click += new System.EventHandler(this.MyList_btn_Click);
            // 
            // ShopLens
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.menu_panel);
            this.Controls.Add(this.live_video);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ShopLens";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ShopLens";
            this.Shown += new System.EventHandler(this.ShopLens_Shown);
            this.Load += new System.EventHandler(this.ShopLens_Load_1);
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

