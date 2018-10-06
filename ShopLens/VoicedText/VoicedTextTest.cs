using System;
using System.Windows.Forms;
using VoicedText.TextVoicers;

namespace VoicedText
{
    public partial class VoicedTextTest : Form
    {
        //A text voicer.
        private TextVoicerSpeechSynthesizer textVoicer = new TextVoicerSpeechSynthesizer();

        //The message that the user wrote.
        private string inputMessage;

        public VoicedTextTest()
        {
            InitializeComponent();
        }

        private void VoicedTextTest_Load(object sender, EventArgs e)
        {
            //Text voicer speed is set to normal by default.
            NormalSpdRadBtn.Checked = true;
        }

        //This method is called after the form has been loaded and shown to the user.
        private void VoicedTextTest_Shown(object sender, EventArgs e)
        {
            //Text voicer says hello and mentions the ability to choose voice.
            textVoicer.SayMessage("Hello I am a text voicer.");
            textVoicer.SayMessage("Please write something in the " +
                "input field for me to say.");
        }

        private void InputTextBox_TextChanged(object sender, EventArgs e)
        {
            inputMessage = InputTextBox.Text;
        }

        private void CommenceVoiceBtn_Click(object sender, EventArgs e)
        {
            if (NormalSpdRadBtn.Checked) //Normal voicer speed.
            {
                textVoicer.speedOfVoicer = SpeedOfVoicer.Normal;

            }
            else if (SlowSpdRadBtn.Checked) //Slow voicer speed.
            {
                textVoicer.speedOfVoicer = SpeedOfVoicer.ExtraSlow;

            }
            else if (FastSpdRadBtn.Checked) //Fast voicer speed.
            {
                textVoicer.speedOfVoicer = SpeedOfVoicer.Fast;
            }

            if (!String.IsNullOrEmpty(inputMessage))
            {
                textVoicer.SayMessage(inputMessage);
            }
        }

        private void VoiceSpeedGrpBox_Enter(object sender, EventArgs e)
        {

        }

        private void VoiceTextGrpBox_Enter(object sender, EventArgs e)
        {

        }
    }
}
