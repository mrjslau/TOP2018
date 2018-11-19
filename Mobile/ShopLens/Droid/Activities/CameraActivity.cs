using Uri = Android.Net.Uri;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Android;
using Android.Provider;
using Android.Graphics;
using Android.Content.PM;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Speech.Tts;
using Android.Widget;
using Camera;
using Android.Support.V4.Content;
using Android.Speech;
using Java.Util;
using File = Java.IO.File;
using Android.Runtime;
using ImageRecognitionMobile.Classificators;
using PCLAppConfig;
using ShopLens.Droid.Helpers;
using Unity;
using ShopLens.Droid.Source;
using Android.Views;
using ShopLens.Droid.Notifications;
using Android.Support.Design.Widget;

namespace ShopLens.Droid
{

    [Activity(Label = "CameraActivity")]
    public class CameraActivity : Activity, TextToSpeech.IOnInitListener, IRecognitionListener
    {
        readonly string PREFS_NAME = ConfigurationManager.AppSettings["ShopCartPrefs"];
        ActivityPreferences prefs;

        Button BtnTakeImg;
        ImageView ImgView;
        Button BtnPickImg;
        Button RecVoice;
        ProgressBar progressBar;

        SpeechRecognizer commandRecognizer;
        Intent speechIntent;

        TextToSpeech tts;

