using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;


using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using BrightIdeasSoftware;
using System.Diagnostics;
using Microsoft.CSharp.RuntimeBinder;
using System.Configuration;
using YLoader.Properties;
using YLoader.Classes.API;
using YLoader.Classes;
using YLoader.Classes.FileHandling;

namespace YLoader
{
    public partial class Form1 : Form
    {
        String active_path = ""; 
        public YouTubeApi yt;
        public Form1()
        {

            InitializeComponent();
            SPathCheck.checkAllPaths();
            yt_Button2.Click += new System.EventHandler(yt_Button1_Click); //copy action
            
            //set active path
            active_path = Settings.Default["active_path"].ToString(); //
            if (active_path == "")
                active_path = Settings.Default["paths"].ToString().Split('|').ToList().Last(); //
            if (active_path == "")
                take_path();

            update_dropdownlist();
            
            CEO_settings(); //settings the table of CEO-data
            egoldsToggleSwitch1.Checked = Convert.ToBoolean(Settings.Default["open_form2"]); // check parametrs

            // oauth2.0
            yt = new YouTubeApi();


            //def description update
            if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + @"\def_descr.txt")) 
            {
            String def_descr = "\r\n";
            File.ReadAllLines(Path.GetDirectoryName(Application.ExecutablePath) + @"\def_descr.txt").ToList().ForEach(line => def_descr += $"{line}\r\n");
            Settings.Default["def_descr"] = def_descr;
            Settings.Default.Save();
            }

