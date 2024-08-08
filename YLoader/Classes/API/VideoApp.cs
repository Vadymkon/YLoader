using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YLoader.Classes.API
{
    public interface VideoApp
    {
        // string Name { get; set; }

        void Auth(string login, string password);
        void Disconnect();
        void UploadVideo(VideoFile vFile);
        void UpdateVideoInfo(VideoFile vFile);
    }
    
    public class VideoAppClient : VideoApp
    {
        public VideoApp VA { get; set; }

       // public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
       
        public VideoAppClient(VideoApp videoApp)
        {
            this.VA = videoApp;
        }

        public void Auth(string login, string password)
        {
            this.VA.Auth(login, password);
        }

        public void UpdateVideoInfo(VideoFile vFile)
        {
            this.VA.UpdateVideoInfo(vFile);
        }

        public void UploadVideo(VideoFile vFile)
        {
            this.VA.UploadVideo(vFile);
        }

        public void Disconnect()
        {
            this.VA.Disconnect();
        }
    }
}
