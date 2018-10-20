using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace ShopLensApp.IO
{
    /// <summary>
    /// Class to write data to a file
    /// </summary>
    public class JsonWriter : IWriter
    {
        /// <inheritdoc cref="IWriter.SerializeFromList(string, List{Item})"/>
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
