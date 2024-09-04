using BrightIdeasSoftware;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YLoader.Classes;
using YLoader.Properties;

namespace YLoader
{
    public static class SMethods
    {
        // TODO : Change this staff to JSON (make another class for it)
        /// <summary>
        /// Rewrite ALL CEO to empty template.
        /// </summary>
        /// <param name="path">pathWhereVideo</param>
        static public void makeCEO_forAllVideos(String path = "")
        {
            // for all videos make CEO
            if (path == "") path = Settings.Default["active_path"].ToString(); //default way
            String pathCEO = SystemPaths.getSEOFilePath(path);
            File.Create(pathCEO);

            SFileSaver.SaveVideosToJson(
                Directory.GetFiles(path)
                .Where(x => x.EndsWith(".mp4")) //for all videos
                .Select(x => new VideoFile(x.Replace(path + @"\", "")
                .Replace(".mp4", "")))
                .ToList()//save
            );
        }

        static public async Task saveIdsToSEO()
        {
            var a = new YouTubeApi();
            await a.getListOfMyVideos(); //videos from youtube

            var b = SFileReader.LoadVideosFromJson(); // vFiles

            b.ForEach(x =>
            {
                var videosComparing = a.own_videos.ToList().Where(
                    y => y.Snippet.Title == (x.Title != "" ? x.Title : "n)sk2") 
                      || y.Snippet.Title.formatOff() == (x.FileName.formatOff())).ToList();
                if (videosComparing.Count > 0) x.Id = videosComparing[0].Snippet.ResourceId.VideoId;
            }); //finding Id for this

            SFileSaver.SaveVideosToJson(b);
        }
        static public async Task saveIdsToSEO_Shorts()
        {
            // API
            var a = new YouTubeApi();
            await a.getListOfMyVideos(); //videos from youtube

            var b = SFileReader.LoadShortsFromJson(); // vFiles

            b.ForEach(x =>
            {
                var videosComparing = a.own_videos.ToList().Where(
                    y => y.Snippet.Title == (x.Title != "" ? x.Title : "n)sk2") 
                      || y.Snippet.Title.formatOff() == (x.FileName.formatOff())).ToList();
                if (videosComparing.Count > 0) x.Id = videosComparing[0].Snippet.ResourceId.VideoId;
            }); //finding Id for this

            SFileSaver.SaveShortsToJson(b);
        }

        static public async void removeNoIDSEO()
        {
            // clear IDs
            List<VideoFile> a = SFileReader.LoadVideosFromJson();
            a.ForEach(x => x.Id = "");
            SFileSaver.SaveVideosToJson(a);

            // get ID
            await saveIdsToSEO();
            
            //remove no id videos
            a = SFileReader.LoadVideosFromJson();
            string noIdVideos = string.Join(" ", a.Where(x => x.Id.Trim() == "").Select(x => x.FileName)); 
            var result = MessageBox.Show(noIdVideos , "Remove?" , MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                SFileSaver.SaveVideosToJson(a.Where(x => x.Id.Trim() != "").ToList());
            }
        }




            //
        }

