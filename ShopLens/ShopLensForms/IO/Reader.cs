using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace ShopLensApp.IO
{
    public class Reader
    {       
        public List<Item> DeserializeToList(string filePath)
        {                       
            string jsonString = File.ReadAllText(filePath);      
            List<Item> lst = JsonConvert.DeserializeObject<List<Item>>(jsonString);
            return lst;
        }
    }
}
