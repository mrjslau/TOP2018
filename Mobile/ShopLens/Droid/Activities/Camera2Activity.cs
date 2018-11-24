using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace ShopLens.Droid
{
    [Activity(Label = "@string/app_name")]
    public class Camera2Activity : Activity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_camera);

            if (savedInstanceState == null)
                FragmentManager.BeginTransaction().Replace(Resource.Id.container, Camera2RawFragment.Create()).Commit();

        }
    }
}


