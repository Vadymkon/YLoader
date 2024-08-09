using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YLoader.Classes;
using YLoader.Properties;

namespace YLoader
{
    public partial class Form3 : Form
    {
        Form1 parentForm;
        public Form3(Form1 form1, string sDate)
        {
            InitializeComponent();
            egoldsGoogleTextBox1.Text = sDate;
            parentForm = form1;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Choose videofiles:";
            openFileDialog.Filter = "Videofiles (*.mp4; *.avi; *.mkv)|*.mp4;*.avi;*.mkv|All Files (*.*)|*.*";
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                List <String> videoFiles = openFileDialog.FileNames.ToList().Select(x => Path.GetFileName(x)).ToList();
                videoFiles.ForEach(line => textBox1.AppendText($"{line}\r\n"));

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Form2(parentForm).Show();
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "")
            {
                MessageBox.Show("Please choose videofiles.");
                return;
            }

            List<string> lines = textBox1.Text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList(); //get lines from textbox
            Graphik gr = new Graphik(SFileReader.LoadVideosFromJson()); //get graphik
            gr.insert(lines, egoldsGoogleTextBox1.Text); //insert this videos
            ScheduleMaker.SaveGRtoFile(gr.getInsertedVideoFiles(), DateTime.Now); //save GRaffik

            //change form
            new Form2(parentForm).Show();
            Close();
        }
    }
}
