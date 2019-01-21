using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopLensApp.IO
{
    /// <summary>
    ///  This class represents an "Item" that can be put in a shopping list.
    /// </summary>
    public class ShoppingItem
    {
        public string itemName;

        public ShoppingItem(string itemName)
        {
            this.itemName = itemName;
        }

        public override string ToString()
        {
            return itemName;
        }
    }
}
