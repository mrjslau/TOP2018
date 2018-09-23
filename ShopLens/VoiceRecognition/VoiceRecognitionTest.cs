using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VoiceRecognition
{
    public partial class VoiceRecognitionTest : Form
    {
        private VoiceRecognizer voiceRecognizer = new VoiceRecognizer(); //A voice recognizer.

        private string[] voiceCommands = { "hello", "What is love", "Show me something", "Say something stupid",
            "how are you doing today", "what is the meaning of life", "stop voice recognition", "I love you"};

        public VoiceRecognitionTest()
        {
            InitializeComponent();
            voiceRecognizer.VoiceRecForm = this;  //Voice recognizer now has a reference to this specific form.
            voiceRecognizer.AddCommands(voiceCommands);
        }

        private void VoiceRecognitionBox_Enter(object sender, EventArgs e)
        {
        }

        public void VoiceRecognitionTest_Shown(object sender, EventArgs e)
        {
        }

        private void CommandOutputBox_TextChanged(object sender, EventArgs e)
        {
        }

        private void StartRecognitionBtn_Click(object sender, EventArgs e)
        {
            voiceRecognizer.StartVoiceRecognition();
            StartRecognitionBtn.Enabled = false;
            CommandOutputBox.Text += "\nTalk to me now, please\n";
        }

        //The speech recognizer would call this method, when it would recognize a particular command.
        public void SpeechRecognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string speechResult = e.Result.Text;

            if (speechResult == "hello")
            {
                CommandOutputBox.Text += "Hello, I am a voice recognizer.\n";
            }
            else if(speechResult == "What is love")
            {
                CommandOutputBox.Text += "\"Baby, don't hurt me, don't hurt me no more\".\n";
            }
            else if (speechResult == "Show me something")
            {
                CommandOutputBox.Text += "Here you go.\n";
                MessageBox.Show("Something");
            }
            else if (speechResult == "Say something stupid")
            {
                CommandOutputBox.Text += "Something stupid.\n";
            }
            else if (speechResult == "how are you doing today")
            {
                CommandOutputBox.Text += "I am doing today, yes, it's pretty simple.\n";
            }
            else if (speechResult == "what is the meaning of life")
            {
                CommandOutputBox.Text += "Everything in life has meaning, that's the meaning of life.\n";
            }
            else if (speechResult == "I love you")
            {
                CommandOutputBox.Text += "I love you too.\n";
            }
            else if (speechResult == "stop voice recognition")
            {
                voiceRecognizer.StopVoiceRecognition();
                CommandOutputBox.Text += "I will go to sleep now.\n";
                StartRecognitionBtn.Enabled = true;
            }
        }

        private void VoiceRecognitionTest_Load(object sender, EventArgs e)
        {

        }

        private void EngineOutputLbl_Click(object sender, EventArgs e)
        {

        }

        private void CommandTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
