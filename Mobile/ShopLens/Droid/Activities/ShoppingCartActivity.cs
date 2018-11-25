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
using System.Linq;
using System.Threading.Tasks;

namespace ShopLens.Droid
{
    [Activity(Label = "ShoppingCartActivity", Theme = "@style/ShopLensTheme")]
    public class ShoppingCartActivity : Activity, TextToSpeech.IOnInitListener
    {
        readonly string PREFS_NAME = ConfigurationManager.AppSettings["ShopCartPrefs"];

        EditText addItemEditText;
        Button addItemButton;
        Button recogniseVoice;
        ListView listView;
        ActivityPreferences prefs;

        List<string> items;
        ArrayAdapter<string> listAdapter;

        private TextToSpeech tts;
        private readonly string voiceCartCmd = ConfigurationManager.AppSettings["CmdVoiceCart"];

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ShoppingCart);

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
        }

        public void OnInit([GeneratedEnum] OperationResult status)
        {
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

                if (recognitionResults == voiceCartCmd)
                {
                    int voicerAwaitTime = int.Parse(ConfigurationManager.AppSettings["VoicerPauseTime"]);
                    string endMessage = "Voicing of shopping cart complete.";

                    Task.Run(() =>
                    {
                        foreach (string item in prefs.GetPreferencesToList())
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
                    string endMessage = itemToAdd + " was added to your shopping cart.";

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
    }
}
