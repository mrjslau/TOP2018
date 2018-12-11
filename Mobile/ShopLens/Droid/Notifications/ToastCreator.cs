using Android.Content;
using Android.Widget;
using Java.Lang;

namespace ShopLens.Droid
{
    public class ToastCreator : Java.Lang.Object, IRunnable
    {
        private readonly string text;
        private readonly Context context;

        public ToastCreator(Context context, string text)
        {
            this.context = context;
            this.text = text;
        }

        public void Run()
        {
            Toast.MakeText(context, text, ToastLength.Short).Show();
        }
    }
}