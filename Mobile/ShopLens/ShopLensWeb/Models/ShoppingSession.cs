using System;
using System.Collections.Generic;

namespace ShopLensWeb
{
    public class ShoppingSession
    {
        public Guid ShoppingSessionId { get; set; }
        public DateTime Date { get; set; }
        
        public List<Product> Products { get; set; }
        
        public Guid UserId { get; set; }
        public User User { get; set; }
        
    }
}