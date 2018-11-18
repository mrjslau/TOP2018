using Android.Support.V4.Widget;
using Android.Support.V7.App;
using SupportActionBarDrawerToggle = Android.Support.V7.App.ActionBarDrawerToggle;

namespace ShopLens.Droid.Activities
{
    public class ActionBarDrawerToggle : SupportActionBarDrawerToggle
    {
        AppCompatActivity hostActivity;
        readonly int openedResource;
        readonly int closedResource;


        public ActionBarDrawerToggle(AppCompatActivity host, DrawerLayout drawerLayout, int openedResource, int closedResource)
            : base(host, drawerLayout, openedResource, closedResource)
        {
            hostActivity = host;
            this.openedResource = openedResource;
            this.closedResource = closedResource;
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
