namespace ShopLens.Droid.Models
{
    public class CartItem
    {
        public string Name { get; set; }
        public string Price { get; set; }
        public string Count { get; set; }

        public override string ToString()
        {
            return Count + " items of " + Name + " for " + Price;
        }
    }
}
