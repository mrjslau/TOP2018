using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Widget;

namespace ShopLens.Droid
{
    [Activity(Label = "ShoppingListActivity")]
    public class ShoppingListActivity : Activity
    {
        EditText addItemEditText;
        Button addItemButton;
        ListView listView;

        List<string> items;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ShoppingList);

            listView = FindViewById<ListView>(Resource.Id.ShopListListView);
            addItemButton = FindViewById<Button>(Resource.Id.ShopListAddItemButton);
            addItemEditText = FindViewById<EditText>(Resource.Id.ShopListAddItemEditText);

            items = new List<string> { "Coconut", "Banana", "Rice", "Beer" };
            ArrayAdapter<string> listAdapter = 
                new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItemChecked, items);
            listView.Adapter = listAdapter;
            listView.ChoiceMode = ChoiceMode.Multiple;

            addItemButton.Click += (sender, e) =>
            {
                if (!String.IsNullOrWhiteSpace(addItemEditText.Text))
                {
                    listAdapter.Add(addItemEditText.Text);
                }
                listAdapter.NotifyDataSetChanged();
            };
        }
    }
}
