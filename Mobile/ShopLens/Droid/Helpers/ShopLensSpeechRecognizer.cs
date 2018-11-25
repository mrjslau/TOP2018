using System;
using Android.App;
using Plugin.SpeechRecognition;

namespace ShopLens.Droid.Helpers
{
    public class ShopLensSpeechRecognizer
    {
        public event EventHandler<ShopLensSpeechRecognizedEventArgs> OnPhraseRecognized;

        public ShopLensSpeechRecognizer(Action<object, ShopLensSpeechRecognizedEventArgs> ReactToSpeechInput)
        {
            OnPhraseRecognized += (obj, eargs) => ReactToSpeechInput(obj, eargs);
        }

        public void RecognizePhrase(Activity activity)
        {
            // According to Java, Voice Recognizer needs to be run on the Main Ui Thread.
            activity.RunOnUiThread(ListenForAPhrase);
        }

        private void ListenForAPhrase()
        {
            CrossSpeechRecognition.Current.ListenUntilPause().Subscribe(phrase =>
            {
                OnPhraseRecognized(this, new ShopLensSpeechRecognizedEventArgs(phrase));
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