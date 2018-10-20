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
        /// The current speed of the voicer.
        /// </summary>
        public SpeedOfVoicer speedOfVoicer;

        /// <summary>
        /// The current volume of the voicer.
        /// </summary>
        public VolumeOfVoicer volumeOfVoicer;

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

            speedOfVoicer = SpeedOfVoicer.Normal;
            volumeOfVoicer = VolumeOfVoicer.Max;

            SetSpeed(speedOfVoicer);
            SetVolume(volumeOfVoicer);
        }

        /// <summary>
        /// Sets the volume of the voicer.
        /// </summary>
        /// <param name="volumeOfVoicer">
        /// 
        /// </param>
        /// <returns></returns>
        public void SetVolume(VolumeOfVoicer volumeOfVoicer)
        {
            this.volumeOfVoicer = volumeOfVoicer;

            switch (volumeOfVoicer)
            {
                case VolumeOfVoicer.NoVolume:
                    textVoicer.Volume = 0;
                    break;

                case VolumeOfVoicer.Low:
                    textVoicer.Volume = 25;
                    break;

                case VolumeOfVoicer.Medium:
                    textVoicer.Volume = 50;
                    break;

                case VolumeOfVoicer.High:
                    textVoicer.Volume = 75;
                    break;

                case VolumeOfVoicer.Max:
                    textVoicer.Volume = 100;
                    break;

                default:
                    throw new InvalidEnumArgumentException("Invalid VolumeOfVoicer enum argument passed.");
            }   
        }

        /// <summary>
        /// Sets the speed of the voicer.
        /// </summary>
        /// <param name="speedOfVoicer">
        /// Pass 0 for ExtraSlow, 1 for Slow, 2 for Normal, 3 for Fast, 4 for ExtraFast speed.
        /// </param>
        /// <returns></returns>
        public void SetSpeed(SpeedOfVoicer speedOfVoicer)
        {
            this.speedOfVoicer = speedOfVoicer;

            switch (speedOfVoicer)
            {
                case SpeedOfVoicer.ExtraSlow:
                    voicerSpeed = (new PromptStyle(PromptRate.ExtraSlow));
                    break;

                case SpeedOfVoicer.Slow:
                    voicerSpeed = (new PromptStyle(PromptRate.Slow));
                    break;

                case SpeedOfVoicer.Normal:
                    voicerSpeed = (new PromptStyle(PromptRate.Medium));
                    break;

                case SpeedOfVoicer.Fast:
                    voicerSpeed = (new PromptStyle(PromptRate.Fast));
                    break;

                case SpeedOfVoicer.ExtraFast:
                    voicerSpeed = (new PromptStyle(PromptRate.ExtraFast));
                    break;

                default:
                    throw new InvalidEnumArgumentException("Invalid SpeedOfVoicer enum argument passed.");
            }

        }

        /// <inheritdoc cref="ITextVoicer.SayMessage(string)"/>
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
