using System.ComponentModel;
using System.Speech.Synthesis;

namespace VoicedText.TextVoicers
{
    public class TextVoicerSpeechSynthesizer : ITextVoicer
    {
        private SpeechSynthesizer textVoicer;
        private PromptBuilder messageBuilder;
        private PromptStyle voicerSpeed;

        /// <summary>
        /// The possible speeds of the voicer.
        /// </summary>
        public enum VoicerSpeed { ExtraSlow, Slow, Normal, Fast, ExtraFast};

        /// <summary>
        /// The current speed of the voicer.
        /// </summary>
        public VoicerSpeed speedOfVoicer;

        /// <summary>
        /// The possible volumes of the voicer.
        /// </summary>
        public enum VoicerVolume { NoVolume, Low, Medium, High, Max};

        /// <summary>
        /// The current volume of the voicer.
        /// </summary>
        public VoicerVolume volumeOfVoicer;

        /// <summary>
        /// The class constructor.
        /// </summary>
        /// <remarks>
        /// By default it sets the speed of the voicer to Normal
        /// and the maximum volume of the voicer to Max.
        /// </remarks>
        public TextVoicerSpeechSynthesizer()
        {
            textVoicer = new SpeechSynthesizer();
            messageBuilder = new PromptBuilder();

            speedOfVoicer = VoicerSpeed.Normal;
            volumeOfVoicer = VoicerVolume.Max;

            SetSpeed((int)speedOfVoicer);
            SetVolume((int)volumeOfVoicer);
        }

        /// <summary>
        /// Sets the volume of the voicer.
        /// </summary>
        /// <param name="volumeOfVoicer">
        /// Pass 0 for NoVolume, 1 for Low, 2 for Medium, 3 for High and 4 for Max volume level.
        /// </param>
        /// <returns></returns>
        public void SetVolume(int volumeOfVoicer)
        {
            this.volumeOfVoicer = (VoicerVolume)volumeOfVoicer;

            switch (volumeOfVoicer)
            {
                case (int)VoicerVolume.NoVolume:
                    textVoicer.Volume = 0;
                    break;

                case (int)VoicerVolume.Low:
                    textVoicer.Volume = 25;
                    break;

                case (int)VoicerVolume.Medium:
                    textVoicer.Volume = 50;
                    break;

                case (int)VoicerVolume.High:
                    textVoicer.Volume = 75;
                    break;

                case (int)VoicerVolume.Max:
                    textVoicer.Volume = 100;
                    break;

                default:
                    throw new InvalidEnumArgumentException("Invalid VoicerVolume enum argument passed.");
            }   
        }

        /// <summary>
        /// Sets the speed of the voicer.
        /// </summary>
        /// <param name="speedOfVoicer">
        /// Pass 0 for ExtraSlow, 1 for Slow, 2 for Normal, 3 for Fast, 4 for ExtraFast speed.
        /// </param>
        /// <returns></returns>
        public void SetSpeed(int speedOfVoicer)
        {
            this.speedOfVoicer = (VoicerSpeed)speedOfVoicer;

            switch (speedOfVoicer)
            {
                case (int)VoicerSpeed.ExtraSlow:
                    voicerSpeed = (new PromptStyle(PromptRate.ExtraSlow));
                    break;

                case (int)VoicerSpeed.Slow:
                    voicerSpeed = (new PromptStyle(PromptRate.Slow));
                    break;

                case (int)VoicerSpeed.Normal:
                    voicerSpeed = (new PromptStyle(PromptRate.Medium));
                    break;

                case (int)VoicerSpeed.Fast:
                    voicerSpeed = (new PromptStyle(PromptRate.Fast));
                    break;

                case (int)VoicerSpeed.ExtraFast:
                    voicerSpeed = (new PromptStyle(PromptRate.ExtraFast));
                    break;

                default:
                    throw new InvalidEnumArgumentException("Invalid VoicerSpeed enum argument passed.");
            }

        }

        /// <inheritdoc cref="ITextVoicer.SayMessage(int)"/>
        public void SayMessage(string message)
        {
            messageBuilder.StartStyle(voicerSpeed);
            messageBuilder.AppendText(message);
            messageBuilder.EndStyle();
            textVoicer.SpeakAsync(messageBuilder);

            messageBuilder.ClearContent(); //Removes sentences that have already been appended.
        }

    }
}
