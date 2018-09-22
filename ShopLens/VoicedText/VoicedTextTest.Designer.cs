namespace VoicedText
{
    partial class VoicedTextTest
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
            this.InputTextBox = new System.Windows.Forms.TextBox();
            this.VoiceTextGrpBox = new System.Windows.Forms.GroupBox();
            this.InputTextLbl = new System.Windows.Forms.Label();
            this.CommenceVoiceBtn = new System.Windows.Forms.Button();
            this.VoiceSpeedGrpBox = new System.Windows.Forms.GroupBox();
            this.FastSpdRadBtn = new System.Windows.Forms.RadioButton();
            this.SlowSpdRadBtn = new System.Windows.Forms.RadioButton();
            this.NormalSpdRadBtn = new System.Windows.Forms.RadioButton();
            this.VoiceTextGrpBox.SuspendLayout();
            this.VoiceSpeedGrpBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // InputTextBox
            // 
            this.InputTextBox.Location = new System.Drawing.Point(80, 36);
            this.InputTextBox.Name = "InputTextBox";
            this.InputTextBox.Size = new System.Drawing.Size(321, 20);
            this.InputTextBox.TabIndex = 0;
            this.InputTextBox.TextChanged += new System.EventHandler(this.InputTextBox_TextChanged);
            // 
            // VoiceTextGrpBox
            // 
            this.VoiceTextGrpBox.Controls.Add(this.InputTextLbl);
            this.VoiceTextGrpBox.Controls.Add(this.InputTextBox);
            this.VoiceTextGrpBox.Location = new System.Drawing.Point(12, 10);
            this.VoiceTextGrpBox.Name = "VoiceTextGrpBox";
            this.VoiceTextGrpBox.Size = new System.Drawing.Size(420, 82);
            this.VoiceTextGrpBox.TabIndex = 1;
            this.VoiceTextGrpBox.TabStop = false;
            this.VoiceTextGrpBox.Text = "Voice text";
            // 
            // InputTextLbl
            // 
            this.InputTextLbl.AutoSize = true;
            this.InputTextLbl.Location = new System.Drawing.Point(20, 39);
            this.InputTextLbl.Name = "InputTextLbl";
            this.InputTextLbl.Size = new System.Drawing.Size(54, 13);
            this.InputTextLbl.TabIndex = 1;
            this.InputTextLbl.Text = "Input text:";
            // 
            // CommenceVoiceBtn
            // 
            this.CommenceVoiceBtn.Location = new System.Drawing.Point(342, 192);
            this.CommenceVoiceBtn.Name = "CommenceVoiceBtn";
            this.CommenceVoiceBtn.Size = new System.Drawing.Size(71, 23);
            this.CommenceVoiceBtn.TabIndex = 2;
            this.CommenceVoiceBtn.Text = "Voice it!";
            this.CommenceVoiceBtn.UseVisualStyleBackColor = true;
            this.CommenceVoiceBtn.Click += new System.EventHandler(this.CommenceVoiceBtn_Click);
            // 
            // VoiceSpeedGrpBox
            // 
            this.VoiceSpeedGrpBox.Controls.Add(this.FastSpdRadBtn);
            this.VoiceSpeedGrpBox.Controls.Add(this.SlowSpdRadBtn);
            this.VoiceSpeedGrpBox.Controls.Add(this.NormalSpdRadBtn);
            this.VoiceSpeedGrpBox.Location = new System.Drawing.Point(12, 103);
            this.VoiceSpeedGrpBox.Name = "VoiceSpeedGrpBox";
            this.VoiceSpeedGrpBox.Size = new System.Drawing.Size(419, 71);
            this.VoiceSpeedGrpBox.TabIndex = 3;
            this.VoiceSpeedGrpBox.TabStop = false;
            this.VoiceSpeedGrpBox.Text = "Text voice speed";
            this.VoiceSpeedGrpBox.Enter += new System.EventHandler(this.VoiceSpeedGrpBox_Enter);
            // 
            // FastSpdRadBtn
            // 
            this.FastSpdRadBtn.AutoSize = true;
            this.FastSpdRadBtn.Location = new System.Drawing.Point(329, 32);
            this.FastSpdRadBtn.Name = "FastSpdRadBtn";
            this.FastSpdRadBtn.Size = new System.Drawing.Size(45, 17);
            this.FastSpdRadBtn.TabIndex = 2;
            this.FastSpdRadBtn.TabStop = true;
            this.FastSpdRadBtn.Text = "Fast";
            this.FastSpdRadBtn.UseVisualStyleBackColor = true;
            // 
            // SlowSpdRadBtn
            // 
            this.SlowSpdRadBtn.AutoSize = true;
            this.SlowSpdRadBtn.Location = new System.Drawing.Point(186, 32);
            this.SlowSpdRadBtn.Name = "SlowSpdRadBtn";
            this.SlowSpdRadBtn.Size = new System.Drawing.Size(48, 17);
            this.SlowSpdRadBtn.TabIndex = 1;
            this.SlowSpdRadBtn.TabStop = true;
            this.SlowSpdRadBtn.Text = "Slow";
            this.SlowSpdRadBtn.UseVisualStyleBackColor = true;
            // 
            // NormalSpdRadBtn
            // 
            this.NormalSpdRadBtn.AutoSize = true;
            this.NormalSpdRadBtn.Location = new System.Drawing.Point(35, 32);
            this.NormalSpdRadBtn.Name = "NormalSpdRadBtn";
            this.NormalSpdRadBtn.Size = new System.Drawing.Size(58, 17);
            this.NormalSpdRadBtn.TabIndex = 0;
            this.NormalSpdRadBtn.TabStop = true;
            this.NormalSpdRadBtn.Text = "Normal";
            this.NormalSpdRadBtn.UseVisualStyleBackColor = true;
            // 
            // VoicedTextTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 228);
            this.Controls.Add(this.VoiceSpeedGrpBox);
            this.Controls.Add(this.CommenceVoiceBtn);
            this.Controls.Add(this.VoiceTextGrpBox);
            this.Name = "VoicedTextTest";
            this.Text = "VoicedTextTest";
            this.Load += new System.EventHandler(this.VoicedTextTest_Load);
            this.Shown += new System.EventHandler(this.VoicedTextTest_Shown);
            this.VoiceTextGrpBox.ResumeLayout(false);
            this.VoiceTextGrpBox.PerformLayout();
            this.VoiceSpeedGrpBox.ResumeLayout(false);
            this.VoiceSpeedGrpBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox InputTextBox;
        private System.Windows.Forms.GroupBox VoiceTextGrpBox;
        private System.Windows.Forms.Label InputTextLbl;
        private System.Windows.Forms.Button CommenceVoiceBtn;
        private System.Windows.Forms.GroupBox VoiceSpeedGrpBox;
        private System.Windows.Forms.RadioButton FastSpdRadBtn;
        private System.Windows.Forms.RadioButton SlowSpdRadBtn;
        private System.Windows.Forms.RadioButton NormalSpdRadBtn;
    }
}

