using Android.App;
using Android.Content;
using System;

namespace ShopLens.Droid.Notifications
{
    public class ErrorDialogCreator
    {
        private AlertDialog.Builder alertDialog;
        private Context context;
        private string title;
        private string message;

        public ErrorDialogCreator (Context context, string title, string message, string neutralButton) : this(context, title, message)
        {
            alertDialog.SetNeutralButton(neutralButton, delegate {
                alertDialog.Dispose();
            });
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
        }

        private ErrorDialogCreator(Context context, string title, string message)
        {
            this.context = context;
            this.title = title;
            this.message = message;
            alertDialog = new AlertDialog.Builder(context);
            alertDialog.SetTitle(title);
            alertDialog.SetMessage(message);
        }

        public void Show()
        {
            alertDialog.Show();
        }
    }
}