        public File productPhoto;
        public File _dir = new File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures), "DCIM");

        public static readonly string FILE_PROVIDER_NAME = ConfigurationManager.AppSettings["fileProviderName"];

        private IDirectoryCreator shopLensPictureDirectoryCreator;
        private CoordinatorLayout rootView;
        private string guess;
        private ErrorDialogCreator shoppingCartErrorDialog;
        private MessageBarCreator shoppingCartMessageBar;

        private static readonly int REQUEST_IMAGE = (int)ActivityIds.ImageRequest;
        private static readonly int REQUEST_PERMISSION = (int)ActivityIds.PermissionRequest;
        private static readonly int PickImageId = (int)ActivityIds.PickImageRequest;
        private static readonly string whatIsThisCmd = ConfigurationManager.AppSettings["CmdWhatIsThis"];
        private static readonly string choosePicCmd = ConfigurationManager.AppSettings["CmdPickPhoto"];

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Camera);

            progressBar = FindViewById<ProgressBar>(Resource.Id.progressBar);
            progressBar.Visibility = ViewStates.Gone;

            if (IsThereAnAppToTakePictures())
            {
                // Set up a custom speech recognizer in this activity.
                commandRecognizer = SpeechRecognizer.CreateSpeechRecognizer(this);
                commandRecognizer.SetRecognitionListener(this);

                shopLensPictureDirectoryCreator = DependencyInjection.Container.Resolve<IDirectoryCreator>();
                try
                {
                    shopLensPictureDirectoryCreator.CreateDirectory(_dir);
                }
                catch (Java.Lang.SecurityException e)
                {
                    System.Diagnostics.Debug.WriteLine(e);
                    RequestPermissions(new string[] { Manifest.Permission.WriteExternalStorage }, REQUEST_PERMISSION);
                }

                BtnTakeImg = FindViewById<Button>(Resource.Id.btntakepicture);
                ImgView = FindViewById<ImageView>(Resource.Id.ImgTakeimg);
                rootView = FindViewById<CoordinatorLayout>(Resource.Id.root_view);
                BtnTakeImg.Click += TakeAPicture;

                BtnPickImg = FindViewById<Button>(Resource.Id.btnPickImage);
                BtnPickImg.Click += PickOnClick;

                RecVoice = FindViewById<Button>(Resource.Id.btnRecVoiceCamera);
                RecVoice.Click += RecogniseVoice;

                shoppingCartErrorDialog = new ErrorDialogCreator(this, Resources.GetString(Resource.String.shoppingCart), 
                    Resources.GetString(Resource.String.shoppingCartQuestion), Resources.GetString(Resource.String.positiveMessage), 
                    Resources.GetString(Resource.String.negativeMessage), addToShoppingCart, doNotAddToShoppingCart);
                shoppingCartMessageBar = new MessageBarCreator(rootView, Resources.GetString(Resource.String.successMessage));
            }

            tts = new TextToSpeech(this, this);
        }

        public void OnInit([GeneratedEnum] OperationResult status)
        {
            // If initialization was successful.
            if (status == OperationResult.Success)
            {
                tts.SetLanguage(Locale.Us);
            }
            else
            {
                tts.SetLanguage(Locale.Default);
            }
        }

        private void RecogniseVoice(object sender, EventArgs e)
        {
            speechIntent = VoiceRecognizerHelper.SetUpVoiceRecognizerIntent();
            commandRecognizer.StartListening(speechIntent);
        }

        private void PickOnClick(object sender, EventArgs e)
        {
            Intent = new Intent();
            Intent.SetType("image/*");
            Intent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(Intent, "Select Picture"), PickImageId);
        }

        private bool IsThereAnAppToTakePictures()
        {
            var intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities = PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }

        private void TakeAPicture(object sender, EventArgs e)
        {
            var takePictureIntent = new Intent(MediaStore.ActionImageCapture);
            productPhoto = new File(_dir, $"Image_{Guid.NewGuid()}.jpg");
            if (productPhoto != null)
            {
                Uri photoUri = FileProvider.GetUriForFile(ApplicationContext, ApplicationContext.PackageName + FILE_PROVIDER_NAME, productPhoto);
                takePictureIntent.PutExtra(MediaStore.ExtraOutput, photoUri);
            }
            // If there's a working camera on the device.
            if (takePictureIntent.ResolveActivity(PackageManager) != null)
            {
                StartActivityForResult(takePictureIntent, REQUEST_IMAGE);
            }

        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == REQUEST_IMAGE)
            {
                if (resultCode == Result.Ok && productPhoto != null)
                {
                    Uri photoUri = FileProvider.GetUriForFile(ApplicationContext, ApplicationContext.PackageName + FILE_PROVIDER_NAME, productPhoto);
                    PictureShowBitmap(photoUri);
                    RecogniseImage(photoUri);
                }
            }
            else if (requestCode == PickImageId)
            {
                if (resultCode == Result.Ok && data != null)
                {
                    PictureShowBitmap(data.Data);
                    RecogniseImage(data.Data);
                }
            }
        }

        private void PictureShowBitmap(Uri uri)
        {
            // Put image in gallery.
            Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
            mediaScanIntent.SetData(uri);
            SendBroadcast(mediaScanIntent);

            // Conversion. 
            int height = ImgView.Height;
            int width = ImgView.Width;
            var image = MediaStore.Images.Media.GetBitmap(ContentResolver, uri);
            image = BitmapHelper.ScaleDown(image, Math.Min(height, width));
            ImgView.SetImageBitmap(image);
        }

        private void RecogniseImage(Uri uri)
        {
            // Run the image recognition task
            int maxWebClassifierImageSize = int.Parse(ConfigurationManager.AppSettings["webClassifierImgSize"]);
            
            progressBar.Visibility = ViewStates.Visible;
            Task.Run(async () =>
            {
                var image = MediaStore.Images.Media.GetBitmap(ContentResolver, uri);
                image = BitmapHelper.ScaleDown(image, maxWebClassifierImageSize);
                using (var stream = new MemoryStream())
                {
                    // 0 because compression quality is not applicable to .png
                    image.Compress(Bitmap.CompressFormat.Png, 0, stream);

                    var classificator = DependencyInjection.Container.Resolve<IAsyncImageClassificator>();
                    var results = await classificator.ClassifyImageAsync(stream.ToArray());

                    prefs = new ActivityPreferences(this, PREFS_NAME);
                    guess = results.OrderByDescending(x => x.Value).First().Key;
                    
                    tts.Speak(
                        $"This is. {guess}",
                        QueueMode.Flush,
                        null,
                        null);
                    
                }
            })
                .ContinueWith(task =>
                {
                    progressBar.Visibility = ViewStates.Gone;
                    if (task.IsFaulted)
                    {
                        System.Diagnostics.Debug.WriteLine(task.Exception);
                    }
                    if (task.IsCompletedSuccessfully)
                    {
                        shoppingCartErrorDialog.Show();                        
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public void AddToShoppingCart()
        {
            prefs.AddString(guess.First().ToString().ToUpper() + guess.Substring(1));
            shoppingCartMessageBar.Show();
        }

        public void DoNotAddToShoppingCart()
        {

        }

        // When the current voice recognition session stops.
        public void OnResults(Bundle results)
        {
            var matches = results.GetStringArrayList(SpeechRecognizer.ResultsRecognition);
            if (matches.Count > 0)
            {
                if (matches[0] == whatIsThisCmd)
                {
                    TakeAPicture(this, new EventArgs());
                }

                else if (matches[0] == choosePicCmd)
                {
                    PickOnClick(this, new EventArgs());
                }
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            if (requestCode == REQUEST_PERMISSION)
            {
                if (grantResults[0] == Permission.Denied)
                {
                    RequestPermissions(new string[] { Manifest.Permission.WriteExternalStorage }, REQUEST_PERMISSION);
                }
                else
                {
                    shopLensPictureDirectoryCreator.CreateDirectory(_dir);
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

