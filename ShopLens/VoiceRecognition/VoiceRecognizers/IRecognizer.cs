using System;

namespace ShopLensApp.VoiceRecognizers
{
    public interface IRecognizer
    {
        void StartVoiceRecognition();
        void StopVoiceRecognition();

        void AddCommand(string command, Action<object, EventArgs> CommandRecognized);
    }
}
