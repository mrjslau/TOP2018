using System.Collections.Generic;
using Android.Content;

namespace ShopLens.Droid.Source
{
    public class ActivityPreferences
    {
        ISharedPreferences prefs;
        ISharedPreferencesEditor prefsEditor;
        Context pContext;

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
    }
}
