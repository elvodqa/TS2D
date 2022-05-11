using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace TS2D.Util
{
    public class Map
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        public string Author { get; set; } = "Unknown";
        public string Version { get; set; } = "0.0.1";
        public string Description { get; set; } = "A nice map";
    
        public int[,] TileMap { get; set; } = new int[30, 30];
        public int[,] EntityMap { get ;set;} = new int[30, 30];
        
       
        
        public Map(string name)
        {
            Name = name;
            FileName = Name + ".json";
            if (File.Exists("Maps/" + FileName))
            {
                var mapJson = JObject.Parse(File.ReadAllText($"./Maps/{FileName}"));
                //TileMap = mapJson["TileMap"]?.ToObject<int[,]>();
                TileMap = mapJson.GetValue("TileMap")?.ToObject<int[,]>();
                EntityMap = mapJson["EntityMap"]?.ToObject<int[,]>();
            }
            else
            {
                using (StreamWriter file = File.CreateText($"./Maps/{FileName}"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Formatting = Formatting.Indented;
                    serializer.Serialize(file, this);
                }
            }
        }

        public void Save(int[,] tMap, int[,] eMap)
        {
            TileMap = tMap;
            EntityMap = eMap; 
            
            string json = File.ReadAllText($"./Maps/{FileName}");
            JObject map = JObject.Parse(json);
            JArray mapTile = (JArray)map["TileMap"];
            mapTile.Replace(JToken.FromObject(tMap));
            Console.WriteLine(map);
            File.WriteAllText($"./Maps/{FileName}", map.ToString());
            /*
            string json = File.ReadAllText($"./Maps/{FileName}");
            dynamic mapJson = JsonConvert.DeserializeObject(json);
            if (mapJson != null)
            {
                mapJson["TileMap"] = TileMap;
                mapJson["EntityMap"] = EntityMap;
                string output =
                    JsonConvert.SerializeObject(mapJson, Formatting.Indented);
                File.WriteAllText($"./Maps/{FileName}", output);
            }*/
        }
        
        
    }
}