
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
using Java.Util;
using File = Java.IO.File;

namespace ShopLens.Droid
{

    [Activity (Label = "CameraActivity")]
    public class CameraActivity : Activity, TextToSpeech.IOnInitListener
    {
        Button BtnTakeImg;
        ImageView ImgView;
        Button BtnPickImg;
        
        TextToSpeech tts;
        
        public static readonly int PickImageId = 1000;

        public File productPhoto;
        public File _dir;

        public const int REQUEST_IMAGE = 102;
        public const string FILE_PROVIDER_NAME = ".shoplens.fileprovider";

        public static int PICK_IMAGE = 1;

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Camera);

            if (IsThereAnAppToTakePictures())
            {
                CreateDirectoryForPictures();
                BtnTakeImg = FindViewById<Button>(Resource.Id.btntakepicture);
                ImgView = FindViewById<ImageView>(Resource.Id.ImgTakeimg);
                BtnTakeImg.Enabled = false;
                BtnTakeImg.Click += TakeAPicture;

                BtnPickImg = FindViewById<Button>(Resource.Id.btnPickImage);
                BtnPickImg.Click += PickOnClick;
            }
            
            tts = new TextToSpeech(this, this);
        }

        private void PickOnClick(object sender, EventArgs eventArgs)
        {
            Intent = new Intent();
            Intent.SetType("image/*");
            Intent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(Intent, "Select Picture"), PickImageId);
        }

        private void CreateDirectoryForPictures()
        {
            _dir = new File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures), "C#Corner");
            if (!_dir.Exists())
            {
                _dir.Mkdirs();
            }
        }
        private bool IsThereAnAppToTakePictures()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities = PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }
        private void TakeAPicture(object sender, EventArgs eventArgs)
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
            if (takePictureIntent.ResolveActivity(PackageManager) != null) {
                StartActivityForResult(takePictureIntent, REQUEST_IMAGE);
            }
            
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == REQUEST_IMAGE && resultCode == Result.Ok)
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
            else if (requestCode == REQUEST_IMAGE && resultCode == Result.Canceled)
            {
                // I don't know what we should do if the user cancelled the photo taking activity.
            }
            else if ((requestCode == PickImageId) && (resultCode == Result.Ok) && (data != null))
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
        
        public void OnInit(OperationResult status)
        {
            if(status == OperationResult.Success)
            {
                tts.SetLanguage(Locale.Us);
            }
        }
    }
}
