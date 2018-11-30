using System;
using Android.App;
using Plugin.SpeechRecognition;

namespace ShopLens.Droid.Helpers
{
    public class ShopLensSpeechRecognizer : ISpeechRecognizer
    {
        public event EventHandler<ShopLensSpeechRecognizedEventArgs> OnPhraseRecognized;

        public ShopLensSpeechRecognizer(Action<object, ShopLensSpeechRecognizedEventArgs> reactToSpeechInput)
        {
            OnPhraseRecognized += (obj, eargs) => reactToSpeechInput(obj, eargs);
        }

        public void RecognizePhrase(Activity activity)
        {
            // According to Java, Voice Recognizer needs to be run on the Main Ui Thread.
            activity.RunOnUiThread(ListenForAPhrase);
        }

        public void ListenForAPhrase()
        {
            CrossSpeechRecognition.Current.ListenUntilPause().Subscribe(phrase =>
            {
                OnPhraseRecognized?.Invoke(this, new ShopLensSpeechRecognizedEventArgs(phrase));
            });
        }
    }

    public class ShopLensSpeechRecognizedEventArgs : EventArgs
    {
        public string Phrase { get; set; }

        public ShopLensSpeechRecognizedEventArgs(string phrase)
        {
            Phrase = phrase;
        }
    }
}