using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShopLensWeb
{
    public class ShoppingSession
    {
        public int ShoppingSessionId { get; set; }
        public DateTime Date { get; set; }
        
        public List<Product> Products { get; set; }
        
        public int UserId { get; set; }
        public User User { get; set; }
        
    }
}