using System.Collections.Generic;

namespace ShopLensApp.IO
{
    interface IReader
    {
        /// <summary>
        /// Deserialize string of JSON format to the list of 'Item' objects.
        /// </summary>
        /// <param name="filePath">Path of the JSON file, where the string to be deserialized is to be located.</param>
        List<ShoppingItem> DeserializeToList(string filePath);
        string ReadText(string filePath);
    }
}
