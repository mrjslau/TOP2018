using Uri = Android.Net.Uri;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
using ImageRecognition.Classificators;
using Android.Speech;
using Java.Util;
using File = Java.IO.File;
using Android.Runtime;
using PCLAppConfig;

namespace ShopLens.Droid
{

    [Activity(Label = "CameraActivity")]
    public class CameraActivity : Activity, TextToSpeech.IOnInitListener
    {
        Button BtnTakeImg;
        ImageView ImgView;
        Button BtnPickImg;
        Button RecVoice;

        TextToSpeech tts;

        public File productPhoto;
        public File _dir = new File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures), "DCIM");

        public static readonly string FILE_PROVIDER_NAME = ConfigurationManager.AppSettings["fileProviderName"];

        private IDirectoryCreator shopLensPictureDirectoryCreator;

        private static readonly int REQUEST_VOICE = (int)ActivityIds.VoiceRequest;
        private static readonly int REQUEST_IMAGE = (int)ActivityIds.ImageRequest;
        private static readonly int PickImageId = (int)ActivityIds.PickImageRequest;
        private static readonly string whatIsThisCmd = ConfigurationManager.AppSettings["CmdWhatIsThis"];
        private static readonly string choosePicCmd = ConfigurationManager.AppSettings["CmdPickPhoto"];

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Camera);

            if (IsThereAnAppToTakePictures())
            {
                shopLensPictureDirectoryCreator = new ShopLensPictureDirectoryCreator();
                shopLensPictureDirectoryCreator.CreateDirectory(_dir);

                BtnTakeImg = FindViewById<Button>(Resource.Id.btntakepicture);
                ImgView = FindViewById<ImageView>(Resource.Id.ImgTakeimg);
                BtnTakeImg.Click += TakeAPicture;

                BtnPickImg = FindViewById<Button>(Resource.Id.btnPickImage);
                BtnPickImg.Click += PickOnClick;

                RecVoice = FindViewById<Button>(Resource.Id.btnRecVoiceCamera);
                RecVoice.Click += RecogniseVoice;
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
        }

        private void RecogniseVoice(object sender, EventArgs e)
        {
            var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);

            // Put a message on the modal dialog.
            voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, Application.Context.GetString(Resource.String.messageSpeakNow));

            // If there is more than 1.5s of silence, consider the speech over.
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1500);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 1500);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);

            // You can specify other languages recognised here, for example:
            // voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Locale.German).

            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Locale.Default);
            StartActivityForResult(voiceIntent, REQUEST_VOICE);
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
                takePictureIntent.AddFlags(ActivityFlags.GrantReadUriPermission);
                takePictureIntent.AddFlags(ActivityFlags.GrantWriteUriPermission);
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
            else if (requestCode == REQUEST_VOICE)
            {
                var matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
                if (matches.Count != 0)
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
            image = BitmapHelpers.ScaleDown(image, Math.Min(height, width));
            ImgView.RecycleBitmap();
            ImgView.SetImageBitmap(image);
        }

        private void RecogniseImage(Uri uri)
        {
            // Run the image recognition task
            int maxWebClassifierImageSize = int.Parse(ConfigurationManager.AppSettings["webClassifierImgSize"]);
            Task.Run(async () =>
            {
                try
                {
                    var image = MediaStore.Images.Media.GetBitmap(ContentResolver, uri);
                    image = BitmapHelpers.ScaleDown(image, maxWebClassifierImageSize);
                    using (var stream = new MemoryStream())
                    {
                        // 0 because compression quality is not applicable to .png
                        image.Compress(Bitmap.CompressFormat.Png, 0, stream);

                        var results = await new WebClassificator(ConfigurationManager.AppSettings["cvProjectId"],
                            ConfigurationManager.AppSettings["cvPredictionKey"],
                            ConfigurationManager.AppSettings["cvRequestUri"]).ClassifyImageAsync(stream.ToArray());
                        tts.Speak(
                            $"This is. {results.OrderByDescending(x => x.Value).First().Key}",
                            QueueMode.Flush,
                            null,
                            null);
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e);
                }
            });
        }
    }
}

