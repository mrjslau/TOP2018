using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Content.PM;
using PCLAppConfig;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;
using ShopLens.Droid.Helpers;
using Android.Views;
using Android.Preferences;
using Android.Views.Accessibility;
using System;
using System.Linq;
using RandomNameGenerator;
using ShopLensWeb;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

public enum IntentIds
{
    VoiceRequest = 101,
    ImageRequest = 201,
    PickImageRequest = 202,
    PermissionRequest = 501
}

namespace ShopLens.Droid
{
    [Activity(Label = "ShopLens", MainLauncher = true, Icon = "@mipmap/icon", Theme = "@style/ShopLensTheme")]
    public class MainActivity : AppCompatActivity
    {
        SupportToolbar toolbar;
        ActionBarDrawerToggle drawerToggle;
        DrawerLayout drawerLayout;
        NavigationView navView;
        CoordinatorLayout rootView;

        ShopLensSpeechRecognizer voiceRecognizer;

        ShopLensTextToSpeech shopLensTts;

        ShopLensContext shopLensDbContext;

        User userInfo;

        ISharedPreferences prefs;

        bool tutorialNeeded;
        bool talkBackEnabled;

        string needUserAnswerId;
        string askUserToRepeat;
        string talkBackEnabledIntentKey;

        string userGuidPrefKey;

        string cmdOpenCamera;
        string cmdOpenCart;
        string cmdOpenList;
        string cmdHelp;
        string cmdRemind;
        string cmdTutorialLikeShopLens;

        public static bool goingFromCartToList = false;
        public static bool goingFromListToCart = false;
        public static List<string> shoppingSessionItems;

        readonly string[] ShopLensPermissions =
        {
            Manifest.Permission.RecordAudio,
            Manifest.Permission.Camera,
            Manifest.Permission.WriteExternalStorage
        };

        readonly int REQUEST_PERMISSION = (int)IntentIds.PermissionRequest;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ConfigurationManager.Initialise(PCLAppConfig.FileSystemStream.PortableStream.Current);
            DependencyInjection.RegisterInterfaces();

            cmdOpenCamera = ConfigurationManager.AppSettings["CmdOpenCamera"];
            cmdOpenCart = ConfigurationManager.AppSettings["CmdOpenCart"];
            cmdOpenList = ConfigurationManager.AppSettings["CmdOpenList"];
            cmdHelp = ConfigurationManager.AppSettings["CmdHelp"];
            cmdRemind = ConfigurationManager.AppSettings["CmdRemind"];
            cmdTutorialLikeShopLens = ConfigurationManager.AppSettings["CmdTutorialLikeShopLens"];

            userGuidPrefKey = ConfigurationManager.AppSettings["UserGuidPrefKey"];

            prefs = PreferenceManager.GetDefaultSharedPreferences(this);

            ConnectToDatabase();

            talkBackEnabledIntentKey = ConfigurationManager.AppSettings["TalkBackKey"];

            talkBackEnabled = IsTalkBackEnabled();

            if (!talkBackEnabled)
            {
               InitiateNoTalkBackMode();
            }

            // Set our view from the "main" layout resource.
            SetContentView(Resource.Layout.Main);

            // We need to request user permissions.
            if ((int) Build.VERSION.SdkInt >= (int) BuildVersionCodes.M)
            {
                RequestPermissions(ShopLensPermissions, REQUEST_PERMISSION);
            }

            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.DrawerLayout);
            toolbar = FindViewById<SupportToolbar>(Resource.Id.Toolbar);
            navView = FindViewById<NavigationView>(Resource.Id.NavView);
            rootView = FindViewById<CoordinatorLayout>(Resource.Id.root_view);

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
                    case Resource.Id.NavItemCamera:
                        StartCameraIntent();
                        break;
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
            tutorialNeeded = IsTutorialNeeded();

