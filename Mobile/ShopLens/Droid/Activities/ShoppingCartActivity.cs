using System.Collections.Generic;
using ShopLens.Extensions;
using Android.App;
using Android.OS;
using Android.Widget;
using ShopLens.Droid.Source;
using PCLAppConfig;
using System;
using Android.Content;
using ShopLens.Droid.Helpers;
using System.Threading;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ShopLens.Droid
{
    [Activity(Label = "ShoppingCartActivity", Theme = "@style/ShopLensTheme")]
    public class ShoppingCartActivity : Activity
    {
        readonly string PREFS_NAME = ConfigurationManager.AppSettings["ShopCartPrefs"];

        EditText addItemEditText;
        Button addItemButton;
        ListView listView;
        ActivityPreferences prefs;

        List<string> items;
        ArrayAdapter<string> listAdapter;

        ShopLensSpeechRecognizer voiceRecognizer;
        ShopLensTextToSpeech shopLensTTS;

        bool talkBackEnabled;

        string needUserAnswerId;
        string askUserToRepeat;
        string afterActionAsk;
        string cmdOpenMain;
        string cmdOpenList;
        string cmdVoiceCart; 
        string cmdHelp;
        string cmdRemind;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ShoppingCart);

            talkBackEnabled = IsTalkBackEnabled();

            if (!talkBackEnabled)
            {
                InitiateNoTalkBackMode();
            }

            prefs = new ActivityPreferences(this, PREFS_NAME);
            items = prefs.GetPreferencesToList();

            listView = FindViewById<ListView>(Resource.Id.ShopCartList);
            addItemButton = FindViewById<Button>(Resource.Id.ShopCartAddItemButton);
            addItemEditText = FindViewById<EditText>(Resource.Id.ShopCartAddItemEditText);

            listAdapter =
                new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItemChecked, items);
            listView.Adapter = listAdapter;
            listView.ChoiceMode = ChoiceMode.Multiple;

            addItemButton.Click += AddTextBoxProductToList;
        }

        protected override void OnRestart()
        {
            if (!talkBackEnabled)
            {
                var message = ConfigurationManager.AppSettings["CartOnRestartMsg"];
                shopLensTTS.Speak(message, needUserAnswerId);
            }

            base.OnRestart();
        }

        protected override void OnStop()
        {
            // Stop TTS Speech.
            shopLensTTS.Stop();

            base.OnStop();
        }

        private bool IsTalkBackEnabled()
        {
            var talkBackEnabledIntentKey = ConfigurationManager.AppSettings["TalkBackKey"];
            return Intent.GetBooleanExtra(talkBackEnabledIntentKey, false);
        }

        private void InitiateNoTalkBackMode()
        {
            needUserAnswerId = ConfigurationManager.AppSettings["AnswerUtteranceId"];
            askUserToRepeat = ConfigurationManager.AppSettings["AskToRepeat"];
            afterActionAsk = ConfigurationManager.AppSettings["AfterActionMsg"];
            cmdOpenMain = ConfigurationManager.AppSettings["CmdOpenMain"];
            cmdOpenList = ConfigurationManager.AppSettings["CmdOpenList"];
            cmdVoiceCart = ConfigurationManager.AppSettings["CmdVoiceCartList"];
            cmdHelp = ConfigurationManager.AppSettings["CmdHelp"];
            cmdRemind = ConfigurationManager.AppSettings["CmdRemind"];

            shopLensTTS = new ShopLensTextToSpeech(this, TTSSpeakAfterInit, TTStoppedSpeaking);
            voiceRecognizer = new ShopLensSpeechRecognizer(OnVoiceRecognitionResults);
        }

        private void TTSSpeakAfterInit(object sender, EventArgs e)
        {
            var message = ConfigurationManager.AppSettings["CartOnRestartMsg"];
            shopLensTTS.Speak(message, needUserAnswerId);
        }

        private void TTStoppedSpeaking(object sender, UtteranceIdArgs e)
        {
            if (e.UtteranceId == needUserAnswerId)
            {
                voiceRecognizer.RecognizePhrase(this);
            }
        }

        private void AskAfterAction()
        {
            shopLensTTS.Speak(afterActionAsk, needUserAnswerId);
        }

        private void SpeakOut(string message, int checkDelay)
        {
            if (!string.IsNullOrEmpty(message))
            {
                shopLensTTS.Speak(message, null);

                while (shopLensTTS.tts.IsSpeaking)
                {
                    // TODO: Tomas needs to fix this.
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

        private void OnVoiceRecognitionResults(object sender, ShopLensSpeechRecognizedEventArgs e)
        {
            var results = e.Phrase;

            if (!string.IsNullOrEmpty(results))
            {
                var cmdAddProduct = ConfigurationManager.AppSettings["CmdAddCartList"];
                int sessionCheckDelay = int.Parse(ConfigurationManager.AppSettings["VoicerCheckDelay"]);
                Regex addProductRegex = new Regex(@"^" + cmdAddProduct + @"(?:\s\w+)+");

                if (results == cmdVoiceCart)
                {
                    int voicerAwaitTime = int.Parse(ConfigurationManager.AppSettings["VoicerPauseTime"]);
                    string endMessage = ConfigurationManager.AppSettings["CartVoicingCompleteMsg"];

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

                        AskAfterAction();
                    }, TaskScheduler.FromCurrentSynchronizationContext());
                }
                else if (addProductRegex.IsMatch(results))
                {
                    addItemButton.Enabled = false;
                    string itemToAdd = results.Substring(cmdAddProduct.Length + 1).FirstCharToUpper();
                    string endMessage = itemToAdd + ConfigurationManager.AppSettings["AddedToCartMsg"];

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
                        AskAfterAction();
                    }, TaskScheduler.FromCurrentSynchronizationContext());
                }
                else if (results == cmdHelp)
                {
                    var message = ConfigurationManager.AppSettings["CartHelpMsg"];
                    shopLensTTS.Speak(message, needUserAnswerId);
                }
                else if (results == cmdRemind)
                {
                    var message = ConfigurationManager.AppSettings["CartRemindMsg"];
                    shopLensTTS.Speak(message, needUserAnswerId);
                }
                else if (results == cmdOpenMain)
                {
                    Finish();
                }
                else if (results == cmdOpenList)
                {
                    MainActivity.goingFromCartToList = true;
                    Finish();
                }
                else
                {
                    shopLensTTS.Speak(askUserToRepeat, needUserAnswerId);
                }
            }
        }
    }
}
