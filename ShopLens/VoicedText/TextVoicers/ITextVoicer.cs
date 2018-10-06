/// <summary>
/// An enum used to represent the different speeds
/// at which the voicer can talk.
/// </summary>
public enum SpeedOfVoicer
{
    ExtraSlow,
    Slow,
    Normal,
    Fast,
    ExtraFast
}

/// <summary>
/// An enum used to represent the different volumes
/// at which the voicer can talk.
/// </summary>
public enum VolumeOfVoicer
{
    NoVolume,
    Low,
    Medium,
    High,
    Max
}

namespace VoicedText.TextVoicers
{
    /// <summary>
    /// Interface that defines methods needed for a text voicer.
    /// </summary>
    public interface ITextVoicer
    {
        /// <summary>
        /// Sets the speed of the text voicer.
        /// </summary>
        /// <returns></returns>
        void SetSpeed(SpeedOfVoicer voicerSpeedValue);

        /// <summary>
        /// Sets the volume of the text voicer.
        /// </summary>
        /// <returns></returns>
        void SetVolume(VolumeOfVoicer voicerVolumeValue);

        /// <summary>
        /// Voices a given message.
        /// </summary>
        /// <param name="message">A string of a message to be voiced.</param>
        /// <returns></returns>
        void SayMessage(string message);
    }
}
