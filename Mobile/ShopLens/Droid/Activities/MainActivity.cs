using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Content.PM;

using PCLAppConfig;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;
using ShopLens.Droid.Helpers;
using Android.Views;
using Android.Speech.Tts;
using Java.Util;
using Android.Preferences;
using Android.Views.Accessibility;
using System;
using ShopLens.Droid.Activities;
using Android.Widget;
using System.Linq;
using ShopLensWeb;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using ShopLens.Droid.Source;

public enum IntentIds
{
    VoiceRequest = 101,
    ImageRequest = 201,
    PickImageRequest = 202,
    PermissionRequest = 501
}

namespace ShopLens.Droid
{
    [Activity(Label = "ShopLens", Icon = "@mipmap/icon", Theme ="@style/ShopLensTheme", 
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity
    {
        SupportToolbar toolbar;
        Android.Support.V7.App.ActionBarDrawerToggle drawerToggle;
        DrawerLayout drawerLayout;
        NavigationView navView;
        CoordinatorLayout rootView;
        GestureDetector gestureDetector;
        GestureListener gestureListener;

        string cmdOpenCamera;
        string cmdOpenCart;
        string cmdOpenList;
        string cmdHelp;
        string cmdRemind;
        string cmdTutorialRequest;
        string cmdTutorialLikeShopLens;
        string cmdTakePhoto;

        ShopLensSpeechRecognizer voiceRecognizer;

        ShopLensTextToSpeech shopLensTts;

        Camera2Fragment camera2Frag;

        public static ShopLensContext shopLensDbContext;

        ISharedPreferences prefs;
        ActivityPreferences voicePrefs;
        bool voiceIsOff;

        bool tutorialRequested = false;
        bool talkBackEnabled;

        string userGuidPrefKey;

        public static bool goingFromCartToList = false;
        public static bool goingFromListToCart = false;
        public static List<string> shoppingSessionItems;

        string needUserAnswerId;
        string needAddToCartAnswerId = "ADD_TO_CART";
        string askUserToRepeat;
        string talkBackEnabledIntentKey;

        public readonly string[] ShopLensPermissions =
        {
            Manifest.Permission.RecordAudio,
            Manifest.Permission.Camera,
            Manifest.Permission.WriteExternalStorage
        };


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ConfigurationManager.Initialise(PCLAppConfig.FileSystemStream.PortableStream.Current);
            DependencyInjection.RegisterInterfaces();

            cmdOpenCamera = ConfigurationManager.AppSettings["CmdOpenCamera"];
            cmdTakePhoto = ConfigurationManager.AppSettings["CmdTakePhoto"];
            cmdOpenCart = ConfigurationManager.AppSettings["CmdOpenCart"];
            cmdOpenList = ConfigurationManager.AppSettings["CmdOpenList"];
            cmdHelp = ConfigurationManager.AppSettings["CmdHelp"];
            cmdRemind = ConfigurationManager.AppSettings["CmdRemind"];
            cmdTutorialRequest = ConfigurationManager.AppSettings["CmdTutorialRequest"];
            cmdTutorialLikeShopLens = ConfigurationManager.AppSettings["CmdTutorialLikeShopLens"];

            userGuidPrefKey = ConfigurationManager.AppSettings["UserGuidPrefKey"];

            shopLensDbContext = ConnectToDatabase();

            prefs = PreferenceManager.GetDefaultSharedPreferences(this);

            talkBackEnabledIntentKey = ConfigurationManager.AppSettings["TalkBackKey"];

            voicePrefs = new ActivityPreferences(this, ConfigurationManager.AppSettings["VoicePrefs"]);
            CheckVoicePrefs();

            talkBackEnabled = IsTalkBackEnabled();

            if (!talkBackEnabled)
            {
                InitiateNoTalkBackMode();
            }

            // Set our view from the "main" layout resource.
            SetContentView(Resource.Layout.Main);
            camera2Frag = Camera2Fragment.NewInstance(this, this);

            if (savedInstanceState == null)
            {
                new Thread(() => { FragmentManager.BeginTransaction().Replace(Resource.Id.container, camera2Frag).Commit(); }
            ).Start();          
            }

            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.DrawerLayout);
            toolbar = FindViewById<SupportToolbar>(Resource.Id.Toolbar);
            navView = FindViewById<NavigationView>(Resource.Id.NavView);
            rootView = FindViewById<CoordinatorLayout>(Resource.Id.root_view);

            drawerToggle = new Android.Support.V7.App.ActionBarDrawerToggle(
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

            gestureListener = new GestureListener();
            gestureListener.LeftEvent += GestureLeft;
            gestureDetector = new GestureDetector(this, gestureListener);

            navView.NavigationItemSelected += (sender, e) =>
            {
                switch (e.MenuItem.ItemId)
                {
                    case Resource.Id.NavItemShoppingCart:
                        StartCartIntent();
                        break;
                    case Resource.Id.NavItemShoppingList:
                        StartListIntent();
                        break;
                }
            };
        }

        protected override void OnStart()
        {
            GenerateUserInfoOnFirstLaunch();
            base.OnStart();
        }

        protected override void OnPause()
        {
            if (IsFinishing)
            {
                var shopSession = GenerateShoppingSession();
                shopLensDbContext.ShoppingSessions.Add(shopSession);
                shopLensDbContext.SaveChanges();

                CloseConnectionToDatabase(shopLensDbContext);
            }

            base.OnPause();
        }

        protected override void OnRestart()
        {
            CheckVoicePrefs();

            if (goingFromCartToList)
            {
                goingFromCartToList = false;
                StartListIntent();
            }
            else if (goingFromListToCart)
            {
                goingFromListToCart = false;
                StartCartIntent();
            }
            else if (!talkBackEnabled && !voiceIsOff)
            {
                var message = ConfigurationManager.AppSettings["MainOnRestartMsg"];
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

        private ShopLensContext ConnectToDatabase()
        {
            var useLocalDb = Convert.ToBoolean(ConfigurationManager.AppSettings["UseLocalDb"]);
            var connectionString = ConfigurationManager.AppSettings["DatabaseSource"];

            var dbContext = new ShopLensContext(connectionString, useLocalDb);
            dbContext.Database.Migrate();

            return dbContext;
        }

        private void CloseConnectionToDatabase(DbContext dbContext)
        {
            dbContext.Dispose();
        }

        private bool IsTalkBackEnabled()
        {
            var accessManager = (AccessibilityManager)GetSystemService(AccessibilityService);
            return accessManager.IsTouchExplorationEnabled;
        }

        private void InitiateNoTalkBackMode()
        {
            needUserAnswerId = ConfigurationManager.AppSettings["AnswerUtteranceId"];
            askUserToRepeat = ConfigurationManager.AppSettings["AskToRepeat"];

            shopLensTts = new ShopLensTextToSpeech(this, TtsSpeakAfterInit, TtsStoppedSpeaking);

            voiceRecognizer = new ShopLensSpeechRecognizer(OnVoiceRecognitionResults);
        }

        private void GenerateUserInfoOnFirstLaunch()
        {
            var firstLaunchPrefKey = ConfigurationManager.AppSettings["FirstTimeLaunchPrefKey"];
            bool firstTime = prefs.GetBoolean(firstLaunchPrefKey, true);

            if (firstTime)
            {
                var newUser = GenerateNewUser();
                shopLensDbContext.Users.Add(newUser);

                prefs.Edit().PutBoolean(firstLaunchPrefKey, false).Commit();
            }
        }

        private User GenerateNewUser()
        {
            var userGuid = Guid.NewGuid();
            var guidPrefKey = ConfigurationManager.AppSettings["UserGuidPrefKey"];
            var minUserAge = int.Parse(ConfigurationManager.AppSettings["MinUserAge"]);
            var maxUserAge = int.Parse(ConfigurationManager.AppSettings["MaxUserAge"]);

            prefs.Edit().PutString(guidPrefKey, userGuid.ToString());

            return ShopLensRandomUserGenerator.GenerateRandomUser(userGuid, minUserAge, maxUserAge);
        }

        private ShoppingSession GenerateShoppingSession()
        {
            var productList = new List<Product>();
            var userGuid = Guid.Parse(prefs.GetString(userGuidPrefKey, ""));

            if (shoppingSessionItems == null)
            {
                return null;
            }
            else
            {
                foreach (string item in shoppingSessionItems)
                {
                    var itemInfo = shopLensDbContext.Products.FirstOrDefault(p => p.Name == item);
                    
                    if (itemInfo != null)
                    {
                        productList.Add(itemInfo);
                    }
                }

                if (productList.Any())
                {
                    return new ShoppingSession { Date = DateTime.Now, Products = productList, UserId = userGuid};  
                }
                else
                {
                    return null;
                }
            }
        }

        private void RunUserTutorial()
        {
            tutorialRequested = true;
            var message = ConfigurationManager.AppSettings["InitialTutorialMsg"];
            shopLensTts.Speak(message, needUserAnswerId);
        }

        private void ContinueUserTutorial()
        {
            tutorialRequested = false;
            var message = ConfigurationManager.AppSettings["FollowUpTutorialMsg"];
            shopLensTts.Speak(message, needUserAnswerId);
        }

        private void TtsSpeakAfterInit(object sender, EventArgs e)
        {
            if (!voiceIsOff)
            {
                var welcomeMsg = ConfigurationManager.AppSettings["WelcomeBackMsg"];
                shopLensTts.Speak(welcomeMsg, needUserAnswerId);
            }
        }

        private void TtsStoppedSpeaking(object sender, UtteranceIdArgs e)
        {
            if (e.UtteranceId == needUserAnswerId)
            {
                voiceRecognizer.RecognizePhrase(this);
            }
        }

        private void OnVoiceRecognitionResults(object sender, ShopLensSpeechRecognizedEventArgs e)
        {
            var results = e.Phrase;

            if (!string.IsNullOrEmpty(results))
            {
                if (!tutorialRequested)
                {
                    if (results == cmdOpenCart)
                    {
                        StartCartIntent();
                    }
                    else if (results == cmdOpenList)
                    {
                        StartListIntent();
                    }
                    else if (results == cmdHelp)
                    {
                        var helpMessage = ConfigurationManager.AppSettings["MainHelpMsg"];
                        shopLensTts.Speak(helpMessage, needUserAnswerId);
                    }
                    else if (results == cmdRemind)
                    {
                        var helpMessage = ConfigurationManager.AppSettings["MainRemindMsg"];
                        shopLensTts.Speak(helpMessage, needUserAnswerId);
                    }
                    else if (results == cmdTutorialRequest)
                    {
                        RunUserTutorial();
                    }
                    else if (results == cmdTakePhoto)
                    {
                        ImageRecognizer.areVoiceCommandsOn = true;
                        ImageRecognizer.mainMenu = this;
                        camera2Frag.LockFocus();
                    }
                    else
                    {
                        shopLensTts.Speak(askUserToRepeat, needUserAnswerId);
                    }
                }
                else if (results == cmdTutorialLikeShopLens)
                {
                    ContinueUserTutorial();
                }
                else
                {
                    shopLensTts.Speak(askUserToRepeat, needUserAnswerId);
                }
            }
        }

        private void StartCartIntent()
        {
            var intentCart = new Intent(this, typeof(ShoppingCartActivity));
            intentCart.PutExtra(talkBackEnabledIntentKey, talkBackEnabled);
            StartActivity(intentCart);
        }

        private void StartListIntent()
        {
            var intentList = new Intent(this, typeof(ShoppingListActivity));
            intentList.PutExtra(talkBackEnabledIntentKey, talkBackEnabled);
            StartActivity(intentList);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            drawerToggle.OnOptionsItemSelected(item);
            return base.OnOptionsItemSelected(item);
        }

        public void WhatWouldUHaveMeDo()
        {
            ImageRecognizer.areVoiceCommandsOn = false;

            var whatDoNextMsg = ConfigurationManager.AppSettings["WhatDoNextMsg"];
            shopLensTts.Speak(whatDoNextMsg, needUserAnswerId);
        }

        private void TurnOffVoice()
        {
            voicePrefs.DeleteAllPreferences();
            voicePrefs.AddString("off");
            voiceIsOff = true;
            tutorialRequested = false;
            shopLensTts.Speak(ConfigurationManager.AppSettings["DisableVoiceControlsMsg"], null);
        }

        private void TurnOnVoice()
        {
            voicePrefs.DeleteAllPreferences();
            voicePrefs.AddString("on");
            voiceIsOff = false;
            shopLensTts.Speak(ConfigurationManager.AppSettings["MainOnVoiceOnMsg"], needUserAnswerId);
        }

        void GestureLeft()
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

