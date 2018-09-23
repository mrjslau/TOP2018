using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Threading;

namespace VoiceRecognition
{
    class VoiceRecognizer
    {
        private SpeechRecognitionEngine speechRecognizer;  //The voice recognizer.


        public VoiceRecognizer()
        {
            speechRecognizer = new SpeechRecognitionEngine();
        }

        //Starts the voice recognition process.
        public void StartVoiceRecognition()
        {
            //Adds an event handler for when a phrase is recognized.
            speechRecognizer.SpeechRecognized += SpeechRecognizer_SpeechRecognized;

            speechRecognizer.SetInputToDefaultAudioDevice(); //Sets the input of the recognizer to the microphone (if it exists).
            speechRecognizer.RecognizeAsync(RecognizeMode.Multiple); //Recognizes speech one statement at a time, asynchronously.

        }

        private void SpeechRecognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Text == "Moo") //Does something
            {

            }
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
