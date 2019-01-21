using System;
using System.Collections.Generic;

namespace ShopLensWeb
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        
        public List<ShoppingSession> ShoppingSessions { get; set; }
    }
}