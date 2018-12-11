using Android.Widget;

namespace ShopLens.Droid.Helpers
{
    public class CartViewHolder : Java.Lang.Object
    {
        public TextView Name { get; set; }
        public TextView Price { get; set; }
        public TextView Count { get; set; }
        public Button RemoveButton { get; set; }
    }
}
