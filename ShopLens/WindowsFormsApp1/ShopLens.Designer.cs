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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.EXIT = new System.Windows.Forms.Button();
            this.CAPTURE = new System.Windows.Forms.Button();
            this.PAUSE = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.RESET = new System.Windows.Forms.Button();
            this.START = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.Logo)).BeginInit();
            this.Intro.SuspendLayout();
            this.MainWindow.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
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
            this.MainWindow.Controls.Add(this.tableLayoutPanel1);
            this.MainWindow.Controls.Add(this.EXIT);
            this.MainWindow.Controls.Add(this.CAPTURE);
            this.MainWindow.Controls.Add(this.PAUSE);
            this.MainWindow.Controls.Add(this.comboBox1);
            this.MainWindow.Controls.Add(this.RESET);
            this.MainWindow.Controls.Add(this.START);
            this.MainWindow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainWindow.Location = new System.Drawing.Point(0, 0);
            this.MainWindow.Name = "MainWindow";
            this.MainWindow.Size = new System.Drawing.Size(766, 418);
            this.MainWindow.TabIndex = 3;
            this.MainWindow.Visible = false;
            this.MainWindow.Paint += new System.Windows.Forms.PaintEventHandler(this.MainWindow_Paint);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox2, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(13, 63);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(741, 343);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox2.Location = new System.Drawing.Point(373, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(365, 337);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // EXIT
            // 
            this.EXIT.Location = new System.Drawing.Point(676, 12);
            this.EXIT.Name = "EXIT";
            this.EXIT.Size = new System.Drawing.Size(75, 23);
            this.EXIT.TabIndex = 5;
            this.EXIT.Text = "EXIT";
            this.EXIT.UseVisualStyleBackColor = true;
            this.EXIT.Click += new System.EventHandler(this.EXIT_Click);
            // 
            // CAPTURE
            // 
            this.CAPTURE.Location = new System.Drawing.Point(447, 14);
            this.CAPTURE.Name = "CAPTURE";
            this.CAPTURE.Size = new System.Drawing.Size(75, 23);
            this.CAPTURE.TabIndex = 4;
            this.CAPTURE.Text = "CAPTURE";
            this.CAPTURE.UseVisualStyleBackColor = true;
            this.CAPTURE.Click += new System.EventHandler(this.CAPTURE_Click);
            // 
            // PAUSE
            // 
            this.PAUSE.Location = new System.Drawing.Point(366, 12);
            this.PAUSE.Name = "PAUSE";
            this.PAUSE.Size = new System.Drawing.Size(75, 23);
            this.PAUSE.TabIndex = 3;
            this.PAUSE.Text = "PAUSE";
            this.PAUSE.UseVisualStyleBackColor = true;
            this.PAUSE.Click += new System.EventHandler(this.PAUSE_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(96, 14);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(157, 21);
            this.comboBox1.TabIndex = 2;
            // 
            // RESET
            // 
            this.RESET.Location = new System.Drawing.Point(285, 12);
            this.RESET.Name = "RESET";
            this.RESET.Size = new System.Drawing.Size(75, 23);
            this.RESET.TabIndex = 1;
            this.RESET.Text = "RESET";
            this.RESET.UseVisualStyleBackColor = true;
            this.RESET.Click += new System.EventHandler(this.RESET_Click);
            // 
            // START
            // 
            this.START.Location = new System.Drawing.Point(13, 13);
            this.START.Name = "START";
            this.START.Size = new System.Drawing.Size(75, 23);
            this.START.TabIndex = 0;
            this.START.Text = "START";
            this.START.UseVisualStyleBackColor = true;
            this.START.Click += new System.EventHandler(this.START_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(364, 337);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // ShopLens
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(766, 418);
            this.Controls.Add(this.Intro);
            this.Name = "ShopLens";
            this.Text = "ShopLens";
            ((System.ComponentModel.ISupportInitialize)(this.Logo)).EndInit();
            this.Intro.ResumeLayout(false);
            this.Intro.PerformLayout();
            this.MainWindow.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
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
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button EXIT;
        private System.Windows.Forms.Button CAPTURE;
        private System.Windows.Forms.Button PAUSE;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button RESET;
        private System.Windows.Forms.Button START;
        private PictureBox pictureBox1;
    }
}

