using System.Collections.Generic;
using System.IO;

namespace TS2D.Util
{
    public static class MapManager
    {
        public static Dictionary<string, Map> Maps = new Dictionary<string, Map>();

        public static void Load(string path)
        {
            if (File.Exists("./Maps/" + path + ".json"))
            {
                //string[] lines = File.ReadAllLines("./Maps/" + path + ".map");



            }
        }
    }
}