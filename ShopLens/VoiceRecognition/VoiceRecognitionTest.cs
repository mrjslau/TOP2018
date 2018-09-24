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

        //Closing and opening statements of the voice recognizer.
        private const string openingStatement = "\nTalk to me now, please.\n";
        private const string closingStatement = "I will go to sleep now.\n";

        //Available user voice commands.
        private const string helloCmd = "Hello";
        private const string whatIsLuvCmd = "What is love";
        private const string somethingShowCmd = "Show me something";
        private const string sayStupidCmd = "Say something stupid";
        private const string howUDoingCmd = "How are you doing today";
        private const string meaningLifeCmd = "What is the meaning of life";
        private const string iLuvUCmd = "I love you";
        private const string stopRecognitionCmd = "Stop voice recognition";

        private string[] voiceCommands = { helloCmd, whatIsLuvCmd, somethingShowCmd, sayStupidCmd,
            howUDoingCmd, meaningLifeCmd, iLuvUCmd, stopRecognitionCmd};

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
            CommandOutputBox.Text += openingStatement;
        }

        //The speech recognizer would call this method, when it would recognize a particular command.
        public void SpeechRecognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string speechResult = e.Result.Text;

            if (speechResult == helloCmd)
            {
                CommandOutputBox.Text += "Hello, I am a voice recognizer.\n";
            }
            else if(speechResult == whatIsLuvCmd)
            {
                CommandOutputBox.Text += "\"Baby, don't hurt me, don't hurt me, no more\".\n";
            }
            else if (speechResult == somethingShowCmd)
            {
                CommandOutputBox.Text += "Here you go.\n";
                MessageBox.Show("Something");
            }
            else if (speechResult == sayStupidCmd)
            {
                CommandOutputBox.Text += "Something stupid.\n";
            }
            else if (speechResult == howUDoingCmd)
            {
                CommandOutputBox.Text += "I am doing today, yes, it's pretty simple.\n";
            }
            else if (speechResult == meaningLifeCmd)
            {
                CommandOutputBox.Text += "Everything in life has meaning, that's the meaning of life.\n";
            }
            else if (speechResult == iLuvUCmd)
            {
                CommandOutputBox.Text += "I love you too.\n";
            }
            else if (speechResult == stopRecognitionCmd)
            {
                voiceRecognizer.StopVoiceRecognition();
                CommandOutputBox.Text += closingStatement;
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
