using System;
using System.Speech.Recognition;

namespace VoiceRecognitionWithTextVoicer.VoiceRecognizers
{
    public class VoiceRecognizerSpeechRecEngine : IVoiceRecognizer
    {
        private SpeechRecognitionEngine speechRecognizer;

        public VoiceRecognizerSpeechRecEngine()
        {
            speechRecognizer = new SpeechRecognitionEngine();
        }

        /// <summary>
        /// Starts the voice recognition process.
        /// </summary>
        /// <remarks>
        /// The voice recognition process is started asynchronously.
        /// </remarks>
        public void StartVoiceRecognition()
        {
            speechRecognizer.SetInputToDefaultAudioDevice();
            speechRecognizer.RecognizeAsync(RecognizeMode.Multiple);
        }

        /// <inheritdoc cref="IVoiceRecognizer.StopVoiceRecognition(int)"/>
        public void StopVoiceRecognition()
        {
            speechRecognizer.RecognizeAsyncStop();
        }

        /// <inheritdoc cref="ITextVoicer.SayMessage(int)"/>
        public void AddCommand(string command, Action<object, EventArgs> CommandRecognized_MethodToRun)
        {
            Choices commandChoices = new Choices();
            commandChoices.Add(command);

            GrammarBuilder grammarBuilder = new GrammarBuilder();
            grammarBuilder.Append(commandChoices);
            Grammar newGrammar = new Grammar(grammarBuilder);
            newGrammar.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(CommandRecognized_MethodToRun);

            speechRecognizer.LoadGrammarAsync(newGrammar);
        }
    }
}
