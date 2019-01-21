using System;
using System.Collections.Generic;

namespace ShopLensWeb
{
    public class Shop
    {
        public Guid ShopId { get; set; }
        public string Name { get; set; }
        
        public List<Product> Products { get; set; }
    }
}