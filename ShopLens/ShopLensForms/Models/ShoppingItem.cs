using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopLensApp.IO
{
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
