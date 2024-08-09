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
    public class Graphik
    {
        public List<String> queue = new List<string>();
        public List<DateTime> queueDT = new List<DateTime>();
        DateTime startDate;
        public Graphik(int value)
        {
            //making list of Graphik
            for (int i = 0; i<value; i++) queue.Add("");
        }

        public Graphik(List <VideoFile> videoFiles)
        {
            videoFiles.ForEach(x => queue.Add(x.FileName));
            videoFiles.ForEach(x => queueDT.Add(x.PublishedDate));
        }

        public List<VideoFile> GetVideoFiles()
        {
            List<VideoFile> videoFiles = new List<VideoFile>();
            List<VideoFile> existringVideoFiles = SFileReader.LoadVideosFromJson();

            queue.ForEach(x =>
            {
                if (existringVideoFiles.Any(y => y.FileName == x.Trim()))
                    videoFiles.Add(existringVideoFiles.First(y => y.FileName == x.Trim()));
                else
                {
                    var a = new VideoFile(x.Trim());
                    a.PublishedDate = queueDT[queue.IndexOf(x)]; // TODO: here can be problems
                    videoFiles.Add(a);
                }
            });
            return videoFiles;
        }

        public void insert(List<String> lines, String inDate)
        {
            queue.InsertRange(queueDT.IndexOf(FindClosestDate(queueDT, inDate.toDateTime())), lines);
            for (int i = 0; i < lines.Count; i++) queueDT.Add(queueDT.Last().AddDays(3));
        }
        
        public List <VideoFile> getInsertedVideoFiles ()
        {
            List<VideoFile> videos = GetVideoFiles();
            //corrective dates
            videos.ForEach(x => { 
            if (queue.Contains(x.FileName) || queue.Contains(x.Title))
                {
                    int index = queue.IndexOf(x.FileName);
                    if (index == -1)
                        index = queue.IndexOf(x.Title); //safety
                    x.PublishedDate = queueDT[index];
                }
            });

            return videos;
        }

        static DateTime FindClosestDate(List<DateTime> dateList, DateTime targetDate)
        {
            DateTime closestDate = dateList.OrderBy(d => Math.Abs((d - targetDate).Ticks)).First();
            return closestDate;
        }
    

    public void newStartDate(DateTime NewstartDate, int amountPerDay = 0)
        {
            startDate = NewstartDate;

            if (amountPerDay != 0)
            {
                for (int i = 0; i < queueDT.Count; i++)
                {
                    if (i != 0 && i%amountPerDay == 0) NewstartDate = NewstartDate.AddDays(1); // new date 
                    queueDT[i] = NewstartDate.AddHours(10 + 14/amountPerDay*(i%amountPerDay) ); // 3 days mode qDT
                }
            }
        }

        public int putElementOnIndex(int index, String element)
        {
            if (index >= queue.Count) { queue.Add(element); return -2; } //if jumped out put and say about rangeout
            if (queue[index] != "") return -1; //if it putted by other video
            queue[index] = element; //put element on que
            return 0; 
        }

        public void remove_spaces()
        {
            queue.RemoveAll(x => x == "");
        }

        public String print_ForEvery3days(DateTime startDate)
        {
            remove_spaces();
            String message = "  Graphik made by YT Uploader\r\n\r\n";
            if (queueDT == null || queueDT.Count == 0)
                queue.ForEach(x => queueDT.Add(startDate.AddDays(3 * queue.IndexOf(x))));
            // queue.ForEach(x => message += $"{startDate.AddDays(3*queue.IndexOf(x)).ToString("dd.MM.yyyy", CultureInfo.InvariantCulture)} : {x.Replace("mp4","")}\r\n");
            
            queue.ForEach(x => message += $"{queueDT[queue.IndexOf(x)].ToString("dd.MM.yyyy", CultureInfo.InvariantCulture)} : {x.Replace(".mp4","")}\r\n");

            return message;
        }

        public String print()
        {
            remove_spaces();
            String message = "  Graphik made by YT Uploader\r\n\r\n";
            queue.ForEach(x => message += $"{queueDT[queue.IndexOf(x)].ToString("dd.MM.yyyy", CultureInfo.InvariantCulture)} : {x.Replace(".mp4","")}\r\n");
            return message;
        }


    }
}
