using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace ShopLens.Droid.Activities
{
    [Activity(Label = "ShopLens", MainLauncher = true, Icon = "@mipmap/icon", Theme = "@style/ShopLensTheme",
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class PermissionActivity : Activity
    {
        static readonly int REQUEST_PERMISSION = (int)IntentIds.PermissionRequest;

        public readonly string[] ShopLensPermissions =
        {
            Manifest.Permission.RecordAudio,
            Manifest.Permission.Camera,
            Manifest.Permission.WriteExternalStorage
        };

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // We need to request user permissions.
            if ((int)Build.VERSION.SdkInt >= (int)BuildVersionCodes.M)
            {
                RequestPermissions(ShopLensPermissions, REQUEST_PERMISSION);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            if (requestCode == REQUEST_PERMISSION)
            {
                for (int i = 0; i <= permissions.Length - 1; i++)
                {
                    if (grantResults[i] == Permission.Denied)
                    {
                        RequestPermissions(ShopLensPermissions, REQUEST_PERMISSION);
                    }
                }
                var intentMain = new Intent(this, typeof(MainActivity));
                StartActivity(intentMain);
            }
        }
    }
}