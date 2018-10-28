
using Uri = Android.Net.Uri;
using System;
using System.Collections.Generic;
using Java.IO;

using Android.Provider;
using Android.Graphics;
using Android.Content.PM;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Camera;
using Android.Support.V4.Content;
using Android.Support.Compat;

namespace ShopLens.Droid
{

    [Activity (Label = "CameraActivity")]
    public class CameraActivity : Activity
    {
        Button BtnTakeImg;
        ImageView ImgView;

        public static File productPhoto;
        public static File _dir;

        public static int REQUEST_IMAGE = 102;
        public static string FILE_PROVIDER_NAME = ".shoplens.fileprovider";

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Camera);

            if (IsThereAnAppToTakePictures())
            {
                CreateDirectoryForPictures();
                BtnTakeImg = FindViewById<Button>(Resource.Id.btntakepicture);
                ImgView = FindViewById<ImageView>(Resource.Id.ImgTakeimg);
                BtnTakeImg.Click += TakeAPicture;
            }
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
            //If there's a working camera on the device.
            if (takePictureIntent.ResolveActivity(PackageManager) != null) {
                StartActivityForResult(takePictureIntent, REQUEST_IMAGE);
            }
            
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == REQUEST_IMAGE && resultCode == Result.Ok)
            {
                // ideti i galerija
                Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                Uri contentUri = Uri.FromFile(productPhoto);
                mediaScanIntent.SetData(contentUri);
                SendBroadcast(mediaScanIntent);
                // konvertavimas 
                int height = ImgView.Height;
                int width = Resources.DisplayMetrics.WidthPixels;
                using (Bitmap bitmap = productPhoto.Path.LoadAndResizeBitmap(width, height))
                {
                    ImgView.RecycleBitmap();
                    ImgView.SetImageBitmap(bitmap);
                    // cia ideti i DB  
                }
            }
        }
    }
}
