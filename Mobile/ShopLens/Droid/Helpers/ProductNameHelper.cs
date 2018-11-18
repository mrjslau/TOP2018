namespace ShopLens.Droid.Helpers
{
    public static class ProductNameHelper
    {
        public static string GetProductNameFromString(string products)
        {
            string[] array = products.Split(' ');
            string productName = string.Empty;

            for (int i = 0; i <= array.Length - 1; i++)
            {
                if (i > 0)
                {
                    if (i == 1)
                    {
                        productName += array[i];
                    }
                    else
                    {
                        productName += " " + array[i];
                    }
                }
            }

            if (string.IsNullOrEmpty(productName))
            {
                return null;
            }
            else
            {
                return productName;
            }
        }
    }
}