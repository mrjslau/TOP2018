
using Android.App;
using Android.Content;
using Android.Speech;
using Java.Util;

namespace ShopLens.Droid.Source
{
    public class Recording
    {

        public Intent CreateRecordingIntent()
        {
            var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);

            // Put a message on the modal dialog.
            voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, Application.Context.GetString(Resource.String.messageSpeakNow));

            // If there is more then 1.5s of silence, consider the speech over.
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1500);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 1500);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);

            // You can specify other languages recognised here, for example:
            // voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Locale.German).

            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Locale.Default);

            return voiceIntent;
        }

        public bool CanRecord()
        {
            string rec = Android.Content.PM.PackageManager.FeatureMicrophone;
            return rec != "android.hardware.microphone" ? true : false;
        }
    }
}
