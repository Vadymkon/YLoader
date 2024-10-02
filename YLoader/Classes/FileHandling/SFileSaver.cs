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
        public static void SaveVideosToJson(List<VideoFile> videos, string filePath, bool RewriteFile)
        {
            if (!RewriteFile)
            {
                videos.AddRange(SFileReader.LoadVideosFromJson());
            }

            // Converting
            string json = JsonConvert.SerializeObject(videos, Formatting.Indented);
            
            File.WriteAllText(filePath, json);
        }

        public static void SaveVideosToJson(List<VideoFile> videos, bool RewriteFile = true)
        {
            SaveVideosToJson(videos, SystemPaths.getSEOFilePath(), RewriteFile);
        }
        public static void SaveShortsToJson(List<VideoFile> videos, bool RewriteFile = true)
        {
            SaveVideosToJson(videos, SystemPaths.getSEOFilePath_Shorts(), RewriteFile);
        }

        public static void SaveGraphikToJson(Graphik graphik, string filePath)
        {
            List<VideoFile> videoFiles = SFileReader.LoadVideosFromJson(filePath);

            var buffer = graphik.queue.Select(x => Path.GetFileName(x)).Select(x => x.Replace(".mp4", "")).ToList();
            for (int i = 0; i < graphik.queue.Count; i++)
            {
                if (videoFiles.Any(y => y.FileName == buffer[i]))
                    videoFiles.First(y => y.FileName == buffer[i]).PublishedDate = graphik.queueDT[i];
                else videoFiles.Add(new VideoFile(graphik.queue[i], graphik.queueDT[i], 0));
            }

            SFileSaver.SaveVideosToJson(videoFiles, filePath, true);
        }

        public static void SaveGraphikToJson(Graphik graphik)
        {
            SaveGraphikToJson(graphik, SystemPaths.getSEOFilePath());
        }
        public static void SaveGraphikToJson_Shorts(Graphik graphik)
        {
            SaveGraphikToJson(graphik, SystemPaths.getSEOFilePath_Shorts());
        }
    }
}
