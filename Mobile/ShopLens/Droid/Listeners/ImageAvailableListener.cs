using System;
using Android.App;
using Android.Content;
using Android.Gms.Vision.Texts;
using Android.Graphics;
using Android.Media;
using Java.IO;
using Java.Lang;
using Java.Nio;

namespace ShopLens.Droid.Listeners
{
    public class ImageAvailableListener : Java.Lang.Object, ImageReader.IOnImageAvailableListener
    {
        Context context;
        Activity activity;

        public ImageAvailableListener(Activity activity, Context context, Camera2Fragment fragment)
        {
            this.activity = activity;
            this.context = context;
            if (fragment == null)
                throw new System.ArgumentNullException("fragment");

            owner = fragment;
        }

        private readonly Camera2Fragment owner;

        public void OnImageAvailable(ImageReader reader)
        {
            owner.mBackgroundHandler.Post(new ImageSaver(activity, context, owner, reader.AcquireNextImage()));
        }

        // Saves a JPEG {@link Image} into the specified {@link File}.
        private class ImageSaver : Java.Lang.Object, IRunnable
        {
            TextRecognizer textRecognizer;
            Context context;
            // The JPEG image
            private Image mImage;
            Camera2Fragment owner;
            ImageRecognizer imageRecognizer;
            Activity activity;

            public ImageSaver(Activity activity, Context context, Camera2Fragment owner, Image image)
            {
                this.owner = owner;
                this.context = context;
                this.activity = activity;
                imageRecognizer = new ImageRecognizer(context, activity);
                mImage = image ?? throw new System.ArgumentNullException("image");
            }

            public void Run()
            {
                ByteBuffer buffer = mImage.GetPlanes()[0].Buffer;
                byte[] bytes = new byte[buffer.Remaining()];
                buffer.Get(bytes);
                Bitmap bitmapImage = BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length, null);
                textRecognizer = new TextRecognizer.Builder(context).Build();
                imageRecognizer.RecognizeImage(bitmapImage, activity, owner);
            }
        }
    }
}