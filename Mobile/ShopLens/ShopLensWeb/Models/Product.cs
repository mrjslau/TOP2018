using System;

namespace ShopLensWeb
{
    public class Product
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public int Discount { get; set; }
        public decimal FullPrice { get; set; }
        
        public Guid ShopId { get; set; }
        public Shop Shop { get; set; }
    }
}