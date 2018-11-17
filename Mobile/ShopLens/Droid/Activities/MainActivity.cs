using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using PCLAppConfig;
using Android.Speech;
using System;

using ShopLens.Droid.Helpers;
using Android.Runtime;

public enum ActivityIds
{
    VoiceRequest = 101,
    ImageRequest = 201,
    PickImageRequest = 202
}

namespace ShopLens.Droid
{
    [Activity(Label = "ShopLens", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity, IRecognitionListener
    {
        Lazy<SpeechRecognizer> commandRecognizer;
        Intent speechIntent;
        private Button voiceCommandButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            ConfigurationManager.Initialise(PCLAppConfig.FileSystemStream.PortableStream.Current);

            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource.
            SetContentView(Resource.Layout.Main);

            commandRecognizer = new Lazy<SpeechRecognizer>(() => SpeechRecognizer.CreateSpeechRecognizer(this));

            if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsVoiceRecognitionEnabled"]))
            {
                commandRecognizer.Value.SetRecognitionListener(this);
            }

            // Get our button from the layout resource,
            // and attach an event to it.
            Button textVoicerButton = FindViewById<Button>(Resource.Id.TextVoicerButton);
            Button cameraButton = FindViewById<Button>(Resource.Id.CameraButton);
            Button speechButton = FindViewById<Button>(Resource.Id.SpeechButton);
            Button shoppingListButton = FindViewById<Button>(Resource.Id.ShoppingListButton);
            Button shoppingCartButton = FindViewById<Button>(Resource.Id.ShoppingCartButton);
            voiceCommandButton = FindViewById<Button>(Resource.Id.MainRecordingButton);

            textVoicerButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(TextVoicerActivity));
                StartActivity(intent);
            };

            cameraButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(CameraActivity));
                StartActivity(intent);
            };

            speechButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(SpeechActivity));
                StartActivity(intent);
            };

            shoppingListButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(ShoppingListActivity));
                StartActivity(intent);
            };

            shoppingCartButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(ShoppingCartActivity));
                StartActivity(intent);
            };

            voiceCommandButton.Click += RecogniseVoice;
        }


        void RecogniseVoice(object sender, EventArgs e)
        {
            if (commandRecognizer.IsValueCreated)
            {
                speechIntent = VoiceRecognizerHelper.SetUpVoiceRecognizerIntent();
                commandRecognizer.Value.StartListening(speechIntent);
            }
        }

        // When the current voice recognition session stops.
        public void OnResults(Bundle results)
        {
            var matches = results.GetStringArrayList(SpeechRecognizer.ResultsRecognition);
            if (matches.Count > 0)
            {
                if (matches[0] == ConfigurationManager.AppSettings["CmdOpenCamera"])
                {
                    var intent = new Intent(this, typeof(CameraActivity));
                    StartActivity(intent);
                }
                else if (matches[0] == ConfigurationManager.AppSettings["CmdOpenCart"])
                {
                    var intent = new Intent(this, typeof(ShoppingCartActivity));
                    StartActivity(intent);
                }
                else if (matches[0] == ConfigurationManager.AppSettings["CmdOpenList"])
                {
                    var intent = new Intent(this, typeof(ShoppingListActivity));
                    StartActivity(intent);
                }
                //debug
                else
                {
                    voiceCommandButton.Text = matches[0];
                }
            }
        }


        #region Unimplemented Speech Recognizer Methods

        // When the user starts to speak.
        public void OnBeginningOfSpeech() { }

        // After the user stops speaking.
        public void OnEndOfSpeech() { }

        // When a network or recognition error occurs.
        public void OnError([GeneratedEnum] SpeechRecognizerError error) { }

        // When the app is ready for the user to start speaking.
        public void OnReadyForSpeech(Bundle @params) { }

        // This method is reserved for adding future events.
        public void OnEvent(int eventType, Bundle @params) { }

        // When more sound has been received.
        public void OnBufferReceived(byte[] buffer) { }

        // When the sound level of the voice input stream has changed.
        public void OnRmsChanged(float rmsdB) { }

        // When partial recognition results are available.
        public void OnPartialResults(Bundle partialResults) { }

        #endregion
    }
}