    public partial class Form1 : Form
    {
        void CEO_settings()
        {
            //table settings
            List<VideoFile> videos = SFileReader.LoadVideosFromJson(); //list with videos
            //table
            objectListView1.GetColumn(0).AspectToStringConverter = delegate (object x)
            { //for all filenames
                String file = (String)x; //example of string
                return file + ".txt"; //make txt
            };
            objectListView1.FormatCell += olv1_FormatCell; //make green cells
            objectListView1.CellClick += olv1_Click; //make opening files
            objectListView1.SetObjects(videos.Where(x => x.IsHaveCEOfile)); //start data
            
            
            //for Videos Without SEO-files (right-bottom corner labels)
            List<string> videosFileNames = Directory.GetFiles(Settings.Default.active_path).ToList().Where(x => x.EndsWith(".mp4")).Select(x => Path.GetFileName(x)).Select(x => x.Replace(".mp4", "")).ToList();
            List<string> videosWithoutSEO = videosFileNames.Except(videos.Select(x => x.FileName)).ToList();
            List<VideoFile> videosFilesWithoutSEO = videosWithoutSEO.Select(x => new VideoFile(x)).ToList();

            if (videosFilesWithoutSEO.Count > 0) //if there are
            {
                label2.Visible = true;
                label2.Text = $"{videosFilesWithoutSEO.Count} videos without SEO at all."; //right-bottom corner message
                linkLabel1.Visible = true;
                linkLabel1.Click += new System.EventHandler((a, e) =>
                {
                    SFileSaver.SaveVideosToJson(videosFilesWithoutSEO, false);
                    table1Reload(videos);
                    label2.Text = "";
                    linkLabel1.Visible = false;
                    SMethods.saveIdsToSEO();
                }); //link-action
            }
        }
        /// <summary>
        /// Make green cells if Title is exists in VideoFile
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void olv1_FormatCell(object sender, FormatCellEventArgs e)
        {
            if (e.ColumnIndex == objectListView1.GetColumn(0).Index)
            {
                VideoFile video = (VideoFile)e.Model; //example
                if (video.Title != "") e.SubItem.BackColor = Color.LightGreen;
            }
        }
        /// <summary>
        /// Open .txt file with CEO of video
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void olv1_Click(object sender, CellClickEventArgs e)
        {
            if (e.ColumnIndex == objectListView1.GetColumn(0).Index)
            {
                VideoFile video = (VideoFile)e.Model; //get example of fule
                if (video != null)
                {
                    if (File.Exists(active_path + "\\CEO\\" + video.FileName + ".txt"))
                        Process.Start(active_path + "\\CEO\\" + video.FileName + ".txt"); //start process
                    if (Process.GetProcessesByName("notepad").Length == 2) Process.GetProcessesByName("notepad").ToList().First().Kill(); //this custom-table have problem with double action on ONLY one click. This is solution - kill one copy of file
                    table1ReloadTimer(); //reload table-list
                }
            }
        }

        /// <summary>
        /// Async reload
        /// </summary>
        async void table1ReloadTimer()
        {
            int scrollpos = objectListView1.LowLevelScrollPosition.Y; //scrollsave

            var process = Process.GetProcessesByName("notepad"); //target
            while (process.Any(x => !x.HasExited)) await Task.Delay(2000).ConfigureAwait(false); //is notepad still open

            Console.WriteLine("TABLE IS RELOADED ASYNC."); //logs
            table1Reload(SFileReader.LoadVideosFromJson()); //reset elements
            Invoke(new Action(() => //bcs thread-error
            {
                objectListView1.LowLevelScroll(0, scrollpos * 8); //scrollsave
            }));
        }

        void table1Reload(List<VideoFile> videos)
        {
            objectListView1.SetObjects(videos.Where(x => x.IsHaveCEOfile)); //reset all elements
        }

        internal void UpdateVideos(List<VideoFile> vf, bool Shorts = false)
        {
            VideoFile templateVideo = SFileReader.LoadTemplateVideo();

            Invoke((Action)(
               () =>
               {
                   vf = vf.Where(x => x.Id != "").ToList();
                   if (Shorts)
                   {
                       vf = vf.GetRange(0, vf.Count > 90 ? 90 : vf.Count);
                   }

                   label3.Visible = true;
                   egoldsProgressBar1.Value = 0;
                   egoldsProgressBar1.ValueMaximum = vf.Count;

                   int againer = 0; //

                   vf.ForEach(x =>
                   {
                       // template video 
                       if (x.Title.Trim() == "" || x.Title.Trim() == "TestTITLE") x.Title = templateVideo.Title;
                       if (x.Description.Trim() == "") x.Title = templateVideo.Description;

                       //stahdart update
                       int hours = 13;
                       x.PublishedDate = x.PublishedDate.AddHours(hours);
                       
                       yt.UpdateVideoInfo(x);
                       ++egoldsProgressBar1.Value;
                       label3.Text = $"Loaded {egoldsProgressBar1.Value}";
                   });
                   new Form2(this).Show();
               }));

        }


        public void take_path()
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Choose directory where videos for upload are.";

