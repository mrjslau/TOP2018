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
            this.StartRecognitionBtn = new System.Windows.Forms.Button();
            this.CommandOutputBox = new System.Windows.Forms.RichTextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.CommandLbl = new System.Windows.Forms.Label();
            this.EngineOutputLbl = new System.Windows.Forms.Label();
            this.recognitionGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // recognitionGroup
            // 
            this.recognitionGroup.Controls.Add(this.EngineOutputLbl);
            this.recognitionGroup.Controls.Add(this.CommandLbl);
            this.recognitionGroup.Controls.Add(this.richTextBox1);
            this.recognitionGroup.Controls.Add(this.CommandOutputBox);
            this.recognitionGroup.Controls.Add(this.StartRecognitionBtn);
            this.recognitionGroup.Location = new System.Drawing.Point(12, 12);
            this.recognitionGroup.Name = "recognitionGroup";
            this.recognitionGroup.Size = new System.Drawing.Size(534, 261);
            this.recognitionGroup.TabIndex = 0;
            this.recognitionGroup.TabStop = false;
            this.recognitionGroup.Text = "Recognize voice";
            this.recognitionGroup.Enter += new System.EventHandler(this.GroupBox1_Enter);
            // 
            // StartRecognitionBtn
            // 
            this.StartRecognitionBtn.Location = new System.Drawing.Point(359, 226);
            this.StartRecognitionBtn.Name = "StartRecognitionBtn";
            this.StartRecognitionBtn.Size = new System.Drawing.Size(159, 26);
            this.StartRecognitionBtn.TabIndex = 3;
            this.StartRecognitionBtn.Text = "Start voice recognition";
            this.StartRecognitionBtn.UseVisualStyleBackColor = true;
            // 
            // CommandOutputBox
            // 
            this.CommandOutputBox.Location = new System.Drawing.Point(144, 53);
            this.CommandOutputBox.Name = "CommandOutputBox";
            this.CommandOutputBox.Size = new System.Drawing.Size(384, 167);
            this.CommandOutputBox.TabIndex = 4;
            this.CommandOutputBox.Text = "";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(9, 53);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(102, 167);
            this.richTextBox1.TabIndex = 5;
            this.richTextBox1.Text = "";
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
            // EngineOutputLbl
            // 
            this.EngineOutputLbl.AutoSize = true;
            this.EngineOutputLbl.Location = new System.Drawing.Point(273, 30);
            this.EngineOutputLbl.Name = "EngineOutputLbl";
            this.EngineOutputLbl.Size = new System.Drawing.Size(135, 13);
            this.EngineOutputLbl.TabIndex = 7;
            this.EngineOutputLbl.Text = "Recognition Engine Output";
            // 
            // VoiceRecognitionTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(558, 282);
            this.Controls.Add(this.recognitionGroup);
            this.Name = "VoiceRecognitionTest";
            this.Text = "VoiceRecognitionTest";
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
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}

