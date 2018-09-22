using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            radioButton1.Checked = true;
        }

        //This method is called after the form has been loaded.
        private void VoicedTextTest_Shown(object sender, EventArgs e)
        {
            //Text voicer says hello and mentions ability to choose voice.
            textVoicer.SayMessage(textVoicer.GetHelloMessage());
            textVoicer.SayMessage(textVoicer.GetChooseMessageSpeed());
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            inputMessage = textBox1.Text;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked) //Normal voicer speed.
            {
                textVoicer.SetSpeedOfVoicer(0);

            } else if (radioButton2.Checked) //Slow voicer speed.
            {
                        textVoicer.SetSpeedOfVoicer(-1);

                    } else if(radioButton3.Checked) //Fast voicer speed.
                            {
                                textVoicer.SetSpeedOfVoicer(1);
                            }



                if (!String.IsNullOrEmpty(inputMessage))
                {
                    textVoicer.SayMessage(inputMessage);
                }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}
