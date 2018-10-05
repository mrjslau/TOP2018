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
        void SetSpeed(int speedOfVoicer);

        /// <summary>
        /// Sets the volume of the text voicer.
        /// </summary>
        /// /// <returns></returns>
        void SetVolume(int volumeOfVoicer);

        /// <summary>
        /// Voices a given message.
        /// </summary>
        /// <param name="message">A string of a message to be voiced.</param>
        /// <returns></returns>
        void SayMessage(string message);
    }
}
