﻿using Android.App;
using Android.OS;

namespace ShopLens.Droid
{
    [Activity(Label = "Camera2Basic", Icon = "@drawable/icon")]
    public class Camera2Activity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.activity_camera);

            if (bundle == null)
            {
                FragmentManager.BeginTransaction().Replace(Resource.Id.container, Camera2Fragment.NewInstance()).Commit();
            }
        }
    }
}

