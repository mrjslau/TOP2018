namespace ShopLensForms
{
    partial class IntroForm
    {
        private System.ComponentModel.IContainer components = null;
   
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
            this.welcome_label = new System.Windows.Forms.Label();
            this.Enter_btn = new System.Windows.Forms.Button();
            this.Logo = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.Logo)).BeginInit();
            this.SuspendLayout();
            // 
            // welcome_label
            // 
            this.welcome_label.AutoSize = true;
            this.welcome_label.Font = new System.Drawing.Font("Arial", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.welcome_label.Location = new System.Drawing.Point(448, 164);
            this.welcome_label.Name = "welcome_label";
            this.welcome_label.Size = new System.Drawing.Size(275, 56);
            this.welcome_label.TabIndex = 0;
            this.welcome_label.Text = "WELCOME";
            // 
            // Enter_btn
            // 
            this.Enter_btn.BackColor = System.Drawing.Color.Transparent;
            this.Enter_btn.FlatAppearance.BorderColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.Enter_btn.FlatAppearance.BorderSize = 0;
            this.Enter_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Enter_btn.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.Enter_btn.ForeColor = System.Drawing.Color.Black;
            this.Enter_btn.Location = new System.Drawing.Point(387, 226);
            this.Enter_btn.Name = "Enter_btn";
            this.Enter_btn.Size = new System.Drawing.Size(386, 43);
            this.Enter_btn.TabIndex = 1;
            this.Enter_btn.Text = "PRESS ENTER TO STRAT";
            this.Enter_btn.UseVisualStyleBackColor = false;
            this.Enter_btn.Click += new System.EventHandler(this.Enter_btn_Click);
            // 
            // Logo
            // 
            this.Logo.Image = global::ShopLensApp.Properties.Resources.shopLensLogo;
            this.Logo.Location = new System.Drawing.Point(65, 73);
            this.Logo.Name = "Logo";
            this.Logo.Size = new System.Drawing.Size(295, 288);
            this.Logo.TabIndex = 2;
            this.Logo.TabStop = false;
            // 
            // IntroFrom
            // 
            this.AcceptButton = this.Enter_btn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.ControlBox = false;
            this.Controls.Add(this.Logo);
            this.Controls.Add(this.Enter_btn);
            this.Controls.Add(this.welcome_label);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "IntroFrom";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Shown += new System.EventHandler(this.IntroForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.Logo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label welcome_label;
        private System.Windows.Forms.Button Enter_btn;
        private System.Windows.Forms.PictureBox Logo;
    }
}