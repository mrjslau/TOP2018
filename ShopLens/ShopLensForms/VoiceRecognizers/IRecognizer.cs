namespace WindowsFormsApp.VoiceRecognizers
{
    public interface IRecognizer
    {
        void StartVoiceRecognition();
        void StopVoiceRecognition();

        object AddCommand(string command);
    }
}
