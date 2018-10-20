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
        public List<Item> DeserializeToList(string filePath)
        {
            string jsonString = ReadText(filePath);
            List<Item> list = JsonConvert.DeserializeObject<List<Item>>(jsonString);
            return list;
        }

        public string ReadText(string filePath)
        {
            string text;
            if (!File.Exists(filePath))
            {
                using (File.Create(filePath))
                {

                }
            }
            text = File.ReadAllText(filePath);
            using (var f = File.OpenRead(filePath))
            {
                text = File.ReadAllText(filePath);
            }
            return text;
        }
    }
}
