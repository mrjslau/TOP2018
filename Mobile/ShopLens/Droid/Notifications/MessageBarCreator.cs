using System;
using Android.Support.Design.Widget;
using Android.Views;

namespace ShopLens.Droid.Notifications
{
    public class MessageBarCreator
    {
        public MessageBarCreator(View rootView, string message)
        {
            Snackbar.Make(rootView, message, Snackbar.LengthLong)
            .Show();
        }

        public MessageBarCreator(View rootView, string message, string buttonName, Action method)
        {
            Snackbar.Make(rootView, message, Snackbar.LengthLong)
            .SetAction(buttonName, delegate {
                method();
            })
            .Show();
        }
    }
}