using System;
using Android.App;
using Plugin.SpeechRecognition;

namespace ShopLens.Droid.Helpers
{
    public class ShopLensSpeechRecognizer : ISpeechRecognizer
    {
        public bool IsListening
        {
            get { return voiceListener != null; }
        }

        public event EventHandler<ShopLensSpeechRecognizedEventArgs> OnPhraseRecognized;

        IDisposable voiceListener;

        public ShopLensSpeechRecognizer(Action<object, ShopLensSpeechRecognizedEventArgs> reactToSpeechInput)
        {
            OnPhraseRecognized += (obj, eargs) => reactToSpeechInput(obj, eargs);
        }

        public void RecognizePhrase(Activity activity)
        {
            activity.RunOnUiThread(ListenForAPhrase);
        }

        public void ListenForAPhrase()
        {
            voiceListener = CrossSpeechRecognition.Current.ListenUntilPause().Subscribe(phrase =>
            {
                OnPhraseRecognized?.Invoke(this, new ShopLensSpeechRecognizedEventArgs(phrase));
            });
        }

        public void StopListening()
        {
            voiceListener.Dispose();
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