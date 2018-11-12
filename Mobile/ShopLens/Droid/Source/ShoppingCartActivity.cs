using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Widget;
using ShopLens.Droid.Source;
using PCLAppConfig;

namespace ShopLens.Droid
{
    [Activity(Label = "ShoppingCartActivity")]
    public class ShoppingCartActivity : Activity
    {
        readonly string PREFS_NAME = ConfigurationManager.AppSettings["ShopCartPrefs"];

        EditText addItemEditText;
        Button addItemButton;
        ListView listView;
        ActivityPreferences prefs;

        List<string> items;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ShoppingCart);

            prefs = new ActivityPreferences(this, PREFS_NAME);
            items = prefs.GetPreferencesToList();

            listView = FindViewById<ListView>(Resource.Id.ShopCartList);
            addItemButton = FindViewById<Button>(Resource.Id.ShopCartAddItemButton);
            addItemEditText = FindViewById<EditText>(Resource.Id.ShopCartAddItemEditText);

            ArrayAdapter<string> listAdapter = 
                new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItemChecked, items);
            listView.Adapter = listAdapter;
            listView.ChoiceMode = ChoiceMode.Multiple;

            addItemButton.Click += (sender, e) =>
            {
                if (!String.IsNullOrWhiteSpace(addItemEditText.Text)){
                    listAdapter.Add(addItemEditText.Text);
                    prefs.AddString(addItemEditText.Text);
                }
                listAdapter.NotifyDataSetChanged();
            };
        }

    }
}
