using Android.App;
using Android.Content;
using System;

namespace ShopLens.Droid.Notifications
{
    public class ErrorDialogCreator
    {
        private AlertDialog.Builder alertDialog;

        public ErrorDialogCreator (Context context, string title, string message, string neutralButton) : this(context, title, message)
        {
            alertDialog.SetNeutralButton(neutralButton, delegate {
                alertDialog.Dispose();
            });
            alertDialog.Show();
        }

        public ErrorDialogCreator(Context context, string title, string message, string positiveButton, string negativeButton, 
            Action positiveMethod, Action negativeMethod) : this(context, title, message)
        {
            alertDialog.SetPositiveButton(positiveButton, delegate {
                positiveMethod();
            });
            alertDialog.SetNegativeButton(negativeButton, delegate {
                negativeMethod();
            });
            alertDialog.Show();
        }

        private ErrorDialogCreator(Context context, string title, string message)
        {
            alertDialog = new AlertDialog.Builder(context);
            alertDialog.SetTitle(title);
            alertDialog.SetMessage(message);
        }
    }
}