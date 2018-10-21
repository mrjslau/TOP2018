using System.Collections.Generic;

namespace ShopLensApp.IO
{
    interface IReader
    {
        /// <summary>
        /// Deserialize string to the list of 'Item' objects.
        /// </summary>
        /// <param name="filePath">Path of the file, where the string to be deserialized is located.</param>
        List<ShoppingItem> DeserializeToList(string filePath);
        /// <summary>
        /// Reads the text of the file and returns it as a string value.
        /// </summary>
        /// <param name="filePath">Path of the file.</param>
        string ReadText(string filePath);
    }
}
