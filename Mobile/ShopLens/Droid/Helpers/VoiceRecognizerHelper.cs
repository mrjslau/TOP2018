using Android.App;
using Android.Content;
using Android.Speech;
using Java.Util;

namespace ShopLens.Droid.Helpers
{
    public static class VoiceRecognizerHelper
    {
        public static Intent SetUpVoiceRecognizerIntent()
        {
            var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);

            // If there is more than 1.5s of silence, consider the speech over.
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1500);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 1500);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);

            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Locale.Default);

            return voiceIntent;
        }
    }
}