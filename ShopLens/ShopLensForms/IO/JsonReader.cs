using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace ShopLensApp.IO
{
    /// <summary>
    /// Class to read data from a file
    /// </summary>
    public class JsonReader : IReader
    {
        /// <inheritdoc cref="IReader.DeserializeToList(string)"/>
        public List<ShoppingItem> DeserializeToList(string filePath)
        {
            string jsonString = ReadText(filePath);
            List<ShoppingItem> list = JsonConvert.DeserializeObject<List<ShoppingItem>>(jsonString);
            return list ?? new List<ShoppingItem>();
        }

        /// <inheritdoc cref="IReader.ReadText(string)"/>
        public string ReadText(string filePath)
        {
            string text = "";
            if (File.Exists(filePath))
            {
                text = File.ReadAllText(filePath);
            }
            return text;
        }
    }
}
