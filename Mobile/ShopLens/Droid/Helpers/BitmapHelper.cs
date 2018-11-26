using System;
using Android.Widget;
using Android.Graphics;
using Android.Graphics.Drawables;
namespace Camera
{
    public static class BitmapHelper
    {
        // This method will recyle the memory help by a bitmap in an ImageView.
        public static void RecycleBitmap(this ImageView imageView)
        {
            if (imageView == null)
            {
                return;
            }
            Drawable toRecycle = imageView.Drawable;
            if (toRecycle != null)
            {
                ((BitmapDrawable)toRecycle).Bitmap.Recycle();
            }
        }
        
        // Resize image to the specified dimensions.  
        public static Bitmap ScaleDown(Bitmap realImage, float maxImageSize) {
            var ratio = Math.Min(
                maxImageSize / realImage.Width,
                maxImageSize / realImage.Height);
            var width = Math.Round(ratio * realImage.Width);
            var height = Math.Round(ratio * realImage.Height);

            var newBitmap = Bitmap.CreateScaledBitmap(realImage, (int) width, (int) height, true);
            return newBitmap;
        }
    }
}