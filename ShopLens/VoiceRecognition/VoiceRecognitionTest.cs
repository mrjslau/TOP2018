using System;
using System.Speech.Recognition;
using System.Windows.Forms;
using VoicedText;

namespace VoiceRecognition
{
    public partial class VoiceRecognitionTest : Form
    {
        private VoiceRecognizer voiceRecognizer = new VoiceRecognizer(); //A voice recognizer.
        private TextVoicer textVoicer = new TextVoicer(); //A text voicer.

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

        //Text voicer responses.
        private const string helloRsp = "Hello, I am a voice recognizer.\n";
        private const string whatIsLuvRsp = "\"Baby, don't hurt me, don't hurt me, no more.\"\n";
        private const string somethingShowRsp = "Here you go.\n";
        private const string sayStupidRsp = "Something stupid.\n";
        private const string howUDoingRsp = "I am doing today, yes, it's pretty simple.\n";
        private const string meaningLifeRsp = "Everything in life has meaning, that's the meaning of life.\n";
        private const string iLuvRsp = "I love you, too.\n";
        private const string stopRecognitionRsp = closingStatement;

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
            textVoicer.SayMessage(openingStatement);
            CommandOutputBox.Text += openingStatement;
        }

        //The speech recognizer would call this method, when it would recognize a particular command.
        public void SpeechRecognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string speechResult = e.Result.Text;

            if (speechResult == helloCmd)
            {
                CommandOutputBox.Text += helloRsp;
                textVoicer.SayMessage(helloRsp);
            }
            else if(speechResult == whatIsLuvCmd)
            {
                CommandOutputBox.Text += whatIsLuvRsp;
                textVoicer.SayMessage(whatIsLuvRsp);
            }
            else if (speechResult == somethingShowCmd)
            {
                CommandOutputBox.Text += somethingShowRsp;
                textVoicer.SayMessage(somethingShowRsp);
                MessageBox.Show("Something");
            }
            else if (speechResult == sayStupidCmd)
            {
                CommandOutputBox.Text += sayStupidRsp;
                textVoicer.SayMessage(sayStupidRsp);
            }
            else if (speechResult == howUDoingCmd)
            {
                CommandOutputBox.Text += howUDoingRsp;
                textVoicer.SayMessage(howUDoingRsp);
            }
            else if (speechResult == meaningLifeCmd)
            {
                CommandOutputBox.Text += meaningLifeRsp;
                textVoicer.SayMessage(meaningLifeRsp);
            }
            else if (speechResult == iLuvUCmd)
            {
                CommandOutputBox.Text += iLuvRsp;
                textVoicer.SayMessage(iLuvRsp);
            }
            else if (speechResult == stopRecognitionCmd)
            {
                voiceRecognizer.StopVoiceRecognition();
                CommandOutputBox.Text += closingStatement;
                textVoicer.SayMessage(closingStatement);
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
