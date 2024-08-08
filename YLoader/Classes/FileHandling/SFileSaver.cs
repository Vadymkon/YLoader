using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YLoader.Classes
{
    public static class SFileSaver
    {
        // Save to JSON
        public static void SaveVideosToJson(List<VideoFile> videos, string filePath)
        {
            // Converting
            string json = JsonConvert.SerializeObject(videos, Formatting.Indented);

            File.WriteAllText(filePath, json);
        } 
        
        public static void SaveVideosToJson(List<VideoFile> videos)
        {
            SaveVideosToJson(videos, SystemPaths.getSEOPath());
        }

       
    }
}
