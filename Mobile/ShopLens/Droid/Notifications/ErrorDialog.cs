using Android.App;
using Android.Content;
using System;

namespace ShopLens.Droid.Notifications
{
    public class ErrorDialog
    {
        public AlertDialog.Builder alertDialog;

        private ErrorDialog(Context context, string title, string message)
        {
            alertDialog = new AlertDialog.Builder(context);
            alertDialog.SetTitle(title);
            alertDialog.SetMessage(message);
        }

        public ErrorDialog (Context context, string title, string message, string neutralButton) : this(context, title, message)
        {
            alertDialog.SetNeutralButton(neutralButton, delegate {
                alertDialog.Dispose();
            });
        }

        public ErrorDialog(Context context, string title, string message, string positiveButton, string negativeButton, 
            Action positiveMethod, Action negativeMethod) : this(context, title, message)
        {
            alertDialog.SetPositiveButton(positiveButton, delegate {
                positiveMethod();
            });
            alertDialog.SetNegativeButton(negativeButton, delegate {
                negativeMethod();
            });
        }

        public void Show ()
        {
            alertDialog.Show();
        }        
    }
}