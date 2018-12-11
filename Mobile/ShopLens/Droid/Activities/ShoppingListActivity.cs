using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using PCLAppConfig;
using ShopLens.Droid.Activities;
using ShopLens.Droid.Helpers;
using ShopLens.Droid.Source;
using ShopLens.Extensions;
using ActionBarDrawerToggle = Android.Support.V7.App.ActionBarDrawerToggle;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;

namespace ShopLens.Droid
{
    [Activity(Label = "Shopping List", Theme = "@style/ShopLensTheme")]
    public class ShoppingListActivity : AppCompatActivity
    {
        readonly string PREFS_NAME = ConfigurationManager.AppSettings["ShopListPrefs"];
        readonly string VOICE_PREFS_NAME = ConfigurationManager.AppSettings["VoicePrefs"];

        SupportToolbar toolbar;
        ActionBarDrawerToggle drawerToggle;
        DrawerLayout drawerLayout;
        NavigationView navView;
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
        ArrayAdapter<string> listAdapter;

        ShopLensSpeechRecognizer voiceRecognizer;
        ShopLensTextToSpeech shopLensTts;

        bool talkBackEnabled;

        string needUserAnswerId;
        string askUserToRepeat;
        string afterActionAsk;
        string cmdOpenMain;
        string cmdOpenCart;
        string cmdVoiceList;
        string cmdHelp;
        string cmdRemind;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Window.SetSoftInputMode(SoftInput.StateHidden);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ShoppingList);

            voicePrefs = new ActivityPreferences(this, VOICE_PREFS_NAME);
            CheckVoicePrefs();

            talkBackEnabled = IsTalkBackEnabled();

            if (!talkBackEnabled)
            {
                InitiateNoTalkBackMode();
            }

            listView = FindViewById<ListView>(Resource.Id.ShopListListView);
            addItemButton = FindViewById<Button>(Resource.Id.ShopListAddItemButton);
            addItemEditText = FindViewById<EditText>(Resource.Id.ShopListAddItemEditText);
            removeItemButton = FindViewById<Button>(Resource.Id.ShopListRemoveItemButton);
            removeAllItemsButton = FindViewById<Button>(Resource.Id.ShopListDeleteAllButton);

            prefs = new ActivityPreferences(this, PREFS_NAME);
            items = prefs.GetPreferencesToList();

            gestureListener = new GestureListener();
            gestureListener.LeftEvent += GestureLeft;
            gestureDetector = new GestureDetector(this, gestureListener);

            listAdapter =
                new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItemChecked, items);
            listView.Adapter = listAdapter;
            listView.ChoiceMode = ChoiceMode.Multiple;

            addItemButton.Click += AddTextBoxProductToList;
            removeItemButton.Click += RemoveTextBoxProductFromList;
            removeAllItemsButton.Click += RemoveAllItems;

            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.DrawerLayout);
            toolbar = FindViewById<SupportToolbar>(Resource.Id.Toolbar);
            navView = FindViewById<NavigationView>(Resource.Id.NavView);

            drawerToggle = new ActionBarDrawerToggle(
                this,
                drawerLayout,
                Resource.String.openDrawer,
                Resource.String.closeDrawer
            );
            drawerLayout.AddDrawerListener(drawerToggle);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);
            drawerToggle.SyncState();

            navView.NavigationItemSelected += (sender, e) =>
            {
                switch (e.MenuItem.ItemId)
                {
                    case Resource.Id.NavItemShoppingCart:
                        GoToShoppingCart();
                        break;
                    case Resource.Id.NavItemShoppingList:
                        OnOptionsItemSelected(e.MenuItem);
                        break;
                }
            };
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            drawerToggle.OnOptionsItemSelected(item);
            return base.OnOptionsItemSelected(item);
        }

        protected override void OnRestart()
        {
            CheckVoicePrefs();

            if (!talkBackEnabled && !voiceIsOff)
            {
                var message = ConfigurationManager.AppSettings["ListOnRestartMsg"];
                shopLensTts.Speak(message, needUserAnswerId);
            }

            base.OnRestart();
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
            cmdOpenCart = ConfigurationManager.AppSettings["CmdOpenCart"];
            cmdVoiceList = ConfigurationManager.AppSettings["CmdVoiceCartList"];
            cmdHelp = ConfigurationManager.AppSettings["CmdHelp"];
            cmdRemind = ConfigurationManager.AppSettings["CmdRemind"];

            shopLensTts = new ShopLensTextToSpeech(this, TtsSpeakAfterInit, TtsStoppedSpeaking);
            voiceRecognizer = new ShopLensSpeechRecognizer(OnVoiceRecognitionResults);
        }

        private void TtsSpeakAfterInit(object sender, EventArgs e)
        {
            if (!voiceIsOff)
            {
                var message = ConfigurationManager.AppSettings["ListOnRestartMsg"];
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

        private void AddTextBoxProductToList(object sender, EventArgs e)
        {
            AddStringToList(addItemEditText.Text);
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

        private void AddStringToList(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                listAdapter.Add(text);
                prefs.AddString(text);
            }
            listAdapter.NotifyDataSetChanged();
        }

        private void GoToShoppingCart()
        {
            MainActivity.goingFromListToCart = true;
            Finish();
        }

        void RemoveTextBoxProductFromList(object sender, EventArgs e)
        {
            RemoveStringFromList(addItemEditText.Text);
        }

        void RemoveStringFromList(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                listAdapter.Remove(text);
                prefs.RemoveString(text);
            }
            listAdapter.NotifyDataSetChanged();
        }

        void RemoveAllItems(object sender, EventArgs e)
        {
            prefs.DeleteAllPreferences();
            listAdapter.Clear();
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

                if (results == cmdVoiceList)
                {
                    int voicerAwaitTime = int.Parse(ConfigurationManager.AppSettings["VoicerPauseTime"]);
                    string endMessage = ConfigurationManager.AppSettings["ListVoicingCompleteMsg"];

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
                    string endMessage = itemToAdd + ConfigurationManager.AppSettings["AddedToListMsg"];

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
                    var message = ConfigurationManager.AppSettings["ListHelpMsg"];
                    shopLensTts.Speak(message, needUserAnswerId);
                }
                else if (results == cmdRemind)
                {
                    var message = ConfigurationManager.AppSettings["ListRemindMsg"];
                    shopLensTts.Speak(message, needUserAnswerId);
                }
                else if (results == cmdOpenMain)
                {
                    Finish();
                }
                else if (results == cmdOpenCart)
                {
                    GoToShoppingCart();
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
            shopLensTts.Speak(ConfigurationManager.AppSettings["ListOnVoiceOnMsg"], needUserAnswerId);
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
