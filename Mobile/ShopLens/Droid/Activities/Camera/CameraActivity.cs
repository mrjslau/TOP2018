using Uri = Android.Net.Uri;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android;
using Android.Provider;
using Android.Content.PM;
using Android.App;
using Android.Content;
using Android.Gms.Vision.Texts;
using Android.OS;
using Android.Speech.Tts;
using Android.Widget;
using Camera;
using Android.Support.V4.Content;
using Android.Speech;
using Java.Util;
using File = Java.IO.File;
using Android.Runtime;
using PCLAppConfig;
using ShopLens.Droid.Helpers;
using Unity;
using ShopLens.Droid.Source;
using Android.Views;
using ShopLens.Droid.Notifications;
using Android.Support.Design.Widget;
using ImageRecognitionMobile;
using ImageRecognitionMobile.OCR;
using ShopLens.Extensions;

namespace ShopLens.Droid
{

    [Activity(Label = "CameraActivity", Theme = "@style/ShopLensTheme")]
    public partial class CameraActivity : Activity, TextToSpeech.IOnInitListener, IRecognitionListener
    {
        private Button btnTakeImg;
        private ImageView imgView;
        private Button btnPickImg;
        private Button recVoice;
        private ProgressBar progressBar;
        private CoordinatorLayout rootView;
        private MessageBarCreator shoppingCartItemAddedMessageBar;
        private SpeechRecognizer commandRecognizer;
        private Intent speechIntent;

