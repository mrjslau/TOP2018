
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Speech;
using Android.Speech.Tts;
using Java.Util;
using Android.Runtime;

namespace ShopLens.Droid
{
    [Activity(Label = "SpeechActivity")]
    public class SpeechActivity : Activity, TextToSpeech.IOnInitListener
    {
        bool isRecording;
        readonly int REQUEST_VOICE = 10;

        TextView textBox;
        Button recButton;
        Button recAndVoiceButton;

        TextToSpeech textVoicer;
        private bool voicerIsEnabled;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set the recording and voicer flags to false.
            isRecording = false;
            voicerIsEnabled = false;

            textVoicer = new TextToSpeech(this, this);

            // Set our view from the "main" layout resource.
            SetContentView(Resource.Layout.Speech);

            // Get the resources from the layout.
            recButton = FindViewById<Button>(Resource.Id.btnRecord);
            recAndVoiceButton = FindViewById<Button>(Resource.Id.btnRecordAndVoice);
            textBox = FindViewById<TextView>(Resource.Id.textYourText);

            // Check to see if we can actually record - if we can, assign the event to the button.
            string rec = Android.Content.PM.PackageManager.FeatureMicrophone;
            if (rec != "android.hardware.microphone")
            {
                // No microphone, no recording. Disable the button and output an alert.
                var alert = new AlertDialog.Builder(recButton.Context);
                alert.SetTitle("You don't seem to have a microphone to record with");
                alert.SetPositiveButton("OK", (sender, e) =>
                {
                    textBox.Text = "No microphone present";
                    recButton.Enabled = false;
                    recAndVoiceButton.Enabled = false;
                    return;
                });

                alert.Show();
            }
            else
            {
                recButton.Click += (sender, e) =>
                {
                    DisableVoicer();
                    RecordVoice(recButton);
                };

                recAndVoiceButton.Click += (sender, e) =>
                {
                    EnableVoicer();
                    RecordVoice(recAndVoiceButton); 
                };
            }
        }

        // Is called to verify that the TTS engine has been initialized successfully on the device.
        public void OnInit([GeneratedEnum] OperationResult status)
        {
            // If initialization was successful.
            if (status == OperationResult.Success)
            {
                textVoicer.SetLanguage(Locale.Us);
            }
        }

        private void EnableVoicer()
        {
            if (!voicerIsEnabled)
            {
                voicerIsEnabled = true;
            }
        }

        private void DisableVoicer()
        {
            if (voicerIsEnabled)
            {
                voicerIsEnabled = false;
            }
        }

        private void RecordVoice(Button recordButton)
        {
            // Change the text on the button.
            recordButton.Text = "End Recording";
            isRecording = !isRecording;
            if (isRecording)
            {
                // Create the intent and start the activity.
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
                StartActivityForResult(voiceIntent, REQUEST_VOICE);
            }
        }

        public void VoiceRecording(string voiceMessage)
        {
            if (!string.IsNullOrEmpty(voiceMessage))
                textVoicer.Speak(voiceMessage, QueueMode.Flush, null, null);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == REQUEST_VOICE)
            {
                if (resultCode == Result.Ok)
                {
                    var matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
                    if (matches.Count != 0)
                    {
                        string textInput = textBox.Text + " " + matches[0];

                        // Limit the output to 500 characters.
                        if (textInput.Length > 500)
                            textInput = textInput.Substring(0, 500);
                        textBox.Text = textInput;

                        if (voicerIsEnabled)
                        {
                            VoiceRecording(matches[0]);
                        }
                    }
                    else
                    {
                        textBox.Text = "No speech was recognised";
                    }
                    // Change the text back on the buttons.
                    recButton.Text = "Start Recording";
                    recAndVoiceButton.Text = "Record And Voice";
                    isRecording = false;
                }
            }

            base.OnActivityResult(requestCode, resultCode, data);
        }
    }
}
