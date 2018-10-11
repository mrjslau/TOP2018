using System;
using System.Speech.Recognition;
using System.Windows.Forms;
using VoicedText.TextVoicers;
using VoiceRecognitionWithTextVoicer.VoiceRecognizers;

namespace VoiceRecognitionWithTextVoicer
{
    public partial class VoiceRecognitionTest : Form
    {
        private IVoiceRecognizer voiceRecognizer = new VoiceRecognizerSpeechRecEngine(); //A voice recognizer.
        private ITextVoicer textVoicer = new TextVoicerSpeechSynthesizer(); //A text voicer.

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
        private const string iLuvURsp = "I love you, too.\n";
        private const string stopRecognitionRsp = closingStatement;

        public VoiceRecognitionTest()
        {
            InitializeComponent();
            voiceRecognizer.AddCommand(helloCmd, CommandRecognized_Hello);
            voiceRecognizer.AddCommand(whatIsLuvCmd, CommandRecognized_WhatIsLuv);
            voiceRecognizer.AddCommand(somethingShowCmd, CommandRecognized_ShowSmth);
            voiceRecognizer.AddCommand(sayStupidCmd, CommandRecognized_StupidSay);
            voiceRecognizer.AddCommand(howUDoingCmd, CommandRecognized_HowUDoin);
            voiceRecognizer.AddCommand(meaningLifeCmd, CommandRecognized_LifeMeaning);
            voiceRecognizer.AddCommand(iLuvUCmd, CommandRecognized_ILuvU);
            voiceRecognizer.AddCommand(stopRecognitionCmd, CommandRecognized_StopRec);
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

        private void CommandRecognized_Hello(object sender, EventArgs e)
        {
            RespondToCommand(helloRsp);
        }

        private void CommandRecognized_WhatIsLuv(object sender, EventArgs e)
        {
            RespondToCommand(whatIsLuvRsp);
        }

        private void CommandRecognized_ShowSmth(object sender, EventArgs e)
        {
            RespondToCommand(somethingShowRsp);
        }

        private void CommandRecognized_StupidSay(object sender, EventArgs e)
        {
            RespondToCommand(sayStupidRsp);
        }

        private void CommandRecognized_HowUDoin(object sender, EventArgs e)
        {
            RespondToCommand(howUDoingRsp);
        }

        private void CommandRecognized_LifeMeaning(object sender, EventArgs e)
        {
            RespondToCommand(meaningLifeRsp);
        }

        private void CommandRecognized_ILuvU(object sender, EventArgs e)
        {
            RespondToCommand(iLuvURsp);
        }

        private void CommandRecognized_StopRec(object sender, EventArgs e)
        {
            voiceRecognizer.StopVoiceRecognition();
            StartRecognitionBtn.Enabled = true;
        }

        public void RespondToCommand(string voicedResponse)
        {
            CommandOutputBox.Text += voicedResponse;
            textVoicer.SayMessage(voicedResponse);
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
