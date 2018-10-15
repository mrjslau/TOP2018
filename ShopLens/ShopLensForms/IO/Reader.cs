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
    public class Reader
    {
        /// <summary>
        /// Deserialize string of JSON format to the list of 'Item' objects.
        /// </summary>
        /// <param name="filePath">Path of the file, where is the string to be deserialized.</param>
        public List<Item> DeserializeToList(string filePath)
        {                       
            string jsonString = File.ReadAllText(filePath);      
            List<Item> list = JsonConvert.DeserializeObject<List<Item>>(jsonString);
            return list;
        }
    }
}
