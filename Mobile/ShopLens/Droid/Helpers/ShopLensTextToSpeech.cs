using System;
using Android.Content;
using Android.Runtime;
using Android.Speech.Tts;
using Java.Util;

namespace ShopLens.Droid.Helpers
{
    class ShopLensTextToSpeech : Java.Lang.Object, TextToSpeech.IOnInitListener
    {
        event EventHandler TtsInitialized;

        TextToSpeech tts;
        ShopLensUtteranceProgressListener uttListener;

        public bool IsSpeaking
        {
            get { return tts.IsSpeaking; }
        }

        public ShopLensTextToSpeech(Context context, Action<object, EventArgs> ReactToTtsInit,
            Action<object, UtteranceIdArgs> ReactToEndOfSpeech)
        {
            tts = new TextToSpeech(context, this);
            uttListener = new ShopLensUtteranceProgressListener(ReactToEndOfSpeech);

            tts.SetOnUtteranceProgressListener(uttListener);
            TtsInitialized += (obj, eargs) => ReactToTtsInit(obj, eargs);
        }

        // Tts Engine method called when Tts is initialized.
        public void OnInit([GeneratedEnum] OperationResult status)
        {
            if (status == OperationResult.Error)
            {
                tts.SetLanguage(Locale.Default);
            }
            else if (status == OperationResult.Success)
            {
                tts.SetLanguage(Locale.English);
            }

            TtsInitialized(this, new EventArgs());
        }

        public void Speak(string message, string utteranceId)
        {
            tts.Speak(message, QueueMode.Flush, null, utteranceId);
        }

        public void Stop()
        {
            tts.Stop();
        }
    }
}