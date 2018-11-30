using System;
using Android.Content;
using Android.Runtime;
using Android.Speech.Tts;
using Java.Util;

namespace ShopLens.Droid.Helpers
{
    class ShopLensTextToSpeech : Java.Lang.Object, TextToSpeech.IOnInitListener
    {
        event EventHandler TTSInitialized;
        ShopLensUtteranceProgressListener uttListener;

        public TextToSpeech tts;

        public ShopLensTextToSpeech(Context context, Action<object, EventArgs> ReactToTTSInit,
            Action<object, UtteranceIdArgs> ReactToEndOfSpeech)
        {
            tts = new TextToSpeech(context, this);
            uttListener = new ShopLensUtteranceProgressListener(ReactToEndOfSpeech);

            tts.SetOnUtteranceProgressListener(uttListener);
            TTSInitialized += (obj, eargs) => ReactToTTSInit(obj, eargs);
        }

        // TTS Engine method called when TTS is initialized.
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

            TTSInitialized(this, new EventArgs());
        }

        public void Speak(string message, string utteranceId)
        {
            tts?.Speak(message, QueueMode.Flush, null, utteranceId);
        }

        public void Stop()
        {
            tts?.Stop();
        }
    }
}