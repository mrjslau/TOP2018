using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;

namespace VoicedText
{
    public class TextVoicer
    {
        private SpeechSynthesizer textVoicer;
        private PromptBuilder messageBuilder; //Used to form a sequence of sentences in various speeds, voices.

        private const string helloMessage = "Hello, I am a text voicer. Please, write something nice in the input field for me to say.";
        private const string chooseMessageSpeed = "You can also choose the speed at which I voice text.";

        //The speed at which the voicer says things.
        //By default the speed is normal.
        private int speedOfVoicer = 0;

        public TextVoicer()
        {
            textVoicer = new SpeechSynthesizer();
            messageBuilder = new PromptBuilder();
        }

        //Voices any message at a desired speed.
        public void SayMessage(string message)
        {
            if(this.speedOfVoicer == 0) //Normal speed.
            {
                textVoicer.Speak(message);

            } else if(this.speedOfVoicer == -1) //Slow speed.
                    {
                        messageBuilder.StartStyle(new PromptStyle(PromptRate.ExtraSlow));
                        messageBuilder.AppendText(message);
                        messageBuilder.EndStyle();
                        textVoicer.Speak(messageBuilder);
                    }
                    else if(this.speedOfVoicer == 1) //Fast speed.
                        {
                            messageBuilder.StartStyle(new PromptStyle(PromptRate.Fast));
                            messageBuilder.AppendText(message);
                            messageBuilder.EndStyle();
                            textVoicer.Speak(messageBuilder);
                        }

            messageBuilder.ClearContent(); //Removes sentences that have already been appended.
        }

        public string GetHelloMessage()
        {
            return helloMessage;
        }

        public string GetChooseMessageSpeed()
        {
            return chooseMessageSpeed;
        }

        public void SetSpeedOfVoicer(int speedOfVoicer)
        {
            this.speedOfVoicer = speedOfVoicer;
        }

    }
}
