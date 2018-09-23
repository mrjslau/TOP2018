using System;
using System.Windows.Forms;

namespace WindowsFormsApp
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
            this.WELCOME_TO = new System.Windows.Forms.Label();
            this.Logo = new System.Windows.Forms.PictureBox();
            this.PRESS_ENTER_TO_START = new System.Windows.Forms.Label();
            this.Intro = new System.Windows.Forms.Panel();
            this.MainWindow = new System.Windows.Forms.Panel();
            this.pictures_panel = new System.Windows.Forms.TableLayoutPanel();
            this.live_video = new System.Windows.Forms.PictureBox();
            this.capture_picture = new System.Windows.Forms.PictureBox();
            this.exit_btn = new System.Windows.Forms.Button();
            this.capture_btn = new System.Windows.Forms.Button();
            this.pause_btn = new System.Windows.Forms.Button();
            this.webcam_combobox = new System.Windows.Forms.ComboBox();
            this.reset_btn = new System.Windows.Forms.Button();
            this.start_btn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Logo)).BeginInit();
            this.Intro.SuspendLayout();
            this.MainWindow.SuspendLayout();
            this.pictures_panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.live_video)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.capture_picture)).BeginInit();
            this.SuspendLayout();
            // 
            // WELCOME_TO
            // 
            this.WELCOME_TO.AutoSize = true;
            this.WELCOME_TO.Font = new System.Drawing.Font("Arial", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.WELCOME_TO.Location = new System.Drawing.Point(197, 9);
            this.WELCOME_TO.Name = "WELCOME_TO";
            this.WELCOME_TO.Size = new System.Drawing.Size(354, 56);
            this.WELCOME_TO.TabIndex = 0;
            this.WELCOME_TO.Text = "WELCOME TO";
            // 
            // Logo
            // 
            this.Logo.Image = global::WindowsFormsApp.Properties.Resources.shopLensLogo;
            this.Logo.Location = new System.Drawing.Point(203, 63);
            this.Logo.Name = "Logo";
            this.Logo.Size = new System.Drawing.Size(344, 284);
            this.Logo.TabIndex = 1;
            this.Logo.TabStop = false;
            // 
            // PRESS_ENTER_TO_START
            // 
            this.PRESS_ENTER_TO_START.AutoSize = true;
            this.PRESS_ENTER_TO_START.Font = new System.Drawing.Font("Arial", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.PRESS_ENTER_TO_START.Location = new System.Drawing.Point(158, 355);
            this.PRESS_ENTER_TO_START.Name = "PRESS_ENTER_TO_START";
            this.PRESS_ENTER_TO_START.Size = new System.Drawing.Size(438, 41);
            this.PRESS_ENTER_TO_START.TabIndex = 2;
            this.PRESS_ENTER_TO_START.Text = "PRESS ENTER TO START";
            this.PRESS_ENTER_TO_START.Click += new System.EventHandler(this.PRESS_ENTER_TO_START_Click);
            // 
            // Intro
            // 
            this.Intro.Controls.Add(this.MainWindow);
            this.Intro.Controls.Add(this.WELCOME_TO);
            this.Intro.Controls.Add(this.PRESS_ENTER_TO_START);
            this.Intro.Controls.Add(this.Logo);
            this.Intro.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Intro.Location = new System.Drawing.Point(0, 0);
            this.Intro.Name = "Intro";
            this.Intro.Size = new System.Drawing.Size(766, 418);
            this.Intro.TabIndex = 3;
            // 
            // MainWindow
            // 
            this.MainWindow.Controls.Add(this.pictures_panel);
            this.MainWindow.Controls.Add(this.exit_btn);
            this.MainWindow.Controls.Add(this.capture_btn);
            this.MainWindow.Controls.Add(this.pause_btn);
            this.MainWindow.Controls.Add(this.webcam_combobox);
            this.MainWindow.Controls.Add(this.reset_btn);
            this.MainWindow.Controls.Add(this.start_btn);
            this.MainWindow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainWindow.Location = new System.Drawing.Point(0, 0);
            this.MainWindow.Name = "MainWindow";
            this.MainWindow.Size = new System.Drawing.Size(766, 418);
            this.MainWindow.TabIndex = 3;
            this.MainWindow.Visible = false;
            this.MainWindow.Paint += new System.Windows.Forms.PaintEventHandler(this.MainWindow_Paint);
            // 
            // pictures_panel
            // 
            this.pictures_panel.ColumnCount = 2;
            this.pictures_panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pictures_panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pictures_panel.Controls.Add(this.live_video, 0, 0);
            this.pictures_panel.Controls.Add(this.capture_picture, 1, 0);
            this.pictures_panel.Location = new System.Drawing.Point(13, 63);
            this.pictures_panel.Name = "pictures_panel";
            this.pictures_panel.RowCount = 1;
            this.pictures_panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.pictures_panel.Size = new System.Drawing.Size(741, 343);
            this.pictures_panel.TabIndex = 6;
            // 
            // live_video
            // 
            this.live_video.Dock = System.Windows.Forms.DockStyle.Fill;
            this.live_video.Location = new System.Drawing.Point(3, 3);
            this.live_video.Name = "live_video";
            this.live_video.Size = new System.Drawing.Size(364, 337);
            this.live_video.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.live_video.TabIndex = 2;
            this.live_video.TabStop = false;
            this.live_video.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // capture_picture
            // 
            this.capture_picture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.capture_picture.Location = new System.Drawing.Point(373, 3);
            this.capture_picture.Name = "capture_picture";
            this.capture_picture.Size = new System.Drawing.Size(365, 337);
            this.capture_picture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.capture_picture.TabIndex = 1;
            this.capture_picture.TabStop = false;
            // 
            // exit_btn
            // 
            this.exit_btn.Location = new System.Drawing.Point(676, 12);
            this.exit_btn.Name = "exit_btn";
            this.exit_btn.Size = new System.Drawing.Size(75, 23);
            this.exit_btn.TabIndex = 5;
            this.exit_btn.Text = "EXIT";
            this.exit_btn.UseVisualStyleBackColor = true;
            this.exit_btn.Click += new System.EventHandler(this.EXIT_Click);
            // 
            // capture_btn
            // 
            this.capture_btn.Location = new System.Drawing.Point(447, 12);
            this.capture_btn.Name = "capture_btn";
            this.capture_btn.Size = new System.Drawing.Size(75, 23);
            this.capture_btn.TabIndex = 4;
            this.capture_btn.Text = "CAPTURE";
            this.capture_btn.UseVisualStyleBackColor = true;
            this.capture_btn.Click += new System.EventHandler(this.CAPTURE_Click);
            // 
            // pause_btn
            // 
            this.pause_btn.Location = new System.Drawing.Point(366, 12);
            this.pause_btn.Name = "pause_btn";
            this.pause_btn.Size = new System.Drawing.Size(75, 23);
            this.pause_btn.TabIndex = 3;
            this.pause_btn.Text = "PAUSE";
            this.pause_btn.UseVisualStyleBackColor = true;
            this.pause_btn.Click += new System.EventHandler(this.PAUSE_Click);
            // 
            // webcam_combobox
            // 
            this.webcam_combobox.FormattingEnabled = true;
            this.webcam_combobox.Location = new System.Drawing.Point(96, 14);
            this.webcam_combobox.Name = "webcam_combobox";
            this.webcam_combobox.Size = new System.Drawing.Size(157, 21);
            this.webcam_combobox.TabIndex = 2;
            // 
            // reset_btn
            // 
            this.reset_btn.Location = new System.Drawing.Point(285, 12);
            this.reset_btn.Name = "reset_btn";
            this.reset_btn.Size = new System.Drawing.Size(75, 23);
            this.reset_btn.TabIndex = 1;
            this.reset_btn.Text = "RESET";
            this.reset_btn.UseVisualStyleBackColor = true;
            this.reset_btn.Click += new System.EventHandler(this.RESET_Click);
            // 
            // start_btn
            // 
            this.start_btn.Location = new System.Drawing.Point(13, 13);
            this.start_btn.Name = "start_btn";
            this.start_btn.Size = new System.Drawing.Size(75, 23);
            this.start_btn.TabIndex = 0;
            this.start_btn.Text = "START";
            this.start_btn.UseVisualStyleBackColor = true;
            this.start_btn.Click += new System.EventHandler(this.START_Click);
            // 
            // ShopLens
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(766, 418);
            this.Controls.Add(this.Intro);
            this.Name = "ShopLens";
            this.Text = "ShopLens";
            this.Load += new System.EventHandler(this.ShopLens_Load_1);
            ((System.ComponentModel.ISupportInitialize)(this.Logo)).EndInit();
            this.Intro.ResumeLayout(false);
            this.Intro.PerformLayout();
            this.MainWindow.ResumeLayout(false);
            this.pictures_panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.live_video)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.capture_picture)).EndInit();
            this.ResumeLayout(false);

        }

        private void MainWindow_Paint(object sender, PaintEventArgs e)
        {
            
        }

        #endregion

        private System.Windows.Forms.Label WELCOME_TO;
        private System.Windows.Forms.PictureBox Logo;
        private System.Windows.Forms.Label PRESS_ENTER_TO_START;
        private System.Windows.Forms.Panel Intro;
        private System.Windows.Forms.Panel MainWindow;
        private System.Windows.Forms.TableLayoutPanel pictures_panel;
        private System.Windows.Forms.PictureBox capture_picture;
        private System.Windows.Forms.Button exit_btn;
        private System.Windows.Forms.Button capture_btn;
        private System.Windows.Forms.Button pause_btn;
        private System.Windows.Forms.ComboBox webcam_combobox;
        private System.Windows.Forms.Button reset_btn;
        private System.Windows.Forms.Button start_btn;
        private PictureBox live_video;
    }
}

