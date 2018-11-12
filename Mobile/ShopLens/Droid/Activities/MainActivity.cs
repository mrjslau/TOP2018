using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using PCLAppConfig;

public enum ActivityIds
{
    VoiceRequest = 101,
    ImageRequest = 201,
    PickImageRequest = 202
}

namespace ShopLens.Droid
{
    [Activity(Label = "ShopLens", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ConfigurationManager.Initialise(PCLAppConfig.FileSystemStream.PortableStream.Current);

            // Set our view from the "main" layout resource.
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it.
            Button textVoicerButton = FindViewById<Button>(Resource.Id.TextVoicerButton);
            Button cameraButton = FindViewById<Button>(Resource.Id.CameraButton);
            Button speechButton = FindViewById<Button>(Resource.Id.SpeechButton);
            Button shoppingListButton = FindViewById<Button>(Resource.Id.ShoppingListButton);
            Button shoppingCartButton = FindViewById<Button>(Resource.Id.ShoppingCartButton);

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

            shoppingListButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(ShoppingListActivity));
                StartActivity(intent);
            };

            shoppingCartButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(ShoppingCartActivity));
                StartActivity(intent);
            };
        }
    }
}

