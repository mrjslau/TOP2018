using System.Collections.Generic;

namespace ShopLensApp.IO
{
    interface IWriter
    {
        /// <summary>
        /// Serialize JSON list of 'Item' objects to string.
        /// </summary>
        /// <param name="filePath">Path of the JSON file, where the string has to be saved.</param>
        /// <param name="items">List of items to be serialized.</param>
        void SerializeFromList(string filePath, List<ShoppingItem> items);
    }
}
