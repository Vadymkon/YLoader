using Facebook;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

using PuppeteerExtraSharp;

namespace YLoader.Classes.API
{
    public class MetaApi : VideoApp
    {
        public BrowserPuppetter browser;
        public MetaApi() {
            browser = new BrowserPuppetter();
        }

        public async void Auth(string login, string password)
        {
            String metaVideos = "https://business.facebook.com/latest/reels_composer";
            await browser.OpenPage(metaVideos);


            await browser.pages.First(x => x.URL.Contains("business.facebook.com"))
             .ClickOn(
             "x1n2onr6.x1ja2u2z.x78zum5.x2lah0s.xl56j7k.x6s0dn4.xozqiw3.x1q0g3np.x9f619.xi112ho.x17zwfj4.x585lrc.x1403ito" +
             ".x1qhmfi1.x1s9qjmn.x39innc.x7gj0x1.x1mpseq2.x13fuv20.xu3j5b3.x1q0q8m5.x26u7qi.x178xt8z.xm81vs4.xso031l.xy80clv.x1r1pt67.x8ikho7.x1xdidmb");

            await browser.pages.First(x => x.URL.Contains("business.facebook.com")).WriteOnInput(login,
                "email.inputtext._55r1.inputtext._1kbt.inputtext._1kbt"
                );

            await browser.pages.First(x => x.URL.Contains("business.facebook.com")).WriteOnInput(password,
                "pass.inputtext._55r1.inputtext._9npi.inputtext._9npi"
                );

            await browser.pages.First(x => x.URL.Contains("business.facebook.com"))
             .ClickOn(
             "_42ft._4jy0._52e0._4jy6._4jy1.selected._51sy", "button");

            await browser.pages.First(x => x.URL.Contains("business.facebook.com"))
             .ClickOn(
             "x1i10hfl.xjqpnuy.xa49m3k.xqeqjp1.x2hbi6w.x972fbf.xcfux6l.x1qhh985.xm0m39n.x9f619.x1ypdohk" );

        }

        public void Disconnect()
        {
            browser.Close();
        }

        public void UpdateVideoInfo(VideoFile vFile)
        {
            
        }

        public void UploadVideo(VideoFile vFile)
        {
          }


}


}

/*
 
String _appAccessToken = "";
        string page_id = "";

        String accessToken = "EAAODlBm2GP4BO2iKyedhznYWaDfU62G7zROewDcEHGzYU18GIzD2hnT00OAFzZAa5ZBPM1LoS6tZCuY9qsWPKEhmeaPVCFW8G1B3wvqfDVWZAiXBhfLnUv1fNyDloldoV6842Xl7wR9G3TjkW14bPkpwYDq3DZBMyXGHZBF50TAh7SButEPSY7UF73vGZBKQjbMZBDaRXVTsnSj9ceRw4oJ4vLt7d1OZBsViqyXIz5Ag8mwZDZD";
        public string AppSecret = "d27ff3377049c110f701ebeb23d3989a";
        public string AppId = "989097039894782";

        public void Auth()
        {
            var client = new FacebookClient
            {
                AppId = this.AppId, // get this from developer.facebook
                AppSecret = this.AppSecret, // get this from developer.facebook
            };

            dynamic appTokenQueryResponse = client.Get("oauth/access_token"
                                                        , new
                                                        {
                                                            client_id = AppId,
                                                            client_secret = AppSecret,
                                                            grant_type = "client_credentials"
                                                        });

            _appAccessToken = appTokenQueryResponse.access_token;



            FacebookClient fb = new FacebookClient(accessToken);
            dynamic parameters = new ExpandoObject();
            parameters.access_token = accessToken;
            dynamic result = fb.Get($"/me/accounts", parameters);
            
            Console.WriteLine(result.ToString());

            /*

            parameters.upload_phase = "start";
            fb.AppId = this.AppId;
            fb.AppSecret = this.AppSecret;
            result = fb.Post($"/{id}/video_reels", parameters);
            Console.WriteLine(result);
        

            FacebookClient fb = new FacebookClient(accessToken);
            dynamic parameters = new ExpandoObject();
            parameters.upload_phase = "start";
            parameters.access_token = accessToken;
            dynamic result = fb.Post($"/410206332167006/video_reels", parameters);
            Console.WriteLine(result);
           
            //parameters.source = new FacebookMediaObject { ContentType = "video/mp4", FileName = vFile.FileName }.SetValue(System.IO.File.ReadAllBytes(vFile.pathToVideo));
 */