            //MakeGraphik();
        }

            MetaApi a;
        void button1_Click(object sender, EventArgs e) //TEST-Button
        {
            /*
            a = new MetaApi();

            a.Auth("gogokon.neko4@gmail.com", "pY]N852#");
            
            // a.Auth();
            a.UploadVideo(new VideoFile("AAAA_1.mp4"));
*/
            //UploadVideo(@"C:\Users\vadymkon\Desktop\test.mp4","test","test Description");
            //RunSomethink(); //upload test video
            /*yt.getListOfMyVideos();
            yt.UpdateVideo(new VideoFile("2",@"C:\Users\vadymkon\Desktop"));*/
            // yt.ThumbnailSetResponse(new VideoFile("2", @"C:\Users\vadymkon\Desktop"));
           // yt.UpdateVideoInfo(new VideoFile("obijmy", @"D:\vadymkon\youtube\НЕКАНОН\Готовый материал\CEO"));

            //MessageBox.Show("done");
        }


        private void button11_Click(object sender, EventArgs e)
        {
        }

        void button2_Click(object sender, EventArgs e) //save template of mails-sending
        {
            using (OpenFileDialog fileDialog = new OpenFileDialog()) //fileDialog
            {
                fileDialog.Title = "Choose directory to save example";
                fileDialog.ValidateNames = false;
                fileDialog.CheckFileExists = false;
                fileDialog.CheckPathExists = true;
                fileDialog.FileName = "mails_example.txt"; 
                fileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                
                DialogResult result = fileDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    string selectedFolder = Path.GetDirectoryName(fileDialog.FileName); 
                    string selectedFileName = Path.GetFileName(fileDialog.FileName).Replace(".txt","");

                    new MailStaff().SaveExample(selectedFolder,selectedFileName);
                }
            }
        }
        
        void yt_Button1_Click(object sender, EventArgs e) //main big button
        {
            if (!File.Exists(SystemPaths.getSEOFilePath())) ScheduleMaker.MakeGraphik();
            new Form2(this).Show();
        }
        
        private void yt_Button9_Click(object sender, EventArgs e) //refresh OAuth2.0
        {
            MessageBox.Show("Now several sites are going to open. \r\nPlease login in your chosen account \r\nfor managing video several times. \r\n\r\n(if you will login into different \r\naccounts - result of working is your deal :) )","ATTENTION!");
            yt.Refresh();
            yt = new YouTubeApi();
        }

        async void yt_Button5_Click(object sender, EventArgs e) //Mailing
        {
            using (OpenFileDialog fileDialog = new OpenFileDialog()) //fileDialog
            {
                fileDialog.Title = "Choose file for emailing.";
                fileDialog.ValidateNames = false;
                fileDialog.CheckFileExists = false;
                fileDialog.CheckPathExists = true;
                fileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

                DialogResult result = fileDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    string selectedFolder = Path.GetDirectoryName(fileDialog.FileName);
                    string selectedFileName = Path.GetFileName(fileDialog.FileName).Replace(".txt", "");

                    var a = new MailStaff();
                    a.readLetters(fileDialog.FileName);
                    var dialogResult = MessageBox.Show($"There are {a.Count()} letters.", "Are you sure?", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        await Task.Run(()=> { a.SendAll(); });
                        MessageBox.Show($"Sended {a.Count()} letters.");
                    }
                }
            }
        }

        async void yt_Button7_Click(object sender, EventArgs e) //MakeGraphik
        {
            await Task.Run(() => {
                ScheduleMaker.MakeGraphik();
                ScheduleMaker.MakeGraphikSh(DateTime.Now);
            }); 
        }

        void egoldsToggleSwitch1_CheckedChanged(object sender) //auto open form2
        {
            Settings.Default["open_form2"] = egoldsToggleSwitch1.Checked;
            Settings.Default.Save();
        } 

        void yt_Button8_Click(object sender, EventArgs e) //add source-folder
        {
            take_path();
            update_dropdownlist();
        } 

        void button3_Click(object sender, EventArgs e) //reset program settings
        {
            Settings.Default.Reset();
            MessageBox.Show("Please Reload Program");
            Close();
        } 

        void cmbStyle_SelectedIndexChanged(object sender, EventArgs e) //change active_path
        {
            active_path = cmbStyle.Items[cmbStyle.SelectedIndex].ToString();
            Settings.Default["active_path"] = active_path;
            Settings.Default.Save();
            Console.WriteLine(active_path);
            label2.Visible = false; linkLabel1.Visible = false;
            CEO_settings();
        }

        void yt_Button3_Click(object sender, EventArgs e) //shorts-GR
        {
            new Form2(this, true).Show();
        } 

        void Form1_Shown(object sender, EventArgs e) //auto open form2
        {
            if (egoldsToggleSwitch1.Checked) new Form2(this).Show(); // CheckBox action
        }

        void yt_Button6_Click(object sender, EventArgs e) //open custom GR file
        {
            using (OpenFileDialog fileDialog = new OpenFileDialog()) //fileDialog
            {
                fileDialog.Title = "Choose file of GRaffik.";
                fileDialog.ValidateNames = false;
                fileDialog.CheckFileExists = false;
                fileDialog.CheckPathExists = true;
                fileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

                DialogResult result = fileDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                   new Form2(this, pathToCustomGR: fileDialog.FileName).Show();
                    
                }
            }
        }

        void yt_Button10_Click(object sender, EventArgs e) //Thumbnails UPLOAD
        {
            var a = SFileReader.LoadVideosFromJson().Where(x => x.Id != "").ToList(); //get Id-in videos
            egoldsProgressBar1.ValueMaximum = a.Count; //progressbar settings
            
            a.ForEach(x => { yt.ThumbnailSetResponse(x); //thumbnails uploading
                ++egoldsProgressBar1.Value;
            });

        }

        void yt_Button4_Click(object sender, EventArgs e) //move next 1 month videos in near folder and open it
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Is there shorts?";
                folderDialog.SelectedPath = Settings.Default.sh_path.Trim() == "" ? Settings.Default.active_path : Settings.Default.sh_path;
                DialogResult result = folderDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    string selectedFolder = folderDialog.SelectedPath;
                    //save path
                    Settings.Default.sh_path = selectedFolder;
                    Settings.Default.Save(); 

                    Directory.CreateDirectory(selectedFolder + "\\monthNext"); //create

                    if (!File.Exists(SystemPaths.getSEOFilePath_Shorts())) ScheduleMaker.MakeGraphikSh( DateTime.Now ); //safety
                    Graphik a = new Graphik(SFileReader.LoadShortsFromJson()); //get GR object
                    int count = a.queueDT.Where(x => x <= DateTime.Now.AddDays(30)).ToList().Count; //how much videos
                    var filesIN = Directory.GetFiles(selectedFolder);
                    a.queue.GetRange(0, count).ForEach(x => {
                        if (filesIN.Contains(selectedFolder+"\\"+x + ".mp4"))
                        {
                            File.Move(selectedFolder + "\\" + x + ".mp4", selectedFolder + "\\monthNext" + "\\" + x + ".mp4"); //moving to this folder
                        }
                    });

                    Process.Start(selectedFolder+"\\monthNext"); // show folder
                        
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SMethods.saveIdsToSEO();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Process.Start(SystemPaths.getSEOPath());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Process.Start(Settings.Default.active_path);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //def description update
            String path = Path.GetDirectoryName(Application.ExecutablePath) + @"\def_descr.txt";
            if (!File.Exists(path))
                File.WriteAllText(path,"");
         
            String def_descr = "\r\n";
            File.ReadAllLines(Path.GetDirectoryName(Application.ExecutablePath) + @"\def_descr.txt").ToList().ForEach(line => def_descr += $"{line}\r\n");
            Settings.Default["def_descr"] = def_descr;
            Settings.Default.Save();
            Process.Start(path);
        }


        private void button9_Click(object sender, EventArgs e)
        {

            var videoFiles = SFileReader.LoadVideosFromJson(); // vFiles
            videoFiles = videoFiles.OrderBy(x => x.PublishedDate).ToList();
            //data
            String pathCEO = SystemPaths.getSEOPath();
            String saveGRdata = new Graphik(videoFiles).print();
            Directory.CreateDirectory(pathCEO);

            //saving
            SFileSaver.SaveVideosToJson(videoFiles); // save&print
            File.WriteAllText(SystemPaths.getHistoryNewGRFilePath(), saveGRdata); // save&print
            new Form2(this).Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            SMethods.removeNoIDSEO();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            SMethods.saveIdsToSEO_Shorts();
        }
    }


   
}