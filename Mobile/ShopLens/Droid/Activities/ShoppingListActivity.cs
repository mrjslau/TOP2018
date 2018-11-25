using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Speech;
using Android.Speech.Tts;
using Android.Widget;
using Java.Util;
using PCLAppConfig;
using ShopLens.Droid.Helpers;
using ShopLens.Droid.Source;
using ShopLens.Extensions;

namespace ShopLens.Droid
{
    [Activity(Label = "ShoppingListActivity", Theme = "@style/ShopLensTheme")]
    public class ShoppingListActivity : Activity, IRecognitionListener, TextToSpeech.IOnInitListener
    {
        readonly string PREFS_NAME = ConfigurationManager.AppSettings["ShopListPrefs"];

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
        private readonly string voiceListCmd = ConfigurationManager.AppSettings["CmdVoiceList"];

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ShoppingList);

            commandRecognizer = SpeechRecognizer.CreateSpeechRecognizer(this);
            commandRecognizer.SetRecognitionListener(this);

            tts = new TextToSpeech(this, this);

            listView = FindViewById<ListView>(Resource.Id.ShopListListView);
            addItemButton = FindViewById<Button>(Resource.Id.ShopListAddItemButton);
            addItemEditText = FindViewById<EditText>(Resource.Id.ShopListAddItemEditText);
            recogniseVoice = FindViewById<Button>(Resource.Id.ShopListRecVoice);

            prefs = new ActivityPreferences(this, PREFS_NAME);
            items = prefs.GetPreferencesToList();

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

        private void SpeakOut(string message, int checkDelay)
        {                
            if (!string.IsNullOrEmpty(message))
            {
                tts.Speak(message, QueueMode.Flush, null, null);

                while (tts.IsSpeaking)
                {
                    Thread.Sleep(checkDelay);
                }
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
            var recognitionResults = results.GetStringArrayList(SpeechRecognizer.ResultsRecognition)[0];
            if (!string.IsNullOrEmpty(recognitionResults))
            {
                string cmdAddProduct = ConfigurationManager.AppSettings["CmdAddCartList"];
                int sessionCheckDelay = int.Parse(ConfigurationManager.AppSettings["VoicerCheckDelay"]);
                Regex addProductRegex = new Regex(@"^" + cmdAddProduct + @"(?:\s\w+)+");

                if (recognitionResults == voiceListCmd)
                {
                    int voicerAwaitTime = int.Parse(ConfigurationManager.AppSettings["VoicerPauseTime"]);
                    string endMessage = "Voicing of shopping list complete.";

                    Task.Run(() =>
                    {
                        foreach (string item in items)
                        {
                            SpeakOut(item, sessionCheckDelay);
                            Thread.Sleep(voicerAwaitTime);
                        }

                        SpeakOut(endMessage, sessionCheckDelay);

                    }).ContinueWith((t) =>
                    {
                        if (t.IsFaulted)
                        {
                            System.Diagnostics.Debug.WriteLine(t.Exception);
                        }
                    }); 
                }

                if (addProductRegex.IsMatch(recognitionResults))
                {
                    addItemButton.Enabled = false;
                    string itemToAdd = recognitionResults.Substring(cmdAddProduct.Length + 1).FirstCharToUpper();
                    string endMessage = itemToAdd + " was added to your shopping list.";

                    AddStringToList(itemToAdd);

                    Task.Run(() =>
                    { 
                        SpeakOut(endMessage, sessionCheckDelay);

                    }).ContinueWith((t) =>
                    {
                        if (t.IsFaulted)
                        {
                            System.Diagnostics.Debug.WriteLine(t.Exception);
                        }
                        addItemButton.Enabled = true;
                    }, TaskScheduler.FromCurrentSynchronizationContext());
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
