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
            if(e.Result.Text == "Moo") //Does something
            {

            }
        }

        //Adds a new grammar to the speech recognizer engine.
        public void AddNewGrammar(string grammarMessage, string grammarName)
        {
            //A Grammar is an object that defines what the speech recognition engine
            //can recognize as meaningful input.
            Grammar newGrammar = new Grammar(new GrammarBuilder(grammarMessage)) //Building a grammar with the specified phrase.
            {
                Name = grammarName  //Giving the grammar a name.
            };  

            speechRecognizer.LoadGrammar(newGrammar);
        }
    }
}
