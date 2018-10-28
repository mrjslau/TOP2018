﻿
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
        readonly int VOICE = 10;
        TextView textBox;
        Button recButton;
        Button recAndVoiceButton;

        TextToSpeech textVoicer;
        private static bool voicerIsEnabled;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // set the isRecording flag to false (not recording)
            isRecording = false;
            voicerIsEnabled = false;

            textVoicer = new TextToSpeech(this, this);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Speech);

            // get the resources from the layout
            recButton = FindViewById<Button>(Resource.Id.btnRecord);
            recAndVoiceButton = FindViewById<Button>(Resource.Id.btnRecordAndVoice);
            textBox = FindViewById<TextView>(Resource.Id.textYourText);

            // check to see if we can actually record - if we can, assign the event to the button
            string rec = Android.Content.PM.PackageManager.FeatureMicrophone;
            if (rec != "android.hardware.microphone")
            {
                // no microphone, no recording. Disable the button and output an alert
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

        public void OnInit([GeneratedEnum] OperationResult status)
        {
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
            // change the text on the button
            recordButton.Text = "End Recording";
            isRecording = !isRecording;
            if (isRecording)
            {
                // create the intent and start the activity
                var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
                voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);

                // put a message on the modal dialog
                voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, Application.Context.GetString(Resource.String.messageSpeakNow));

                // if there is more then 1.5s of silence, consider the speech over
                voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1500);
                voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 1500);
                voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);
                voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);

                // you can specify other languages recognised here, for example
                // voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.German);
                // if you wish it to recognise the default Locale language and German
                // if you do use another locale, regional dialects may not be recognised very well

                voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);
                StartActivityForResult(voiceIntent, VOICE);
            }
        }

        public void VoiceRecording(string voiceMessage)
        {
            if (!string.IsNullOrEmpty(voiceMessage))
                textVoicer.Speak(voiceMessage, QueueMode.Flush, null, null);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == VOICE)
            {
                if (resultCode == Result.Ok)
                {
                    var matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
                    if (matches.Count != 0)
                    {
                        string textInput = textBox.Text + " " + matches[0];

                        // limit the output to 500 characters
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
                    // change the text back on the button
                    recButton.Text = "Start Recording";
                    recAndVoiceButton.Text = "Record And Voice";
                    isRecording = false;
                }
            }

            base.OnActivityResult(requestCode, resultCode, data);
        }
    }
}
