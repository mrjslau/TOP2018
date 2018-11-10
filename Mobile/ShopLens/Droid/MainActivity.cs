using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;

namespace ShopLens.Droid
{
    [Activity(Label = "ShopLens", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            // Set our view from the "main" layout resource.
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it.
            Button textVoicerButton = FindViewById<Button>(Resource.Id.TextVoicerButton);
            Button cameraButton = FindViewById<Button>(Resource.Id.CameraButton);
            Button speechButton = FindViewById<Button>(Resource.Id.SpeechButton);

            textVoicerButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(TextVoicerActivity));
                StartActivity(intent);
            };

            cameraButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(CameraActivity));
                StartActivity(intent);
            };

            speechButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(SpeechActivity));
                StartActivity(intent);
            };
        }
    }
}

