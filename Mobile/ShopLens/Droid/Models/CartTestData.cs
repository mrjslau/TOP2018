using System;
using System.Collections.Generic;

namespace ShopLens.Droid.Models
{
    public static class CartTestData
    {
        public static List<CartItem> CartItems { get; private set; }

        static CartTestData()
        {
            var temp = new List<CartItem>();

            AddUser(temp);
            AddUser(temp);
            AddUser(temp);

            CartItems = temp;
        }

        static void AddUser(List<CartItem> cartItems)
        {
            cartItems.Add(new CartItem()
            {
                Name = "Basmati Rice",
                Price = "0.89€",
                Count = "1"
            });

            cartItems.Add(new CartItem()
            {
                Name = "Paulaner Weissbeer",
                Price = "1.20€",
                Count = "3"
            });
            cartItems.Add(new CartItem()
            {
                Name = "Apples",
                Price = "0.20€",
                Count = "1"
            });
        }
    }
}