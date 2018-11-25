using Android.App;
using Android.Content.PM;
using Android.OS;

namespace ShopLens.Droid
{
    [Activity(Label = "Camera2Activity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class Camera2Activity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Camera2);

            if (savedInstanceState == null)
                FragmentManager.BeginTransaction().Replace(Resource.Id.container, Camera2Fragment.Create()).Commit();
        }
    }
}


