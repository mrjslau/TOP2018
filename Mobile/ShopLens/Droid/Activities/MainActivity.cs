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

public enum ActivityIds
{
    VoiceRequest = 101,
    ImageRequest = 201,
    PickImageRequest = 202,
    PermissionRequest = 501
}

namespace ShopLens.Droid
{
    [Activity(Label = "ShopLens", MainLauncher = true, Icon = "@mipmap/icon", Theme ="@style/ShopLensTheme")]
    public class MainActivity : AppCompatActivity, IRecognitionListener
    {
        Lazy<SpeechRecognizer> commandRecognizer;
        Intent speechIntent;
        Button voiceCommandButton;
        SupportToolbar toolbar;
        ActionBarDrawerToggle drawerToggle;
        DrawerLayout drawerLayout;
        NavigationView navView;
        CoordinatorLayout rootView;

        public readonly string[] ShopLensPermissions =
        {
            Manifest.Permission.RecordAudio,
            Manifest.Permission.Camera,
            Manifest.Permission.WriteExternalStorage
        };


        static readonly int REQUEST_PERMISSION = (int)ActivityIds.PermissionRequest;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // We need to request user permissions.
            if ((int)Build.VERSION.SdkInt >= 23)
            {
                RequestPermissions(ShopLensPermissions, REQUEST_PERMISSION);
            }
            
            ConfigurationManager.Initialise(PCLAppConfig.FileSystemStream.PortableStream.Current);
            DependencyInjection.RegisterInterfaces();

            // Set our view from the "main" layout resource.
            SetContentView(Resource.Layout.Main);

            commandRecognizer = new Lazy<SpeechRecognizer>(() => SpeechRecognizer.CreateSpeechRecognizer(this));

            if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsVoiceRecognitionEnabled"]))
            {
                commandRecognizer.Value.SetRecognitionListener(this);
            }

            // Find Resources
            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.DrawerLayout);
            toolbar = FindViewById<SupportToolbar>(Resource.Id.Toolbar);
            navView = FindViewById<NavigationView>(Resource.Id.NavView);
            rootView = FindViewById<CoordinatorLayout>(Resource.Id.root_view);
            voiceCommandButton = FindViewById<Button>(Resource.Id.MainRecordingButton);

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

            // Events
            voiceCommandButton.Click += RecogniseVoice;

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
                    case Resource.Id.NavItemTextToSpeech:
                        var intentTTS = new Intent(this, typeof(TextVoicerActivity));
                        StartActivity(intentTTS);
                        break;
                    case Resource.Id.NavItemSpeechRecogniser:
                        var intentSpeech = new Intent(this, typeof(SpeechActivity));
                        StartActivity(intentSpeech);
                        break;
                    case Resource.Id.NavItemCamera2:
                        var intentCamera2 = new Intent(this, typeof(Camera2Activity));
                        StartActivity(intentCamera2);
                        break;
                }
            };
        }

        void RecogniseVoice(object sender, EventArgs e)
        {
            if (commandRecognizer.IsValueCreated)
            {
                speechIntent = VoiceRecognizerHelper.SetUpVoiceRecognizerIntent();
                commandRecognizer.Value.StartListening(speechIntent);
            }
        }

        // When the current voice recognition session stops.
        public void OnResults(Bundle results)
        {
            var matches = results.GetStringArrayList(SpeechRecognizer.ResultsRecognition);
            if (matches.Count > 0)
            {
                if (matches[0] == ConfigurationManager.AppSettings["CmdOpenCamera"])
                {
                    var intent = new Intent(this, typeof(CameraActivity));
                    StartActivity(intent);
                }
                else if (matches[0] == ConfigurationManager.AppSettings["CmdOpenCart"])
                {
                    var intent = new Intent(this, typeof(ShoppingCartActivity));
                    StartActivity(intent);
                }
                else if (matches[0] == ConfigurationManager.AppSettings["CmdOpenList"])
                {
                    var intent = new Intent(this, typeof(ShoppingListActivity));
                    StartActivity(intent);
                }
                // For debugging purposes.
                else
                {
                    voiceCommandButton.Text = matches[0];
                }
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            drawerToggle.OnOptionsItemSelected(item);
            return base.OnOptionsItemSelected(item);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            if(requestCode == REQUEST_PERMISSION)
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

