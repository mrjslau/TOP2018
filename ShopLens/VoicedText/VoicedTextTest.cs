using System;
using System.Windows.Forms;

namespace VoicedText
{
    public partial class VoicedTextTest : Form
    {
        //A text voicer.
        private TextVoicer textVoicer = new TextVoicer();

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
            textVoicer.SayMessage(textVoicer.HelloMessageText);
            textVoicer.SayMessage(textVoicer.ChooseMessageSpeedText);
        }

        private void InputTextBox_TextChanged(object sender, EventArgs e)
        {
            inputMessage = InputTextBox.Text;
        }

        private void CommenceVoiceBtn_Click(object sender, EventArgs e)
        {
            if (NormalSpdRadBtn.Checked) //Normal voicer speed.
            {
                textVoicer.SpeedOfVoicer = 0;

            }
            else if (SlowSpdRadBtn.Checked) //Slow voicer speed.
            {
                textVoicer.SpeedOfVoicer = -1;

            }
            else if (FastSpdRadBtn.Checked) //Fast voicer speed.
            {
                textVoicer.SpeedOfVoicer = 1;
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
