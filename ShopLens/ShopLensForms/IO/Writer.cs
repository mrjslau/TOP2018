using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace ShopLensApp.IO
{
    /// <summary>
    /// Class to write data to a file
    /// </summary>
    public class Writer
    {
        /// <summary>
        /// Serialize list of 'Item' objects to string.
        /// </summary>
        /// <param name="filePath">Path of the file, where the string has to be saved.</param>
        /// <param name="items">List of items to be serialized.</param>
        public void SerializeFromList(string filePath, List<Item> items)
        {
            using (StreamWriter file = File.CreateText(filePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, items);
            }
        }
    }
}