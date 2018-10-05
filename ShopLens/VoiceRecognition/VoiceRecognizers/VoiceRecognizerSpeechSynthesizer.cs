using System;
using System.Speech.Recognition;

namespace VoiceRecognitionWithTextVoicer.VoiceRecognizers
{
    public class VoiceRecognizerSpeechSynthesizer : IVoiceRecognizer
    {
        private SpeechRecognitionEngine speechRecognizer;  //The voice recognizer.

        public VoiceRecognizerSpeechSynthesizer()
        {
            speechRecognizer = new SpeechRecognitionEngine();
        }

        //Starts the voice recognition process.
        public void StartVoiceRecognition()
        {
            speechRecognizer.SetInputToDefaultAudioDevice(); //Sets the input of the recognizer to the microphone (if it exists).
            speechRecognizer.RecognizeAsync(RecognizeMode.Multiple); //Recognizes speech one statement at a time, asynchronously.
        }

        public void StopVoiceRecognition()
        {
            //Stop recognizing phrases.
            speechRecognizer.RecognizeAsyncStop();
        }

        //Adds a new grammar to the speech recognizer engine
        //and returns a command's grammar object so that it could be possible
        //to add a specific event for this command.
        public void AddCommand(string command, Action<object, EventArgs> CommandRecognized_MethodToRun)
        {
            //Adds a string of phrases to be distinguished by the voice recognizer.
            Choices commandChoices = new Choices();
            commandChoices.Add(command);

            GrammarBuilder grammarBuilder = new GrammarBuilder();
            grammarBuilder.Append(commandChoices);
            Grammar newGrammar = new Grammar(grammarBuilder);
            newGrammar.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(CommandRecognized_MethodToRun);

            speechRecognizer.LoadGrammarAsync(newGrammar); //Loads the phrases asynchronously.
        }
    }
}
