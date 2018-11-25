using Android;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using Android.Runtime;
using Android.Content.PM;
using Android.Speech;
using System;

using PCLAppConfig;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;
using ShopLens.Droid.Helpers;
using Android.Views;
using Android.Speech.Tts;
using Java.Util;

public enum IntentIds
{
    VoiceRequest = 101,
    ImageRequest = 201,
    PickImageRequest = 202,
    PermissionRequest = 501
}

namespace ShopLens.Droid
{
    [Activity(Label = "ShopLens", MainLauncher = true, Icon = "@mipmap/icon", Theme ="@style/ShopLensTheme")]
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

        string needUserAnswerId;
        string askUserToRepeat;

        public readonly string[] ShopLensPermissions =
        {
            Manifest.Permission.RecordAudio,
            Manifest.Permission.Camera,
            Manifest.Permission.WriteExternalStorage
        };


        static readonly int REQUEST_PERMISSION = (int)IntentIds.PermissionRequest;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ConfigurationManager.Initialise(PCLAppConfig.FileSystemStream.PortableStream.Current);
            DependencyInjection.RegisterInterfaces();

            needUserAnswerId = ConfigurationManager.AppSettings["AnswerUtteranceId"];
            askUserToRepeat = ConfigurationManager.AppSettings["AskToRepeat"];

            // Set our view from the "main" layout resource.
            SetContentView(Resource.Layout.Main);

            voiceRecognizer = new ShopLensSpeechRecognizer(OnVoiceRecognitionResults);

            ttsListener = new ShopLensUtteranceProgressListener(TtsStoppedSpeaking);

            tts = new TextToSpeech(this, this);
            tts.SetOnUtteranceProgressListener(ttsListener);

            // We need to request user permissions.
            if ((int)Build.VERSION.SdkInt >= (int)BuildVersionCodes.M)
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
                        var intentCam = new Intent(this, typeof(CameraActivity));
                        StartActivity(intentCam);
                        break;
                    case Resource.Id.NavItemShoppingCart:
                        var intentCart = new Intent(this, typeof(ShoppingCartActivity));
                        StartActivity(intentCart);
                        break;
                    case Resource.Id.NavItemShoppingList:
                        var intentList = new Intent(this, typeof(ShoppingListActivity));
                        StartActivity(intentList);
                        break;
                }
            };
            
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
            // TODO: Make it so that the tutorial only runs when the app is launched for the first time.
            RunUserTutorial();
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

            if (results == ConfigurationManager.AppSettings["CmdOpenCamera"])
            {
                var intent = new Intent(this, typeof(CameraActivity));
                StartActivity(intent);
            }
            else if (results == ConfigurationManager.AppSettings["CmdOpenCart"])
            {
                var intent = new Intent(this, typeof(ShoppingCartActivity));
                StartActivity(intent);
            }
            else if (results == ConfigurationManager.AppSettings["CmdOpenList"])
            {
                var intent = new Intent(this, typeof(ShoppingListActivity));
                StartActivity(intent);
            }
            else if (results == ConfigurationManager.AppSettings["CmdHelp"])
            {
                var helpMessage = ConfigurationManager.AppSettings["MainHelpMsg"];
                tts.Speak(helpMessage, QueueMode.Flush, null, needUserAnswerId);
            }
            else if (results == ConfigurationManager.AppSettings["CmdTutorialLikeShopLens"])
            {
                ContinueUserTutorial();
            }
            else
            {
                tts.Speak(askUserToRepeat, QueueMode.Flush, null, needUserAnswerId);
            }
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

