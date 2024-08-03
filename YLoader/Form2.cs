using BrightIdeasSoftware;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YLoader.Properties;

namespace YLoader
{
    public partial class Form2 : Form
    {
        public bool Shorts { get; }
        List<VideoFile> videoFiles = new List<VideoFile>();
        List<VideoFile> videoFiles_buffer = new List<VideoFile>();
        String CustomGR = "";
        Form1 parentForm;
        bool isFormatted = false;

        internal Form2(Form1 form1, bool shorts = false, String pathToCustomGR = "")
        {
            InitializeComponent();
            Shorts = shorts;
            parentForm = form1;
            parentForm.yt.getListOfMyVideos();

            table_settings(); 
            CustomGR = pathToCustomGR;

            if (Shorts) //hide panel
            {
                panel2.Visible = false;
                objectListView1.Width = (int)(Width * 0.9 );
            }
        }

        private void yt_Button13_Click(object sender, EventArgs e) //Shift GR
        {
            if (label1.Visible == false)
            {
                DialogResult result = MessageBox.Show("Вы собираетесь\r\nдобавлять новые видео?", "Вопросикос", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    new Form3(parentForm, egoldsGoogleTextBox2.Text).Show();
                    Close();
                    return;
                }
            }
            
            objectListView1.CheckBoxes = !objectListView1.CheckBoxes;
            label1.Visible = !label1.Visible;
        } 

        private void yt_Button8_Click(object sender, EventArgs e) //add videos to GR
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Choose videofiles:";
            openFileDialog.Filter = "Videofiles (*.mp4; *.avi; *.mkv)|*.mp4;*.avi;*.mkv|All Files (*.*)|*.*";
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                ScheduleMaker.MakeGraphik(openFileDialog.FileNames.ToList().Select(x => Path.GetFileName(x)).ToList(),true); //files into
                
                string selectedFolder = Path.GetDirectoryName(openFileDialog.FileName);
                if (!Settings.Default["paths"].ToString().Contains(selectedFolder))
                { 
                    //reload paths
                    Settings.Default["paths"] += "|" + selectedFolder;
                    Settings.Default["active_path"] = selectedFolder;
                    Settings.Default.Save();
                    update_dropdownlist();
                }
                //reload table
                reload_table();
            }
            
        }

        private void yt_Button11_Click(object sender, EventArgs e) //add source folder
        {
            using (Form1 a = new Form1()) a.take_path();
            update_dropdownlist();
        }

        public void yt_Button10_Click(object sender, EventArgs e) //correct dates
        {
            try
            {
                egoldsGoogleTextBox2.Text.toDateTime(); //is there norm format
            }
            catch
            {
                MessageBox.Show("Put date correctly");
                return;
            }
            Graphik b = new Graphik(Path.GetDirectoryName(Application.ExecutablePath) + @"\GR_history\_graphik.txt");
            b.newStartDate(egoldsGoogleTextBox2.Text.toDateTime());
            //data
            String pathCEO = Path.GetDirectoryName(Application.ExecutablePath) + "/GR_history";
            String saveGRdata = b.print_ForEvery3days(egoldsGoogleTextBox2.Text.toDateTime());
            //saving
            Directory.CreateDirectory(pathCEO);
            File.WriteAllText(pathCEO + "/_graphik.txt", saveGRdata); // save&print
            File.WriteAllText(pathCEO + $"/GR_{Directory.GetFiles(pathCEO).Length}.txt", saveGRdata); // save&print
            b.writeDatesToCEO( 
                Settings.Default["active_path"].ToString() + @"\CEO", 
                Path.GetDirectoryName(Application.ExecutablePath) + @"\GR_history\_graphik.txt");
            
            // Make NEW SH-GR
            ScheduleMaker.MakeGraphikSh(egoldsGoogleTextBox2.Text.toDateTime());
            // Update table
            objectListView1.SetObjects(b.GetVideoFiles());
        } 

        private void yt_Button12_Click(object sender, EventArgs e) //open shorts-GR
        {
            new Form2(parentForm, true).Show();
        }

        private void yt_Button3_Click(object sender, EventArgs e) //Realize GR
        {
            if (videoFiles.Where(x => x.Id != "").ToList().Count == 0)
            {
                MessageBox.Show("если вам охота загрузить видео, \r\nто загрузите просто файлики на ютуб, \r\nи уже потом нажимайте реализовать");
                return;
            }
            parentForm.UpdateVideos(videoFiles,Shorts); //update all
            Close();
        }

    }
}