                DialogResult result = folderDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    string selectedFolder = folderDialog.SelectedPath;
                    Settings.Default["paths"] += "|" + selectedFolder;
                    Settings.Default["active_path"] = selectedFolder;
                    Settings.Default.Save();
                }
            }
        }

        void update_dropdownlist()
        {
            string[] a = Settings.Default["paths"].ToString().Split('|').ToList().Where(x => x.Trim() != "").ToArray(); //range of paths and no empty slots
            cmbStyle.Items.Clear();
            cmbStyle.Items.AddRange(a); //add
            cmbStyle.SelectedIndex = a.ToList().IndexOf(Settings.Default["active_path"].ToString()) != -1 ?
                a.ToList().IndexOf(Settings.Default["active_path"].ToString()) //active path
            : a.Length - 1; //last
        }
    }

    public partial class Form2 : Form
    {
        void table_settings()
        {
            //for DateTime converting in table
            objectListView1.GetColumn(1).AspectToStringConverter = delegate (object x) {
                DateTime date = (DateTime)x;
                return date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
            };
            objectListView1.FormatCell += olv1_FormatCell;
            objectListView1.FormatRow += olv1_FormatRow;
            objectListView1.ItemActivate += objectListView1_ItemActivate;
            objectListView1.ItemChecked += ObjectListView1_ItemChecked;
            objectListView1.CellRightClick += ObjectListView1_CellRightClick;


            reload_table();
            update_dropdownlist();
        }

        private void ObjectListView1_CellRightClick(object sender, CellRightClickEventArgs e)
        {
            if (videoFiles_buffer.Count != 0)
            {

                if (e.ColumnIndex == objectListView1.GetColumn(0).Index)
                {
                    VideoFile video = (VideoFile)e.Model; //get example of fule
                    if (video != null)
                    {
                        videoFiles.InsertRange(videoFiles.IndexOf(video) + 1, videoFiles_buffer); //add to this place all range
                        videoFiles_buffer.Clear();

                        using (var a = new Form1())
                        {
                            try
                            {
                                ScheduleMaker.SaveGRtoFile(videoFiles, egoldsGoogleTextBox2.Text.toDateTime());
                            }
                            catch
                            {
                                MessageBox.Show("write startdate correctly");
                                return;
                            }
                        }
                        reload_table();
                        yt_Button10_Click(this, e);
                    }
                }
            }
        }
        void olv1_FormatRow(object sender, FormatRowEventArgs e)
        {
            if (!isFormatted) return;

            VideoFile video = (VideoFile)e.Model; //example

            //format already PUBLIC videos
            if (parentForm.yt.own_videos.Where(x => x.Snippet.PublishedAt < DateTime.Now) //all public videos
                .Where(x => x.Snippet.Title.formatOff().Contains(video.Title != "" ? video.Title.formatOff() : "a2f)d") //if Title exists 
                || x.Snippet.Title.formatOff().Contains(video.FileName.formatOff()) || x.Id == video.Id) //which are in our collection
                .ToList().Count != 0) //if there this video 
            {
                e.Item.BackColor = Color.DarkGreen;
                return;
            }
            //format no public videos
            if (video.Id.Trim() == "") e.Item.BackColor = Color.FromArgb(220, 20, 60); //or just only on PC
            else e.Item.BackColor = Color.LightGreen; // but on YouTube (with Id)

        }
        void olv1_FormatCell(object sender, FormatRowEventArgs e)
        {
            if (!isFormatted) return;

            VideoFile video = (VideoFile)e.Model; //example
            List<Color> colorsInFormatRow = new List<Color>(new Color[] { Color.FromArgb(220, 20, 60) });
            //protect FormatRow
            if (colorsInFormatRow.Contains(e.Item.BackColor)) return;

            //format Dates
            if (parentForm.yt.own_videos.Where(x => x.Snippet.PublishedAt == video.PublishedDate) //all videos with same Pdate
                .Where(x => x.Snippet.Title.Contains(video.Title != "" ? video.Title : "a2f)d") //if Title exists 
                || x.Snippet.Title.Contains(video.FileName) || x.Id == video.Id) //which are in our collection
                .ToList().Count == 0) //if there this video 
            {
                e.Item.GetSubItem(1).BackColor = Color.FromArgb(220, 20, 60); //light red
            }
            else
                e.Item.GetSubItem(1).BackColor = Color.LightGreen; //if date correct


            //format Titles
            if (parentForm.yt.own_videos.Where(x => x.Id == video.Id) //all videos with same Pdate
                .Where(x => x.Snippet.Title.formatOff().Contains(video.Title != "" ? video.Title.formatOff() : "a2f)d")) //if Title exists 
                .ToList().Count == 0) //if there this video 
            {
                e.Item.BackColor = Color.Orange; //light red
            }
            else
                e.Item.BackColor = Color.LightGreen; //if title correct

        }

        /*
       //table staff
       void olv1_FormatCell(object sender, FormatCellEventArgs e)
       {
           if (e.ColumnIndex == objectListView1.GetColumn(0).Index)
           {
               VideoFile video = (VideoFile)e.Model; //example
               if (video.Title != "") e.SubItem.BackColor = Color.LightGreen;
           }
       }
*/
        private void objectListView1_ItemActivate(object sender, EventArgs e)
        {
            foreach (OLVListItem olvi in objectListView1.SelectedItems)
            {
                olvi.Checked = !olvi.Checked;
                Console.WriteLine(olvi.RowObject.ToString());
            }
        }
        private void ObjectListView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            Console.WriteLine(e.Item.Checked + e.Item.Text);
            if (e.Item.Checked)
            {
                videoFiles_buffer.Add(videoFiles.First(x => x.FileName == e.Item.Text || x.Title == e.Item.Text)); //in buffer add
                videoFiles.Remove(videoFiles.First(x => x.FileName == e.Item.Text || x.Title == e.Item.Text)); //remove from gen list
            }
            else //false
            {
                videoFiles.Add(videoFiles_buffer.First(x => x.FileName == e.Item.Text || x.Title == e.Item.Text)); //add to gen list
                videoFiles_buffer.Remove(videoFiles_buffer.First(x => x.FileName == e.Item.Text || x.Title == e.Item.Text)); //remove in buffer 
            }

        }

        async void reload_table()
        {
            //   Invoke((Action)(() => { 
            //   }));
            //update own_videos
            await parentForm.yt.getListOfMyVideos();
            //get Graphik infoInvoke
            Graphik a;
            if (CustomGR.Trim() != "")
                a = new Graphik(SFileReader.LoadVideosFromJson(CustomGR)); //in case where we gonna watch custom GR
            else
            {

                if (Shorts == false)
                {
                    a = new Graphik(SFileReader.LoadVideosFromJson());
                }
                else
                {
                    if (!File.Exists(SystemPaths.getSEOFilePath_Shorts()))
                        Task.Run(() => { ScheduleMaker.MakeGraphikSh((DateTime.Now)); }).Wait(); //if GR-SH doesn't exist - improve it
                    a = new Graphik(SFileReader.LoadShortsFromJson());
                }
            }


            videoFiles = a.GetVideoFiles();
            objectListView1.SetObjects(videoFiles); //put VideoFiles-info to table

            DateTime dateForField = DateTime.Now; 
            if (a.queueDT.Count > 0) dateForField = a.queueDT[0];
            egoldsGoogleTextBox2.Text = dateForField.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture); //change textbox // that's comforable to set it here
            egoldsGoogleTextBox1.Text = dateForField.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture); //change textbox // that's comforable to set it here
            egoldsGoogleTextBox3.Text = 3.ToString(); 
        }



        void update_dropdownlist()
        {
            string[] a = Settings.Default["paths"].ToString().Split('|').ToList().Where(x => x.Trim() != "").ToArray(); //range of paths and no empty slots
            cmbStyle.Items.Clear();
            cmbStyle.Items.AddRange(a); //add
            cmbStyle.SelectedIndex = a.ToList().IndexOf(Settings.Default["active_path"].ToString()) != -1 ?
                a.ToList().IndexOf(Settings.Default["active_path"].ToString()) //active path
            : a.Length - 1; //last
        }


        private void egoldsToggleSwitch1_CheckedChanged(object sender)
        {
            isFormatted = egoldsToggleSwitch1.Checked;
            objectListView1.BuildList();
        }

        private void egoldsToggleSwitch2_CheckedChanged(object sender)
        {
            objectListView1.GetColumn(0).AspectName = egoldsToggleSwitch2.Checked ? "Title" : "FileName";
            objectListView1.BuildList();
        }
    }
}
