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
using Android.Views;
using ShopLens.Droid.Activities;
using ShopLens.Droid.Models;

namespace ShopLens.Droid
{
    [Activity(Label = "ShoppingCartActivity", Theme = "@style/ShopLensTheme")]
    public class ShoppingCartActivity : Activity
    {
        readonly string PREFS_NAME = ConfigurationManager.AppSettings["ShopCartPrefs"];
        readonly string VOICE_PREFS_NAME = ConfigurationManager.AppSettings["VoicePrefs"];

        EditText addItemEditText;
        Button addItemButton;
        ListView listView;
        ActivityPreferences prefs;
        ActivityPreferences voicePrefs;
        bool voiceIsOff;
        Button removeItemButton;
        Button removeAllItemsButton;
        GestureDetector gestureDetector;
        GestureListener gestureListener;

        List<string> items;
        CartItemsAdapter cartItemsAdapter;

        ShopLensSpeechRecognizer voiceRecognizer;
        ShopLensTextToSpeech shopLensTts;

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

            voicePrefs = new ActivityPreferences(this, VOICE_PREFS_NAME);
            CheckVoicePrefs();

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
            removeItemButton = FindViewById<Button>(Resource.Id.ShopCartRemoveItemButton);
            removeAllItemsButton = FindViewById<Button>(Resource.Id.ShopCartDeleteAllButton);

            gestureListener = new GestureListener();
            gestureListener.LeftEvent += GestureLeft;
            gestureDetector = new GestureDetector(this, gestureListener);

            cartItemsAdapter = new CartItemsAdapter(prefs.GetCartItemPreferencesToList());
            listView.Adapter = cartItemsAdapter;
            listView.ChoiceMode = ChoiceMode.Multiple;

            addItemButton.Click += AddTextBoxProductToList;
            removeItemButton.Click += RemoveTextBoxProductFromList;
            removeAllItemsButton.Click += RemoveAllItems;
        }

        protected override void OnRestart()
        {
            CheckVoicePrefs();

            if (!talkBackEnabled && !voiceIsOff)
            {
                var message = ConfigurationManager.AppSettings["CartOnRestartMsg"];
                shopLensTts.Speak(message, needUserAnswerId);
            }

            base.OnRestart();
        }

        protected override void OnPause()
        {
            if (IsFinishing)
            {
                MainActivity.shoppingSessionItems = items;
            }

            base.OnPause();
        }

        protected override void OnStop()
        {
            if (!talkBackEnabled && !voiceIsOff)
            {
                // Stop Tts Speech.
                shopLensTts.Stop();
            }

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

            shopLensTts = new ShopLensTextToSpeech(this, TtsSpeakAfterInit, TtsStoppedSpeaking);
            voiceRecognizer = new ShopLensSpeechRecognizer(OnVoiceRecognitionResults);
        }

        private void TtsSpeakAfterInit(object sender, EventArgs e)
        {
            if (!voiceIsOff)
            {
                var message = ConfigurationManager.AppSettings["CartOnRestartMsg"];
                shopLensTts.Speak(message, needUserAnswerId);
            }
        }

        private void TtsStoppedSpeaking(object sender, UtteranceIdArgs e)
        {
            if (e.UtteranceId == needUserAnswerId)
            {
                voiceRecognizer.RecognizePhrase(this);
            }
        }

        private void AskAfterAction()
        {
            shopLensTts.Speak(afterActionAsk, needUserAnswerId);
        }

        private void SpeakOut(string message, int checkDelay)
        {
            if (!string.IsNullOrEmpty(message))
            {
                shopLensTts.Speak(message, null);

                while (shopLensTts.IsSpeaking)
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
                cartItemsAdapter.AddCartItem(text);
                prefs.AddCartItem(text);
            }
            cartItemsAdapter.NotifyDataSetChanged();
        }

        void RemoveTextBoxProductFromList(object sender, EventArgs e)
        {
            RemoveStringFromList(addItemEditText.Text);
        }

        void RemoveStringFromList(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                cartItemsAdapter.RemoveCartItem(text);
                prefs.RemoveCartItem(text);
            }
            cartItemsAdapter.NotifyDataSetChanged();
        }

        void RemoveAllItems(object sender, EventArgs e)
        {
            prefs.DeleteAllPreferences();
            cartItemsAdapter.Clear();
            cartItemsAdapter.NotifyDataSetChanged();
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
                    shopLensTts.Speak(message, needUserAnswerId);
                }
                else if (results == cmdRemind)
                {
                    var message = ConfigurationManager.AppSettings["CartRemindMsg"];
                    shopLensTts.Speak(message, needUserAnswerId);
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
                    shopLensTts.Speak(askUserToRepeat, needUserAnswerId);
                }
            }
        }

        private void TurnOffVoice()
        {
            voicePrefs.DeleteAllPreferences();
            voicePrefs.AddString("off");
            voiceIsOff = true;
            shopLensTts.Speak(ConfigurationManager.AppSettings["DisableVoiceControlsMsg"], null);
        }

        private void TurnOnVoice()
        {
            voicePrefs.DeleteAllPreferences();
            voicePrefs.AddString("on");
            voiceIsOff = false;
            shopLensTts.Speak(ConfigurationManager.AppSettings["CartOnVoiceOnMsg"], needUserAnswerId);
        }

        private void GestureLeft()
        {
            if (!voiceIsOff)
            {
                if (voiceRecognizer.IsListening)
                    voiceRecognizer.StopListening();
                if (shopLensTts.IsSpeaking)
                    shopLensTts.Stop();
                TurnOffVoice();
            }
            else
            {
                TurnOnVoice();
            }
        }
        public override bool DispatchTouchEvent(MotionEvent ev)
        {
            gestureDetector.OnTouchEvent(ev);
            return base.DispatchTouchEvent(ev);
        }

        private void CheckVoicePrefs()
        {
            if (!voicePrefs.IsEmpty)
            {
                voiceIsOff = voicePrefs.GetPreferencesToList()[0] == "off";
            }
            else
            {
                voiceIsOff = false;
            }
        }
    }
}
