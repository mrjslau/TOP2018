using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShopLensWeb
{
    public class Shop
    {
        public int ShopId { get; set; }
        public string Name { get; set; }
        
        public List<Product> Products { get; set; }
    }
}