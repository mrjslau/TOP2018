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
    public class ReaderJSON : IReader
    {
        /// <inheritdoc cref="IReader.DeserializeToList(string)"/>
        public List<Item> DeserializeToList(string filePath)
        {
            string jsonString;
            try
            {
                jsonString = File.ReadAllText(filePath);
            }
            catch (FileNotFoundException e)
            {
                File.Create(filePath);
                using (var f = File.OpenRead(filePath))
                {
                    jsonString = File.ReadAllText(filePath);
                }
            }
            List<Item> list = JsonConvert.DeserializeObject<List<Item>>(jsonString);
            return list;
        }
    }
}
