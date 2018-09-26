using System.Speech.Synthesis;

namespace VoicedText.TextVoicers
{
    public class TextVoicer : IVoicer
    {
        private SpeechSynthesizer textVoicer;
        private PromptBuilder messageBuilder; //Used to form a sequence of sentences in various speeds, voices.

        public string HelloMessageText { get; private set; }
        public string ChooseMessageSpeedText { get; private set; }

        //The speed at which the voicer says things.
        //By default the speed is normal.
        private int speedOfVoicer;
        public int SpeedOfVoicer
        {
            get { return speedOfVoicer; }
            set { speedOfVoicer = value; }
        }

        //Should we use auto properties instead (the IDE suggests to do so, 
        //but that first capital letter naming convention, man)?
        //public int SpeedOfVoicer { get; set; }

        public int MaxVolumeValue { get; set; }

        public TextVoicer()
        {
            textVoicer = new SpeechSynthesizer();
            messageBuilder = new PromptBuilder();

            MaxVolumeValue = 100;

            HelloMessageText = "Hello, I am a text voicer. Please, write something nice in the input field for me to say.";
            ChooseMessageSpeedText = "You can also choose the speed at which I talk.";
            speedOfVoicer = 0; //Default voice speed.
        }

        public void SetVolume(int newVolume)
        {
            textVoicer.Volume = newVolume;
        }

        //Voices any message at a desired speed.
        public void SayMessage(string message)
        {
            if (SpeedOfVoicer == 0) //Normal speed.
            {
                textVoicer.SpeakAsync(message); //The voicer will speak asynchronously so that it wouldn't "hang" the main Thread.

            }
            else if (SpeedOfVoicer == -1) //Slow speed.
            {
                messageBuilder.StartStyle(new PromptStyle(PromptRate.ExtraSlow));
                messageBuilder.AppendText(message);
                messageBuilder.EndStyle();
                textVoicer.SpeakAsync(messageBuilder);
            }
            else if (SpeedOfVoicer == 1) //Fast speed.
            {
                messageBuilder.StartStyle(new PromptStyle(PromptRate.Fast));
                messageBuilder.AppendText(message);
                messageBuilder.EndStyle();
                textVoicer.SpeakAsync(messageBuilder);
            }
            
                messageBuilder.ClearContent(); //Removes sentences that have already been appended.
        }

    }
}
