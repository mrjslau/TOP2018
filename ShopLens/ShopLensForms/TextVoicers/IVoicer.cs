namespace VoicedText.TextVoicers
{
    public interface IVoicer
    {
        int SpeedOfVoicer { get; set; }
        int MaxVolumeValue { get; set; }

        void SetSpeed(int speedOfVoicer);
        void SetVolume(int newVolume);
        void SayMessage(string message);
    }
}
