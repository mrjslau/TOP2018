using System.Speech.Synthesis;

namespace VoicedText.TextVoicers
{
    public class TextVoicer : IVoicer
    {
        private SpeechSynthesizer textVoicer;
        private PromptBuilder messageBuilder; //Used to form a sequence of sentences in various speeds, voices.
        private PromptStyle voicerSpeed;

        public string HelloMessageText { get; private set; }
        public string ChooseMessageSpeedText { get; private set; }

        //The speed at which the voicer says things.
        //By default the speed is normal.
        public int SpeedOfVoicer { get; set; }

        public int MaxVolumeValue { get; set; }

        public TextVoicer()
        {
            textVoicer = new SpeechSynthesizer();
            messageBuilder = new PromptBuilder();

            MaxVolumeValue = 100;

            HelloMessageText = "Hello, I am a text voicer. Please, write something nice in the input field for me to say.";
            ChooseMessageSpeedText = "You can also choose the speed at which I talk.";
            SpeedOfVoicer = 0; //Default voice speed.
        }

        public void SetVolume(int newVolume)
        {
            textVoicer.Volume = newVolume;
        }

        public void SetSpeed(int speedOfVoicer)
        {
            switch (speedOfVoicer)
            {
                case -1: //Slow speed.
                    voicerSpeed = (new PromptStyle(PromptRate.ExtraSlow));
                    break;

                case 0: //Normal speed.
                    voicerSpeed = (new PromptStyle(PromptRate.Medium));
                    break;

                case 1: //Fast speed.
                    voicerSpeed = (new PromptStyle(PromptRate.ExtraFast));
                    break;
            }

        }

        //Voices any message at a desired speed.
        public void SayMessage(string message)
        {
            SetSpeed(SpeedOfVoicer);

            messageBuilder.StartStyle(voicerSpeed);
            messageBuilder.AppendText(message);
            messageBuilder.EndStyle();
            textVoicer.SpeakAsync(messageBuilder);

            messageBuilder.ClearContent(); //Removes sentences that have already been appended.
        }

    }
}
