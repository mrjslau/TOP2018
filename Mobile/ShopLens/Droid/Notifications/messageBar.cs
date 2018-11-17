using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;

namespace ShopLens.Droid.Notifications
{
    public class MessageBar
    {
        public MessageBar(View rootView, string message)
        {
            Snackbar.Make(rootView, message, Snackbar.LengthLong)
            .Show();
        }

        public MessageBar(View rootView, string message, string buttonName, Action method)
        {
            Snackbar.Make(rootView, message, Snackbar.LengthLong)
            .SetAction(buttonName, delegate {
                method();
            })
            .Show();
        }
    }
}