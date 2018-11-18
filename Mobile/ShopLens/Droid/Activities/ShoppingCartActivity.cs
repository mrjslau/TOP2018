using System.Collections.Generic;
using ShopLens.Extensions;
using Android.App;
using Android.OS;
using Android.Widget;
using ShopLens.Droid.Source;
using PCLAppConfig;
using System;
using Android.Speech;
using Android.Content;
using ShopLens.Droid.Helpers;
using Android.Runtime;
using Android.Speech.Tts;
using Java.Util;
using System.Threading;
using System.Text.RegularExpressions;

namespace ShopLens.Droid
{
    [Activity(Label = "ShoppingCartActivity")]
    public class ShoppingCartActivity : Activity, IRecognitionListener, TextToSpeech.IOnInitListener
    {
        readonly string PREFS_NAME = ConfigurationManager.AppSettings["ShopCartPrefs"];

        EditText addItemEditText;
        Button addItemButton;
        Button recogniseVoice;
        ListView listView;
        ActivityPreferences prefs;

        SpeechRecognizer commandRecognizer;
        Intent speechIntent;

        List<string> items;
        ArrayAdapter<string> listAdapter;

        private TextToSpeech tts;
        private readonly string voiceCartCmd = ConfigurationManager.AppSettings["CmdVoiceCart"];

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ShoppingCart);

            commandRecognizer = SpeechRecognizer.CreateSpeechRecognizer(this);
            commandRecognizer.SetRecognitionListener(this);

            prefs = new ActivityPreferences(this, PREFS_NAME);
            items = prefs.GetPreferencesToList();

            tts = new TextToSpeech(this, this);

            listView = FindViewById<ListView>(Resource.Id.ShopCartList);
            addItemButton = FindViewById<Button>(Resource.Id.ShopCartAddItemButton);
            addItemEditText = FindViewById<EditText>(Resource.Id.ShopCartAddItemEditText);
            recogniseVoice = FindViewById<Button>(Resource.Id.ShopCartRecVoice);

            listAdapter = 
                new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItemChecked, items);
            listView.Adapter = listAdapter;
            listView.ChoiceMode = ChoiceMode.Multiple;

            addItemButton.Click += AddTextBoxProductToList;
            recogniseVoice.Click += RecogniseVoice;
        }

        private void RecogniseVoice(object sender, EventArgs e)
        {
            speechIntent = VoiceRecognizerHelper.SetUpVoiceRecognizerIntent();
            commandRecognizer.StartListening(speechIntent);
        }

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
            }
        }

        private void SpeakOut(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                tts.Speak(message, QueueMode.Flush, null, null);

                while (tts.IsSpeaking) { }
            }       
        }

        private void AddTextBoxProductToList(object sender, EventArgs e)
        {
            AddStringToList(addItemEditText.Text);
        }

        private void AddStringToList(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                listAdapter.Add(text);
                prefs.AddString(text);
            }
            listAdapter.NotifyDataSetChanged();
        }

        public void OnResults(Bundle results)
        {
            var matches = results.GetStringArrayList(SpeechRecognizer.ResultsRecognition);
            if (matches.Count > 0)
            {
                Regex addProductRegex = new Regex(@"^" + @ConfigurationManager.AppSettings["CmdAddCartList"] + @"(?:\s\w+)+");

                if (matches[0] == voiceCartCmd)
                {
                    string endMessage = "Voicing of cart complete.";

                    foreach (string item in prefs.GetPreferencesToList())
                    {
                        SpeakOut(item);
                        Thread.Sleep(int.Parse(ConfigurationManager.AppSettings["VoicerPauseTime"]));
                    }

                    SpeakOut(endMessage);
                }
                
                if (addProductRegex.IsMatch(matches[0]))
                {
                    string itemToAdd = ProductNameHelper.GetProductNameFromString(matches[0]).FirstCharToUpper();
                    string endMessage = itemToAdd + " was added to your cart.";

                    AddStringToList(itemToAdd);

                    SpeakOut(endMessage);
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
