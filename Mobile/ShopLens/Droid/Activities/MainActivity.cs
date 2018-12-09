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
using System.Threading;

public enum IntentIds
{
    VoiceRequest = 101,
    ImageRequest = 201,
    PickImageRequest = 202,
    PermissionRequest = 501
}

namespace ShopLens.Droid
{
    [Activity(Label = "ShopLens", MainLauncher = true, Icon = "@mipmap/icon", Theme ="@style/ShopLensTheme", 
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity, TextToSpeech.IOnInitListener
    {
        SupportToolbar toolbar;
        ActionBarDrawerToggle drawerToggle;
        DrawerLayout drawerLayout;
        NavigationView navView;
        CoordinatorLayout rootView;

        ShopLensSpeechRecognizer voiceRecognizer;

        TextToSpeech tts;
        ShopLensUtteranceProgressListener ttsListener;

        ISharedPreferences prefs;

        bool tutorialNeeded;
        bool talkBackEnabled;

        string needUserAnswerId;
        string askUserToRepeat;
        string talkBackEnabledIntentKey;

        public readonly string[] ShopLensPermissions =
        {
            Manifest.Permission.RecordAudio,
            Manifest.Permission.Camera,
            Manifest.Permission.WriteExternalStorage
        };


        static readonly int REQUEST_PERMISSION = (int) IntentIds.PermissionRequest;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ConfigurationManager.Initialise(PCLAppConfig.FileSystemStream.PortableStream.Current);
            DependencyInjection.RegisterInterfaces();

            prefs = PreferenceManager.GetDefaultSharedPreferences(this);

            talkBackEnabledIntentKey = ConfigurationManager.AppSettings["TalkBackKey"];

            // Check if TalkBack is enabled.
            if (IsTalkBackEnabled())
            {
                talkBackEnabled = true;
            }
            else
            {
                talkBackEnabled = false;
                InitiateNoTalkBackMode();
            }

            // Set our view from the "main" layout resource.
            SetContentView(Resource.Layout.Main);            

            // We need to request user permissions.
            if ((int) Build.VERSION.SdkInt >= (int) BuildVersionCodes.M)
            {
                RequestPermissions(ShopLensPermissions, REQUEST_PERMISSION);
            }

            if (savedInstanceState == null)
            {
                new Thread(() => { FragmentManager.BeginTransaction().Replace(Resource.Id.container, Camera2Fragment.NewInstance(this, this)).Commit(); }
            ).Start();          
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
            base.OnStart();
            tutorialNeeded = IsTutorialNeeded();
        }

        protected override void OnStop()
        {
            // Get rid of TTS.
            if (tts != null)
            {
                //TODO: possibly resume TTS in the OnResume method.
                tts.Stop();
                tts.Shutdown();
            }
            base.OnStop();
        }

        //TODO: make it so that once we return to the main menu, the voicer greets us and asks questions (OnResume method).

        private bool IsTalkBackEnabled()
        {
            var accessManager = (AccessibilityManager)GetSystemService(AccessibilityService);
            return accessManager.IsTouchExplorationEnabled;
        }

        private void InitiateNoTalkBackMode()
        {
            needUserAnswerId = ConfigurationManager.AppSettings["AnswerUtteranceId"];
            askUserToRepeat = ConfigurationManager.AppSettings["AskToRepeat"];

            ttsListener = new ShopLensUtteranceProgressListener(TtsStoppedSpeaking);

            tts = new TextToSpeech(this, this);
            tts.SetOnUtteranceProgressListener(ttsListener);

            voiceRecognizer = new ShopLensSpeechRecognizer(OnVoiceRecognitionResults);
        }

        private bool IsTutorialNeeded()
        {
            bool firstTime = prefs.GetBoolean("FirstTime", true);

            if (firstTime)
            {
                prefs.Edit().PutBoolean("FirstTime", false).Commit();
                return true;
            }
            else return false;
        }

        private void RunUserTutorial()
        {
            var message = ConfigurationManager.AppSettings["InitialTutorialMsg"];
            tts.Speak(message, QueueMode.Flush, null, needUserAnswerId);
        }

        private void ContinueUserTutorial()
        {
            var message = ConfigurationManager.AppSettings["FollowUpTutorialMsg"];
            tts.Speak(message, QueueMode.Flush, null, needUserAnswerId);
        }

        // TTS Engine method called when TTS is initialized.
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

            //TODO: schedule a TTS init method to be run here instead of writing it to this method.
            if (tutorialNeeded)
            {
                RunUserTutorial();
            }
            else
            {
                var welcomeMsg = ConfigurationManager.AppSettings["WelcomeBackMsg"];
                tts.Speak(welcomeMsg, QueueMode.Flush, null, needUserAnswerId);
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

            if (results == ConfigurationManager.AppSettings["CmdOpenCart"])
            {
                StartCartIntent();
            }
            else if (results == ConfigurationManager.AppSettings["CmdOpenList"])
            {
                StartListIntent();
            }
            else if (results == ConfigurationManager.AppSettings["CmdHelp"])
            {
                var helpMessage = ConfigurationManager.AppSettings["MainHelpMsg"];
                tts.Speak(helpMessage, QueueMode.Flush, null, needUserAnswerId);
            }
            else if (results == ConfigurationManager.AppSettings["CmdTutorialLikeShopLens"] && tutorialNeeded)
            {
                ContinueUserTutorial();
            }
            else
            {
                tts.Speak(askUserToRepeat, QueueMode.Flush, null, needUserAnswerId);
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

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            if (requestCode == REQUEST_PERMISSION)
            {
                for (int i = 0; i <= permissions.Length - 1; i++)
                {
                    if(grantResults[i] == Permission.Denied)
                    {
                        RequestPermissions(ShopLensPermissions, REQUEST_PERMISSION);
                    }
                }
            }
        }
    }
}

