using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YLoader.Properties;

namespace YLoader.Classes
{
    public static class SFileReader
    {
        // Read from JSON
        public static List<VideoFile> LoadVideosFromJson(string filePath)
        {   
            // Reading
            string json = File.ReadAllText(filePath);
            // Convert
            List<VideoFile> videos = JsonConvert.DeserializeObject<List<VideoFile>>(json);
            return videos ?? new List<VideoFile>();
        }

        public static VideoFile LoadVideo(string fileName, string filePath)
        {
            // Reading
            string json = File.ReadAllText(filePath);
            // Convert
            List<VideoFile> videos = JsonConvert.DeserializeObject<List<VideoFile>>(json);
            if (videos.Where(x => x.FileName == fileName).ToList().Count == 0) throw new Exception("Now such video here.");
            return videos.First(x => x.FileName == fileName);
        }
        // Read from JSON
        public static List<VideoFile> LoadVideosFromJson()
        {
            return LoadVideosFromJson(SystemPaths.getSEOFilePath());
        }

        public static VideoFile LoadVideo(string fileName)
        {
            return LoadVideo(fileName, SystemPaths.getSEOFilePath());
        }
        
        public static List<VideoFile> LoadShortsFromJson()
        {
            return LoadVideosFromJson(SystemPaths.getSEOFilePath_Shorts());
        }
        public static VideoFile LoadTemplateVideo()
        {
            List<VideoFile> a = LoadVideosFromJson(SystemPaths.getSEOTemplateFile());
            //safety
            if (a.Count <= 1)
            {
                a.Add(new VideoFile("template")); // template short-data
                SFileSaver.SaveVideosToJson(a,SystemPaths.getSEOTemplateFile(),true);
                throw new Exception("Please write a template video file.");
            }
            return a.First();
        }


    }
}
