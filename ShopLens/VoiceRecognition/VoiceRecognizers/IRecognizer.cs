using System;

namespace VoiceRecognitionWithTextVoicer.VoiceRecognizers
{
    public interface IVoiceRecognizer
    {
        void StartVoiceRecognition();
        void StopVoiceRecognition();

        void AddCommand(string command, Action<object, EventArgs> CommandRecognized);
    }
}
