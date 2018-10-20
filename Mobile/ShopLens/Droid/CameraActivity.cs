
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

namespace ShopLens.Droid
{
    [Activity(Label = "CameraActivity")]
    public class CameraActivity : Activity
    {
        Button BtnTakeImg;
        ImageView ImgView;
        public static File _file;
        public static File _dir;
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
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            _file = new File(_dir, string.Format("Image_{0}.jpg", Guid.NewGuid()));
            intent.PutExtra(MediaStore.ExtraOutput, Uri.FromFile(_file));
            StartActivityForResult(intent, 102);
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == 102 && resultCode == Result.Ok)
            {
                // ideti i galerija
                Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                Uri contentUri = Uri.FromFile(_file);
                mediaScanIntent.SetData(contentUri);
                SendBroadcast(mediaScanIntent);
                // konvertavimas 
                int height = ImgView.Height;
                int width = Resources.DisplayMetrics.WidthPixels;
                using (Bitmap bitmap = _file.Path.LoadAndResizeBitmap(width, height))
                {
                    ImgView.RecycleBitmap();
                    ImgView.SetImageBitmap(bitmap);
                    // cia ideti i DB  
                }
            }
        }
    }
}