            base.OnStart();
        }

        protected override void OnPause()
        {
            if (IsFinishing)
            {
                InsertShoppingSessionInfo();
            }

            base.OnPause();
        }

        protected override void OnRestart()
        {
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
            else if (!talkBackEnabled)
            {
                var message = ConfigurationManager.AppSettings["MainOnRestartMsg"];
                shopLensTts.Speak(message, needUserAnswerId);
            }

            base.OnRestart();
        }

        protected override void OnStop()
        {
            // Stop Tts Speech.
            shopLensTts.Stop();

            base.OnStop();
        }

        private void ConnectToDatabase()
        {
            var useLocalDb = Convert.ToBoolean(ConfigurationManager.AppSettings["UseLocalDb"]);
            var connectionString = ConfigurationManager.AppSettings["DatabaseSource"];

            shopLensDbContext = new ShopLensContext(connectionString, useLocalDb);
            shopLensDbContext.Database.Migrate();
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

        private bool IsTutorialNeeded()
        {
            var firstLaunchPrefKey = ConfigurationManager.AppSettings["FirstTimeLaunchPrefKey"];
            bool firstTime = prefs.GetBoolean(firstLaunchPrefKey, true);

            if (firstTime)
            {
                prefs.Edit().PutBoolean(firstLaunchPrefKey, false).Commit();
                GenerateUserInfo();
                return true;
            }
            else return false;
        }

        private void GenerateUserInfo()
        {
            var userId = Guid.NewGuid();

            prefs.Edit().PutString(userGuidPrefKey, userId.ToString());

            var randomNumberGenerator = new Random();

            var randomYear = randomNumberGenerator.Next(DateTime.Now.Year - 100, DateTime.Now.Year - 14);
            var randomMonth = randomNumberGenerator.Next(1, 13);
            var randomDay = randomNumberGenerator.Next(1, DateTime.DaysInMonth(randomYear, randomMonth) + 1);

            var randomBirthday = new DateTime(randomYear, randomMonth, randomDay);

            Gender userGender;
            int genderChance = randomNumberGenerator.Next(1, 101);

            if (genderChance >= 50) userGender = Gender.Female;

            else userGender = Gender.Male;

            var randomName = NameGenerator.GenerateFirstName(userGender);

            userInfo = new User {Birthday = randomBirthday, Name = randomName, UserId = userId.ToString()};

            shopLensDbContext.Users.Add(userInfo);
            shopLensDbContext.SaveChanges();
        }

        private void InsertShoppingSessionInfo()
        {
            var productList = new List<Product>();
            var userId = prefs.GetString(userGuidPrefKey, null);
            
            userInfo = shopLensDbContext.Users.FirstOrDefault(u => u.UserId == userId);

            if (shoppingSessionItems != null)
            {
                foreach (string item in shoppingSessionItems)
                {
                    var itemInfo = shopLensDbContext.Products.FirstOrDefault(p => p.Name == item);

                    if (itemInfo != null) productList.Add(itemInfo);
                }

                if (!productList.Any())
                {
                    var shoppingSession = new ShoppingSession { Date = DateTime.Now, Products = productList, User = userInfo };
                    userInfo.ShoppingSessions.Add(shoppingSession);

                    shopLensDbContext.SaveChanges();
                }
            }

            shopLensDbContext.Dispose();
        }

        private void RunUserTutorial()
        {
            var message = ConfigurationManager.AppSettings["InitialTutorialMsg"];
            shopLensTts.Speak(message, needUserAnswerId);
        }

        private void ContinueUserTutorial()
        {
            var message = ConfigurationManager.AppSettings["FollowUpTutorialMsg"];
            shopLensTts.Speak(message, needUserAnswerId);
        }

        private void TtsSpeakAfterInit(object sender, EventArgs e)
        {
            if (tutorialNeeded)
            {
                RunUserTutorial();
            }
            else
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
                if (results == cmdOpenCamera)
                {
                    StartCameraIntent();
                }
                else if (results == cmdOpenCart)
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
                else if (results == cmdTutorialLikeShopLens && tutorialNeeded)
                {
                    ContinueUserTutorial();
                }
                else
                {
                    shopLensTts.Speak(askUserToRepeat, needUserAnswerId);
                }
            }
        }

        private void StartCameraIntent()
        {
            var intentCam = new Intent(this, typeof(CameraActivity));
            StartActivity(intentCam);
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

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            if (requestCode == REQUEST_PERMISSION)
            {
                foreach (Permission appPermission in grantResults)
                {
                    if (appPermission == Permission.Denied)
                    {
                        RequestPermissions(ShopLensPermissions, REQUEST_PERMISSION);
                    }
                }
            }
        }
    }
}

