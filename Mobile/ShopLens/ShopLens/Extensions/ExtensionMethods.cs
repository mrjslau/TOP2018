using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShopLens.Extensions
{
    static class ExtensionMethods
    {
        public static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                case null:
                    throw new ArgumentNullException("Null value was passed.");

                case "":
                    return string.Empty;

                default:
                    return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }
    }
}
