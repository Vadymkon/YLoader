using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YLoader.Properties;

namespace YLoader
{
    public partial class Form1 : Form
    {
        //Graphik Staff (Shorts)
        internal void MakeGraphikSh(DateTime startDate)
        {
            List<Playlist> playlists = new List<Playlist>();
            int countVideos;
            Graphik graphik;

            List<VideoFile> longlist = getVideoListFromSEO(Settings.Default["active_path"].ToString()); //get long list
            longlist.ForEach(video => playlists.Add(new Playlist(video.FileName, video.PublishedDate))); //get groups of video

            String path = Settings.Default["active_path"].ToString() + "\\shorts"; //path to shorts video
            if (Settings.Default["active_path"].ToString().Contains("shorts")) { MessageBox.Show("Dont use shorts-directory as main"); return; }
            if (!Directory.Exists(path)) { MessageBox.Show("For Shorts make \\shorts directory in the long-videos dir"); return; }
            List<String> notUsedVideos = new List<string>();

            //Part 1 - GROUPING
            Directory.GetFiles(path).Where(file => file.EndsWith(".mp4")).ToList().ForEach(
                video => {
                    try //if this is part of long
                    {
                        playlists.First(pl => video.Contains(pl.NameOfPlaylist)).push_back(video); //in first playlist with similar name push video
                    }
                    catch(SystemException e)
                    {
                        notUsedVideos.Add(video);
                    }
                    }
                );



            //Part 2 - get pairs
            Dictionary<DateTime, List<String>> pairs = new Dictionary<DateTime, List<string>>(); //here pairs date - videos 
            startDate = startDate.AddDays(-10);
            for (int i = 0; i < 750; i++) pairs.Add(startDate.AddDays(i), new List<String>()); //put for 2 years next empty slots

            //Part 3 - put to DateTime
            playlists.ForEach(pl =>
            {
                // first 4
                if (pl.Count() != 0)
                    pairs[pl.date4SHORTS.AddDays(-1)].Add(pl.get_elem());  //first [sh] to day before
                if (pl.Count() != 0)
                    pairs[pl.date4SHORTS].Add(pl.get_elem());   // [sh] to day of publication
                if (pl.Count() != 0)
                    pairs[pl.date4SHORTS.AddDays(1)].Add(pl.get_elem());  // [sh] to day after
                if (pl.Count() != 0)
                    pairs[pl.date4SHORTS.AddDays(3)].Add(pl.get_elem());  // [sh] to 3 days after

                if (pl.Count() != 0)
                    pairs[pl.date4SHORTS].Add(pl.get_elem());

                if (pl.Count() != 0)
                    pairs[pl.date4SHORTS.AddDays(2)].Add(pl.get_elem());  // [sh] to day after

                int plCount = pl.Count();
                for (int i = 0; i < plCount; i++)
                    pairs[pl.date4SHORTS.AddDays(3 + 1 + i)].Add(pl.get_elem());

            });


            //Part 4 - Where more than 3 [sh] - move it to next
            int whiletry = 0;
            pairs.ToList().ForEach(pair =>
           {
               whiletry = 0;
//               whiletry++;
               while (pair.Value.Count > 3)
               {
                   if (pairs[pair.Key.AddDays(whiletry)].Count < 3) //and next day is 
                   {
                       pairs[pair.Key.AddDays(whiletry)].Add(pair.Value.Last()); //try to add to next list
                       pair.Value.Remove(pair.Value.Last()); //remove it from list
                   }
                   else whiletry++; //or go next
               }
           });

            //Part 5 - not used [sh] to GR
            whiletry = 0;
            pairs.ToList().ForEach(pair =>
           {
               while (pair.Value.Count < 3)
               {
                   if (notUsedVideos.Count == 0) break;
                       pair.Value.Add(notUsedVideos.First()); //try to add to next list
                       notUsedVideos.RemoveAt(0);
               }
           });

            //saving
            String message = "";
            String pathCEO = Path.GetDirectoryName(Application.ExecutablePath) + "/GR_history";
            Directory.CreateDirectory(pathCEO);
            pairs.ToList().ForEach(pair => pair.Value.ForEach(videoname => message += $"{pair.Key.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture)} : {Path.GetFileName(videoname)}\r\n"));
            File.WriteAllText(pathCEO + $"/GR_sh_{Directory.GetFiles(pathCEO).Length + 1}.txt", message);
            File.WriteAllText(pathCEO + $"/_graphik_SH.txt", message);
        }
    }
}
