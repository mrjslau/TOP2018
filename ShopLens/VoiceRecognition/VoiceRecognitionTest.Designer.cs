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
            this.label1 = new System.Windows.Forms.Label();
            this.checkIfRecognized2 = new System.Windows.Forms.CheckBox();
            this.recognitionButton = new System.Windows.Forms.Button();
            this.checkIfRecognized1 = new System.Windows.Forms.CheckBox();
            this.inputGrammarLabel = new System.Windows.Forms.Label();
            this.grammarInputBox = new System.Windows.Forms.TextBox();
            this.recognitionGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // recognitionGroup
            // 
            this.recognitionGroup.Controls.Add(this.label1);
            this.recognitionGroup.Controls.Add(this.checkIfRecognized2);
            this.recognitionGroup.Controls.Add(this.recognitionButton);
            this.recognitionGroup.Controls.Add(this.checkIfRecognized1);
            this.recognitionGroup.Controls.Add(this.inputGrammarLabel);
            this.recognitionGroup.Controls.Add(this.grammarInputBox);
            this.recognitionGroup.Location = new System.Drawing.Point(12, 12);
            this.recognitionGroup.Name = "recognitionGroup";
            this.recognitionGroup.Size = new System.Drawing.Size(534, 252);
            this.recognitionGroup.TabIndex = 0;
            this.recognitionGroup.TabStop = false;
            this.recognitionGroup.Text = "Recognize voice";
            this.recognitionGroup.Enter += new System.EventHandler(this.GroupBox1_Enter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(62, 180);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(434, 40);
            this.label1.TabIndex = 5;
            this.label1.Text = "Say \"Please, stop recognizing!\" to stop the voice recognition \r\nonce you have sta" +
    "rted it.\r\n";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // checkIfRecognized2
            // 
            this.checkIfRecognized2.AutoSize = true;
            this.checkIfRecognized2.Location = new System.Drawing.Point(346, 140);
            this.checkIfRecognized2.Name = "checkIfRecognized2";
            this.checkIfRecognized2.Size = new System.Drawing.Size(98, 17);
            this.checkIfRecognized2.TabIndex = 4;
            this.checkIfRecognized2.Text = "Not recognized";
            this.checkIfRecognized2.UseVisualStyleBackColor = true;
            // 
            // recognitionButton
            // 
            this.recognitionButton.Location = new System.Drawing.Point(197, 78);
            this.recognitionButton.Name = "recognitionButton";
            this.recognitionButton.Size = new System.Drawing.Size(159, 33);
            this.recognitionButton.TabIndex = 3;
            this.recognitionButton.Text = "Start voice recognition";
            this.recognitionButton.UseVisualStyleBackColor = true;
            // 
            // checkIfRecognized1
            // 
            this.checkIfRecognized1.AutoSize = true;
            this.checkIfRecognized1.Location = new System.Drawing.Point(123, 140);
            this.checkIfRecognized1.Name = "checkIfRecognized1";
            this.checkIfRecognized1.Size = new System.Drawing.Size(86, 17);
            this.checkIfRecognized1.TabIndex = 2;
            this.checkIfRecognized1.Text = "Recognized!";
            this.checkIfRecognized1.UseVisualStyleBackColor = true;
            // 
            // inputGrammarLabel
            // 
            this.inputGrammarLabel.AutoSize = true;
            this.inputGrammarLabel.Location = new System.Drawing.Point(29, 39);
            this.inputGrammarLabel.Name = "inputGrammarLabel";
            this.inputGrammarLabel.Size = new System.Drawing.Size(114, 13);
            this.inputGrammarLabel.TabIndex = 1;
            this.inputGrammarLabel.Text = "Input a word to detect:";
            // 
            // grammarInputBox
            // 
            this.grammarInputBox.Location = new System.Drawing.Point(149, 36);
            this.grammarInputBox.Name = "grammarInputBox";
            this.grammarInputBox.Size = new System.Drawing.Size(120, 20);
            this.grammarInputBox.TabIndex = 0;
            // 
            // VoiceRecognitionTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(558, 276);
            this.Controls.Add(this.recognitionGroup);
            this.Name = "VoiceRecognitionTest";
            this.Text = "VoiceRecognitionTest";
            this.recognitionGroup.ResumeLayout(false);
            this.recognitionGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox recognitionGroup;
        private System.Windows.Forms.CheckBox checkIfRecognized2;
        private System.Windows.Forms.Button recognitionButton;
        private System.Windows.Forms.CheckBox checkIfRecognized1;
        private System.Windows.Forms.Label inputGrammarLabel;
        private System.Windows.Forms.TextBox grammarInputBox;
        private System.Windows.Forms.Label label1;
    }
}

