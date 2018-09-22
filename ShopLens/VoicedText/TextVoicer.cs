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

        public string HelloMessageText { get; private set; }
        public string ChooseMessageSpeedText { get; private set; }

        //The speed at which the voicer says things.
        //By default the speed is normal.
        public int SpeedOfVoicer { get; set; }

        public TextVoicer()
        {
            textVoicer = new SpeechSynthesizer();
            messageBuilder = new PromptBuilder();

            HelloMessageText = "Hello, I am a text voicer. Please, write something nice in the input field for me to say.";
            ChooseMessageSpeedText = "You can also choose the speed at which I talk.";
            SpeedOfVoicer = 0; //Default voice speed.
        }

        //Voices any message at a desired speed.
        public void SayMessage(string message)
        {
            if (SpeedOfVoicer == 0) //Normal speed.
            {
                textVoicer.Speak(message);

            }
            else if (SpeedOfVoicer == -1) //Slow speed.
            {
                messageBuilder.StartStyle(new PromptStyle(PromptRate.ExtraSlow));
                messageBuilder.AppendText(message);
                messageBuilder.EndStyle();
                textVoicer.Speak(messageBuilder);
            }
            else if (SpeedOfVoicer == 1) //Fast speed.
            {
                messageBuilder.StartStyle(new PromptStyle(PromptRate.Fast));
                messageBuilder.AppendText(message);
                messageBuilder.EndStyle();
                textVoicer.Speak(messageBuilder);
            }
            
                messageBuilder.ClearContent(); //Removes sentences that have already been appended.
        }

    }
}
