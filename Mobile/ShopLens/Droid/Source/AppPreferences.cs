using System.Collections.Generic;
using Android.Content;

namespace ShopLens.Droid.Source
{
    public class ActivityPreferences
    {
        ISharedPreferences prefs;
        ISharedPreferencesEditor prefsEditor;
        Context pContext;
        List<string> items;
        int itemCounter = 0;
        //test

        public ActivityPreferences(Context context, string name)
        {
            pContext = context;
            prefs = pContext.GetSharedPreferences(name, FileCreationMode.Private);
            prefsEditor = prefs.Edit();
            foreach (KeyValuePair<string, object> entry in prefs.All)
            {
                itemCounter += 1;
            }
        }

        public List<string> GetPreferencesToList()
        {
            if (items == null)
            {
                items = new List<string> { };
            }
            foreach (KeyValuePair<string, object> entry in prefs.All)
            {
                items.Add(entry.Value.ToString());
            }
            return items;
        }

        public void AddString(string name)
        {
            itemCounter += 1;
            prefsEditor.PutString("item" + itemCounter.ToString(), name);
            prefsEditor.Apply();
        }
    }
}
