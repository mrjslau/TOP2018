using System.Collections.Generic;
using Android.Database;
using Android.Views;
using Android.Widget;
using ShopLens.Droid.Models;
using static Android.Support.V7.Widget.RecyclerView;

namespace ShopLens.Droid.Helpers
{
    public class CartItemsAdapter : BaseAdapter<CartItem>
    {
        List<CartItem> items;

        public CartItemsAdapter(List<CartItem> items)
        {
            this.items = items;
        }

        public void AddCartItem(string name, string price = "0.00", string count = "1")
        {
            foreach (CartItem ci in items)
            {
                if (ci.Name == name)
                {
                    CartItem newCartItem = new CartItem()
                    {
                        Name = name,
                        Price = price,
                        Count = (int.Parse(ci.Count) + int.Parse(count)).ToString()
                    };
                    items.Remove(ci);
                    items.Add(newCartItem);
                    return;
                }
            }

            items.Add(new CartItem()
            {
                Name = name,
                Price = price,
                Count = count
            });
        }

        public override CartItem this[int position]
        {
            get
            {
                return items[position];
            }
        }

        public override int Count
        {
            get
            {
                return items.Count;
            }
        }

        public void Clear()
        {
            items = new List<CartItem>();
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;

            if (view == null)
            {
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.cart_item, parent, false);

                var name = view.FindViewById<TextView>(Resource.Id.cartItemNameTextView);
                var price = view.FindViewById<TextView>(Resource.Id.cartItemPriceTextView);
                var count = view.FindViewById<TextView>(Resource.Id.cartItemCountTextView);
                var removeBtn = view.FindViewById<Button>(Resource.Id.cartItemRemoveItemButton);

                view.Tag = new CartViewHolder() { Name = name, Price = price, Count = count, RemoveButton = removeBtn};
            }

            var holder = (CartViewHolder)view.Tag;

            holder.Name.Text = items[position].Name;
            holder.Price.Text = items[position].Price;
            holder.Count.Text = items[position].Count;

            return view;
        }
    }
}
