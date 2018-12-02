using System;
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

        public ImageAvailableListener(Context context, Camera2Fragment fragment, File file)
        {
            this.context = context;
            if (fragment == null)
                throw new System.ArgumentNullException("fragment");
            if (file == null)
                throw new System.ArgumentNullException("file");

            owner = fragment;
            this.file = file;
        }

        private readonly File file;
        private readonly Camera2Fragment owner;

        public void OnImageAvailable(ImageReader reader)
        {
            owner.mBackgroundHandler.Post(new ImageSaver(context, owner, reader.AcquireNextImage(), file));
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

            // The file we save the image into.
            private File mFile;

            public ImageSaver(Context context, Camera2Fragment owner, Image image, File file)
            {
                this.owner = owner;
                this.context = context;
                imageRecognizer = new ImageRecognizer(context);
                mImage = image ?? throw new System.ArgumentNullException("image");
                mFile = file ?? throw new System.ArgumentNullException("file");
            }

            public void Run()
            {
                ByteBuffer buffer = mImage.GetPlanes()[0].Buffer;
                byte[] bytes = new byte[buffer.Remaining()];
                buffer.Get(bytes);
                Bitmap bitmapImage = BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length, null);
                textRecognizer = new TextRecognizer.Builder(context).Build();
                imageRecognizer.RecogniseImage(bitmapImage, owner);
            }
        }
    }
}