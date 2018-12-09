namespace ShopLensWeb
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int Discount { get; set; }
        public decimal FullPrice { get; set; }
        
        public int ShopId { get; set; }
        public Shop Shop { get; set; }
    }
}