using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YLoader.Classes;
using YLoader.Properties;

namespace YLoader
{
    public class VideoFile
    {
        //types are made for Video the class of YT lib
        public string FileName { get; set; } = "";
        public string Title { get; set; } = "TestTITLE";
        public string Description { get; set; } = "";
        public string[] Tags { get; set; } = new string[] { };
        public string CategoryID { get; set; } = "22";
        public DateTime PublishedDate { get; set; } = new DateTime();
        public string Id { get; set; } = "";
        public string PathToVideo { get; set; } = "";

        public bool Uploaded { get; set; } = false;
        public bool Published { get; set; } = false;
        public bool IsHaveCEOfile { get; set; } = false;


        public VideoFile() { }
        public VideoFile(String filename)
        {
            FileName = filename;
        }
        public VideoFile(String filename, DateTime date, int hours)
        {
            FileName = filename;
            PublishedDate = date.AddHours(hours);
        }

        public void maximumSymbols (String pathToFile)
        {
            // Safety existings
            if (!File.Exists(pathToFile)) return;

            List<string> tags_buffer = Tags.ToList();
            while (string.Join(" ", tags_buffer).Length > 470)  // clear unused tags
                tags_buffer.Remove(tags_buffer.Last());

            Description = Description.Replace("{descr}", Settings.Default.def_descr);    
            Description = Description.Replace("<", "");
            Description = Description.Replace(">", "");
            Title = Title.Replace(">", "");
            Title = Title.Replace(">", "");
            if (Title.Length >= 99) Title = Title.Substring(0, 99);
        }
    }
}
