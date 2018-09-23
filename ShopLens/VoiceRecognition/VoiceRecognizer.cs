using System.Speech.Recognition;

namespace VoiceRecognition
{
    class VoiceRecognizer
    {
        private SpeechRecognitionEngine speechRecognizer;  //The voice recognizer.

        public VoiceRecognitionTest VoiceRecForm { get; set; }  //A reference to a specific form.

        public VoiceRecognizer()
        {
            speechRecognizer = new SpeechRecognitionEngine();
        }

        //Starts the voice recognition process.
        public void StartVoiceRecognition()
        {
            //Adds an event handler for when a phrase is recognized.
            //The method inside the Form class will be called, so the form can 
            //interpret commands independently from the voice recognizer.
            speechRecognizer.SpeechRecognized += VoiceRecForm.SpeechRecognizer_SpeechRecognized;

            speechRecognizer.SetInputToDefaultAudioDevice(); //Sets the input of the recognizer to the microphone (if it exists).
            speechRecognizer.RecognizeAsync(RecognizeMode.Multiple); //Recognizes speech one statement at a time, asynchronously.

        }

        public void StopVoiceRecognition()
        {
            //Stop recognizing phrases.
            speechRecognizer.RecognizeAsyncStop();
            //Remove the recognition event handler.
            speechRecognizer.SpeechRecognized -= VoiceRecForm.SpeechRecognizer_SpeechRecognized;
        }

        //Adds a new grammar to the speech recognizer engine.
        public void AddCommands(string [] commands)
        {
            //Adds a string of phrases to be distinguished by the voice recognizer.
            Choices commandChoices = new Choices();
            commandChoices.Add(commands);

            GrammarBuilder grammarBuilder = new GrammarBuilder();
            grammarBuilder.Append(commandChoices);
            Grammar newGrammar = new Grammar(grammarBuilder);

            speechRecognizer.LoadGrammarAsync(newGrammar); //Loads the phrases asynchronously.
        }
    }
}
