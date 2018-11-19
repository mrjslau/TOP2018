using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;

using Android.Speech.Tts;
using Android.Support.V7.App;
using Java.Util;

namespace ShopLens.Droid
{
    [Activity(Label = "TextVoicerActivity", Theme = "@style/ShopLensTheme")]
    public class TextVoicerActivity : AppCompatActivity, TextToSpeech.IOnInitListener
    {
        EditText editTextToVoice;
        Button textToVoiceButton;
        TextToSpeech tts;

        public void OnInit([GeneratedEnum] OperationResult status)
        {
            // If we get an error, use the default language.
            if (status == OperationResult.Error)
            {
                tts.SetLanguage(Locale.Default);
            }
            else if (status == OperationResult.Success)
            {
                tts.SetLanguage(Locale.Us);
                SpeakOut();
            }
        }

        void SpeakOut()
        {
            string text = editTextToVoice.Text;
            if (!string.IsNullOrEmpty(text))
                tts.Speak(text, QueueMode.Flush, null, null);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here.
            SetContentView(Resource.Layout.TextVoicer);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "Text To Speech";

            editTextToVoice = FindViewById<EditText>(Resource.Id.editTextToVoice);
            textToVoiceButton = FindViewById<Button>(Resource.Id.TextToVoiceButton);
            tts = new TextToSpeech(this, this);

            textToVoiceButton.Click += delegate {
                SpeakOut();
            };
        }
    }
}
    