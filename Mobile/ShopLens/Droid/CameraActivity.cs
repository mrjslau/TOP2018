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

        public static readonly int PickImageId = 1000;

        public File productPhoto;
        public File _dir;

        // TO DO: change ID constants into global enum.
        public const int REQUEST_IMAGE = 102;
        public const string FILE_PROVIDER_NAME = ".shoplens.fileprovider";

        private IDirectoryCreator shopLensPictureDirectoryCreator;

        private const string whatIsThisCmd = "what is this";
        private const string choosePicCmd = "I have a photo";
        private const int REQUEST_VOICE = 10;

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

            // If there is more then 1.5s of silence, consider the speech over.
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
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities = PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }

        private void TakeAPicture(object sender, EventArgs e)
        {
            Intent takePictureIntent = new Intent(MediaStore.ActionImageCapture);
            productPhoto = new File(_dir, string.Format("Image_{0}.jpg", Guid.NewGuid()));
            if (productPhoto != null)
            {
                Uri photoUri = FileProvider.GetUriForFile(ApplicationContext, ApplicationContext.PackageName + FILE_PROVIDER_NAME, productPhoto);
                takePictureIntent.PutExtra(MediaStore.ExtraOutput, photoUri);
                takePictureIntent.SetFlags(ActivityFlags.GrantReadUriPermission);
                takePictureIntent.SetFlags(ActivityFlags.GrantWriteUriPermission);
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
                    PictureShowBitmap();
                    RecogniseImage(data);
                }
            }
            else if (requestCode == PickImageId)
            {
                if (resultCode == Result.Ok && data != null)
                {
                    RecogniseImage(data);
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
                        PictureShowBitmap();
                        RecogniseImage(data);
                    }

                    else if (matches[0] == choosePicCmd)
                    {
                        PickOnClick(this, new EventArgs());
                        RecogniseImage(data);
                    }
                }
            }
        }

        private void PictureShowBitmap()
        {
            // Put image in gallery.
            Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
            Uri contentUri = Uri.FromFile(productPhoto);
            mediaScanIntent.SetData(contentUri);
            SendBroadcast(mediaScanIntent);

            // Conversion. 
            int height = ImgView.Height;
            int width = Resources.DisplayMetrics.WidthPixels;
            using (Bitmap bitmap = productPhoto.Path.LoadAndResizeBitmap(width, height))
            {
                ImgView.RecycleBitmap();
                ImgView.SetImageBitmap(bitmap);
            }
        }

        private void RecogniseImage(Intent data)
        {
            Uri uri = data.Data;
            ImgView.SetImageURI(uri);

            // Run the image recognition task
            Task.Run(() =>
            {
                try
                {
                    var image = MediaStore.Images.Media.GetBitmap(ContentResolver, uri);
                    using (var stream = new MemoryStream())
                    {
                        // 0 because compression quality not applicable to .png
                        image.Compress(Bitmap.CompressFormat.Png, 0, stream);

                        var results = new WebClassificator().ClassifyImage(stream.ToArray());
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

