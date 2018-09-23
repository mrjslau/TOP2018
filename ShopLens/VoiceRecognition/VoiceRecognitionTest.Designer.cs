namespace VoiceRecognition
{
    partial class VoiceRecognitionTest
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
            this.recognitionGroup = new System.Windows.Forms.GroupBox();
            this.EngineOutputLbl = new System.Windows.Forms.Label();
            this.CommandLbl = new System.Windows.Forms.Label();
            this.CommandTextBox = new System.Windows.Forms.RichTextBox();
            this.CommandOutputBox = new System.Windows.Forms.RichTextBox();
            this.StartRecognitionBtn = new System.Windows.Forms.Button();
            this.recognitionGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // recognitionGroup
            // 
            this.recognitionGroup.Controls.Add(this.EngineOutputLbl);
            this.recognitionGroup.Controls.Add(this.CommandLbl);
            this.recognitionGroup.Controls.Add(this.CommandTextBox);
            this.recognitionGroup.Controls.Add(this.CommandOutputBox);
            this.recognitionGroup.Controls.Add(this.StartRecognitionBtn);
            this.recognitionGroup.Location = new System.Drawing.Point(12, 12);
            this.recognitionGroup.Name = "recognitionGroup";
            this.recognitionGroup.Size = new System.Drawing.Size(534, 261);
            this.recognitionGroup.TabIndex = 0;
            this.recognitionGroup.TabStop = false;
            this.recognitionGroup.Text = "Recognize voice";
            this.recognitionGroup.Enter += new System.EventHandler(this.VoiceRecognitionBox_Enter);
            // 
            // EngineOutputLbl
            // 
            this.EngineOutputLbl.AutoSize = true;
            this.EngineOutputLbl.Location = new System.Drawing.Point(273, 30);
            this.EngineOutputLbl.Name = "EngineOutputLbl";
            this.EngineOutputLbl.Size = new System.Drawing.Size(135, 13);
            this.EngineOutputLbl.TabIndex = 7;
            this.EngineOutputLbl.Text = "Recognition Engine Output";
            this.EngineOutputLbl.Click += new System.EventHandler(this.EngineOutputLbl_Click);
            // 
            // CommandLbl
            // 
            this.CommandLbl.AutoSize = true;
            this.CommandLbl.Location = new System.Drawing.Point(6, 30);
            this.CommandLbl.Name = "CommandLbl";
            this.CommandLbl.Size = new System.Drawing.Size(105, 13);
            this.CommandLbl.TabIndex = 6;
            this.CommandLbl.Text = "Available Commands";
            // 
            // CommandTextBox
            // 
            this.CommandTextBox.Location = new System.Drawing.Point(0, 53);
            this.CommandTextBox.Name = "CommandTextBox";
            this.CommandTextBox.Size = new System.Drawing.Size(160, 167);
            this.CommandTextBox.TabIndex = 5;
            this.CommandTextBox.Text = "-Hello\n-What is love?\n-How are you doing today?\n-Show me something\n-Say something" +
    " stupid\n-What is the meaning of life?\n-I love you\n-Stop voice recognition";
            this.CommandTextBox.TextChanged += new System.EventHandler(this.CommandTextBox_TextChanged);
            // 
            // CommandOutputBox
            // 
            this.CommandOutputBox.Location = new System.Drawing.Point(179, 53);
            this.CommandOutputBox.Name = "CommandOutputBox";
            this.CommandOutputBox.Size = new System.Drawing.Size(349, 167);
            this.CommandOutputBox.TabIndex = 4;
            this.CommandOutputBox.Text = "-Output Log-";
            this.CommandOutputBox.TextChanged += new System.EventHandler(this.CommandOutputBox_TextChanged);
            // 
            // StartRecognitionBtn
            // 
            this.StartRecognitionBtn.Location = new System.Drawing.Point(359, 226);
            this.StartRecognitionBtn.Name = "StartRecognitionBtn";
            this.StartRecognitionBtn.Size = new System.Drawing.Size(159, 26);
            this.StartRecognitionBtn.TabIndex = 3;
            this.StartRecognitionBtn.Text = "Start voice recognition";
            this.StartRecognitionBtn.UseVisualStyleBackColor = true;
            this.StartRecognitionBtn.Click += new System.EventHandler(this.StartRecognitionBtn_Click);
            // 
            // VoiceRecognitionTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(558, 282);
            this.Controls.Add(this.recognitionGroup);
            this.Name = "VoiceRecognitionTest";
            this.Text = "VoiceRecognitionTest";
            this.Load += new System.EventHandler(this.VoiceRecognitionTest_Load);
            this.Shown += new System.EventHandler(this.VoiceRecognitionTest_Shown);
            this.recognitionGroup.ResumeLayout(false);
            this.recognitionGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox recognitionGroup;
        private System.Windows.Forms.Button StartRecognitionBtn;
        private System.Windows.Forms.RichTextBox CommandOutputBox;
        private System.Windows.Forms.Label EngineOutputLbl;
        private System.Windows.Forms.Label CommandLbl;
        private System.Windows.Forms.RichTextBox CommandTextBox;
    }
}

