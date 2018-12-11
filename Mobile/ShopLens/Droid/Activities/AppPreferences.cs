using System.Collections.Generic;
using System.Linq;
using Android.Content;
using ShopLens.Droid.Models;

namespace ShopLens.Droid.Source
{
    public class ActivityPreferences
    {
        ISharedPreferences prefs;
        ISharedPreferencesEditor prefsEditor;
        Context pContext;

        public bool IsEmpty
        {
            get { return !GetPreferencesToList().Any(); }
        }

        public ActivityPreferences(Context context, string name)
        {
            pContext = context;
            prefs = pContext.GetSharedPreferences(name, FileCreationMode.Private);
            prefsEditor = prefs.Edit();
        }

        public List<string> GetPreferencesToList()
        {
            // TODO: change to LINQ
            List<string> items = new List<string> { };
            foreach (KeyValuePair<string, object> entry in prefs.All)
            {
                items.Add(entry.Value.ToString());
            }
            return items;
        }

        public void AddString(string name)
        {
            prefsEditor.PutString("item" + prefs.All.Count.ToString(), name);
            prefsEditor.Apply();
        }

        public void RemoveString(string name)
        {
            foreach (KeyValuePair<string, object> entry in prefs.All)
            {
                if (entry.Value.ToString() == name)
                {
                    prefs.Edit().Remove(entry.Key).Commit();
                }            
            }
        }

        public void DeleteAllPreferences()
        {
            prefsEditor.Clear().Commit();
        }

        public void AddCartItem(string name, string price = "0.00", int quantity = 1)
        {
            foreach (KeyValuePair<string, object> entry in prefs.All)
            {
                if (entry.Key == name)
                {
                    int newQuantity = int.Parse(entry.Value.ToString()) + 1;
                    prefs.Edit().Remove(entry.Key).Commit();
                    prefsEditor.PutInt(entry.Key, newQuantity);
                    prefsEditor.Apply();
                    return;
                }
            }

            //List<string> icollection = new List<string>();
            //icollection.Add(price);
            //icollection.Add(quantity);
            //prefsEditor.PutStringSet(name, icollection);

            prefsEditor.PutInt(name, quantity);
            prefsEditor.Apply();
        }

        public void RemoveCartItem(string name)
        {
            foreach (KeyValuePair<string, object> entry in prefs.All)
            {
                if (entry.Key == name)
                {
                    prefs.Edit().Remove(entry.Key).Commit();
                }
            }
        }

        public List<CartItem> GetCartItemPreferencesToList()
        {
            // TODO: change to LINQ
            List<CartItem> items = new List<CartItem> { };
            foreach (KeyValuePair<string, object> entry in prefs.All)
            {
                items.Add(new CartItem()
                {
                    Name = entry.Key,
                    Price = "0.00",
                    Count = entry.Value.ToString()
                });
            }
            return items;
        }
    }
}
