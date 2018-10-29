using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;

using Android.Speech.Tts;
using Android.Support.V7.App;
using Java.Util;

namespace ShopLens.Droid
{
    [Activity(Label = "TextVoicerActivity", Theme = "@style/MyTheme")]
    public class TextVoicerActivity : AppCompatActivity, TextToSpeech.IOnInitListener
    {
        private EditText editTextToVoice;
        private Button textToVoiceButton;
        private TextToSpeech tts;

        public void OnInit([GeneratedEnum] OperationResult status)
        {
            if(status == OperationResult.Success)
            {
                tts.SetLanguage(Locale.Us);
                SpeakOut();
            }
        }

        private void SpeakOut()
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
    