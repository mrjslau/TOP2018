using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShopLensWeb
{
    public class User
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        
        public List<ShoppingSession> ShoppingSessions { get; set; }
    }
}