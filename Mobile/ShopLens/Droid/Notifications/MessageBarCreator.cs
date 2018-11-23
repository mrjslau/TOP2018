using System;
using Android.Support.Design.Widget;
using Android.Views;

namespace ShopLens.Droid.Notifications
{
    public class MessageBarCreator
    {
        private View rootView;
        private string message;
        private string buttonName;
        private Snackbar snackbar;

        public MessageBarCreator(View rootView, string message)
        {
            this.rootView = rootView;
            this.message = message;
            snackbar = Snackbar.Make(rootView, message, Snackbar.LengthLong);
        }

        public MessageBarCreator(View rootView, string message, string buttonName, Action method) : this(rootView, message)
        {
            this.buttonName = buttonName;
            snackbar.SetAction(buttonName, delegate {
                 method();
             });
        }

        public void Show()
        {
            snackbar.Show();
        }
    }
}