        private File productPhoto;
        private File dir =
            new File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures),
                "DCIM");

        private static readonly string FileProviderName = ConfigurationManager.AppSettings["fileProviderName"];
        private readonly string prefsName = ConfigurationManager.AppSettings["ShopCartPrefs"];
        
        private static readonly int RequestImageId = (int) ActivityIds.ImageRequest;
        private static readonly int RequestPermissionId = (int) ActivityIds.PermissionRequest;
        private static readonly int PickImageId = (int) ActivityIds.PickImageRequest;
        private static readonly string CmdWhatIsThis = ConfigurationManager.AppSettings["CmdWhatIsThis"];
        private static readonly string CmdPickPhoto = ConfigurationManager.AppSettings["CmdPickPhoto"];

        private IDirectoryCreator shopLensPictureDirectoryCreator;
        private TextToSpeech tts;
        private TextRecognizer textRecognizer;

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
                    shopLensPictureDirectoryCreator.CreateDirectory(dir);
                }
                catch (Java.Lang.SecurityException e)
                {
                    System.Diagnostics.Debug.WriteLine(e);
                    RequestPermissions(new string[] { Manifest.Permission.WriteExternalStorage }, RequestPermissionId);
                }

                btnTakeImg = FindViewById<Button>(Resource.Id.btntakepicture);
                imgView = FindViewById<ImageView>(Resource.Id.ImgTakeimg);
                rootView = FindViewById<CoordinatorLayout>(Resource.Id.root_view);
                btnTakeImg.Click += TakeAPicture;

                btnPickImg = FindViewById<Button>(Resource.Id.btnPickImage);
                btnPickImg.Click += PickOnClick;

                recVoice = FindViewById<Button>(Resource.Id.btnRecVoiceCamera);
                recVoice.Click += RecogniseVoice;

                
                shoppingCartItemAddedMessageBar = new MessageBarCreator(rootView, Resources.GetString(Resource.String.successMessage));
            }

            tts = new TextToSpeech(this, this);
            textRecognizer = new TextRecognizer.Builder(ApplicationContext).Build();
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

        private void TakeAPicture(object sender, EventArgs e)
        {
            var takePictureIntent = new Intent(MediaStore.ActionImageCapture);
            productPhoto = new File(dir, $"Image_{Guid.NewGuid()}.jpg");
            if (productPhoto != null)
            {
                var photoUri = FileProvider.GetUriForFile(ApplicationContext, ApplicationContext.PackageName + FileProviderName, productPhoto);
                takePictureIntent.PutExtra(MediaStore.ExtraOutput, photoUri);
            }
            // If there's a working camera on the device.
            if (takePictureIntent.ResolveActivity(PackageManager) != null)
            {
                StartActivityForResult(takePictureIntent, RequestImageId);
            }

        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == RequestImageId)
            {
                if (resultCode == Result.Ok && productPhoto != null)
                {
                    var photoUri = FileProvider.GetUriForFile(ApplicationContext, ApplicationContext.PackageName + FileProviderName, productPhoto);
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
            var mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
            mediaScanIntent.SetData(uri);
            SendBroadcast(mediaScanIntent);

            // Conversion. 
            var height = imgView.Height;
            var width = imgView.Width;
            var image = MediaStore.Images.Media.GetBitmap(ContentResolver, uri);
            image = BitmapHelper.ScaleDown(image, Math.Min(height, width));
            imgView.SetImageBitmap(image);
        }

        private void RecogniseImage(Uri uri)
        {
            progressBar.Visibility = ViewStates.Visible;
            Task.Run(async () =>
                {
                    var image = GetResizedImageForClassification(uri);

                    var classifyImageTask = GetClassifyImageTask(image);

                    if (!textRecognizer.IsOperational)
                    {
                        System.Diagnostics.Debug.WriteLine("[Warning]: Text recognizer not operational.");
                        return new RecognitionResult { Predictions = await classifyImageTask };
                    }
                    
                    var recognizeTextTask = GetTextFromImageAsync(image);

                    await Task.WhenAll(classifyImageTask, recognizeTextTask);

                    var ocrResult = recognizeTextTask.Result;
                    var weightString = new RegexMetricWeightSubstringFinder().FindWeightSpecifier(ocrResult);

                    var recognitionResult = new RecognitionResult
                    {
                        RawOcrResult = ocrResult,
                        Predictions = classifyImageTask.Result,
                        WeightSpecifier = weightString
                    };
                    return recognitionResult;
                })
                .ContinueWith(task =>
                {
                    progressBar.Visibility = ViewStates.Gone;
                    if (task.IsFaulted)
                    {
                        System.Diagnostics.Debug.WriteLine(task.Exception);
                        return;
                    }

                    var recognitionResult = task.Result;
                    var thingsToSay = new List<string>
                        {$"This is {recognitionResult.BestPrediction}", recognitionResult.WeightSpecifier};
                    
                    tts.Speak(
                        string.Join(". ", thingsToSay),
                        QueueMode.Flush,
                        null,
                        null);

                    var ocrResult = recognitionResult.RawOcrResult;
                    if (!string.IsNullOrEmpty(ocrResult))
                        new MessageBarCreator(rootView, $"OCR: {ocrResult}").Show();

                    GetShoppingCartAddItemDialog(recognitionResult).Show();
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void AddToShoppingCart(string guess)
        {
            var preferences = new ActivityPreferences(this, prefsName);
            preferences.AddString(guess.FirstCharToUpper());
            shoppingCartItemAddedMessageBar.Show();
        }

        // When the current voice recognition session stops.
        public void OnResults(Bundle results)
        {
            var matches = results.GetStringArrayList(SpeechRecognizer.ResultsRecognition);
            if (matches.Count <= 0) return;
            
            if (matches[0] == CmdWhatIsThis)
            {
                TakeAPicture(this, new EventArgs());
            }

            else if (matches[0] == CmdPickPhoto)
            {
                PickOnClick(this, new EventArgs());
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            if (requestCode == RequestPermissionId)
            {
                if (grantResults[0] == Permission.Denied)
                {
                    RequestPermissions(new string[] { Manifest.Permission.WriteExternalStorage }, RequestPermissionId);
                }
                else
                {
                    shopLensPictureDirectoryCreator.CreateDirectory(dir);
                }
            }
        }    
    }
}

