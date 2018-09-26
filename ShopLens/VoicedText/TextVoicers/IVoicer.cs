namespace VoicedText.TextVoicers
{
    public interface IVoicer
    {
        int SpeedOfVoicer { get; set; }
        int MaxVolumeValue { get; set; }  //Is it better to have this field as a constant in a static class?

        void SetVolume(int newVolume);
        void SayMessage(string message);
    }
}
