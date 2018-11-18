using Android.App;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using SupportActionBarDrawerToggle = Android.Support.V7.App.ActionBarDrawerToggle;

namespace ShopLens.Droid.Activities
{
    public class ActionBarDrawerToggle : SupportActionBarDrawerToggle
    {
        AppCompatActivity hostActivity;
        readonly int openedResource;
        readonly int closedResource;


        public ActionBarDrawerToggle(AppCompatActivity activity, DrawerLayout drawerLayout, Toolbar toolbar,
                                     int openDrawerContentDescRes, int closeDrawerContentDescRes)
            : base(activity, drawerLayout, toolbar, openDrawerContentDescRes, closeDrawerContentDescRes) 
        {
            hostActivity = activity;
            openedResource = openDrawerContentDescRes;
            closedResource = closeDrawerContentDescRes;
        }

        public override void OnDrawerOpened(Android.Views.View drawerView)
        {
            base.OnDrawerOpened(drawerView);
            hostActivity.SupportActionBar.SetTitle(openedResource);
        }

        public override void OnDrawerClosed(Android.Views.View drawerView)
        {
            base.OnDrawerClosed(drawerView);
            hostActivity.SupportActionBar.SetTitle(closedResource);
        }

        public override void OnDrawerSlide(Android.Views.View drawerView, float slideOffset)
        {
            base.OnDrawerSlide(drawerView, slideOffset);
        }
    }
}
