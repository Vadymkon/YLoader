using PuppeteerExtraSharp.Plugins.AnonymizeUa;
using PuppeteerExtraSharp.Plugins.BlockResources;
using PuppeteerExtraSharp.Plugins.ExtraStealth;
using PuppeteerExtraSharp;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YLoader.Classes
{
    public class BrowserPuppetter 
    {
        public IBrowser browser;
        public List<PagePuppetter> pages = new List<PagePuppetter>();

        String pathToBrowser = @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe";

        String loginEmail = "";
        String password = "";

        // constructor
        public BrowserPuppetter()
        {
            Init();
        }
        public BrowserPuppetter(String login_name, String pass)
        {
            loginEmail = login_name;
            password = pass;
            Init();
        }

        async public void Init( )
        {
            var blockPlugin = new BlockResourcesPlugin();
            var extra = new PuppeteerExtra().Use(new AnonymizeUaPlugin()).Use(new StealthPlugin());

            //block staff on page
            blockPlugin.AddRule(builder => builder.BlockedResources(
                ResourceType.Image,
                ResourceType.Img,
                ResourceType.Media,
                ResourceType.Font,
                ResourceType.Manifest,
                ResourceType.EventSource,
                ResourceType.Ping,
                ResourceType.TextTrack,
                ResourceType.Unknown,
                ResourceType.WebSocket,


                /*
                 * BLOCKING SITE FULLY
                 * 
                 ResourceType.StyleSheet,
                 ResourceType.Document,
                 ResourceType.Script,
                 ResourceType.Other,
                 ResourceType.Fetch,
                 */
                ResourceType.Xhr
                ).ForUrl(""));

            extra.Use(blockPlugin); //blocker

            var log_name = loginEmail.Split('@')[0];


            LaunchOptions browserOptions;

            browserOptions = new LaunchOptions
            {
                Headless = false,
                ExecutablePath = pathToBrowser,
                Args = new String[4] { "--fast-start", "--disable-extensions", "--no-sandbox", "--disable-dev-shm-usage", },

                    //HERE STARTS THIS LIST 

             /*       "--autoplay-policy=user-gesture-required",
                "--disable-background-networking",
                "--disable-background-timer-throttling",
                "--disable-backgrounding-occluded-windows",
                "--disable-breakpad",
                "--disable-client-side-phishing-detection",
                "--disable-component-update",
                "--disable-default-apps",
                "--disable-dev-shm-usage",
                "--disable-domain-reliability",
                "--disable-extensions",
                "--disable-features=AudioServiceOutOfProcess",
                "--disable-hang-monitor",
                "--disable-ipc-flooding-protection",
                "--disable-notifications",
                "--disable-offer-store-unmasked-wallet-cards",
                "--disable-popup-blocking",
                "--disable-print-preview",
                "--disable-prompt-on-repost",
                "--disable-renderer-backgrounding",
                "--disable-setuid-sandbox",
                "--disable-speech-api",
                "--disable-sync",
                "--hide-scrollbars",
                "--ignore-gpu-blacklist",
                "--metrics-recording-only",
                "--mute-audio",
                "--no-default-browser-check",
                "--no-first-run",
                "--no-pings",
                "--no-sandbox",
                "--no-zygote",
                "--password-store=basic",
                "--use-gl=swiftshader",
                "--use-mock-keychain"


                //HERE END OF THIS LIST


                    ,"--disable-plugins" }, *///best (silent&show)
                                            // "--headless",

                IgnoreHTTPSErrors = true,
                UserDataDir = $"userData"
            };


            //var browser
            browser = await extra.LaunchAsync(browserOptions);
        }
        async public void Close()
        {
            await this.browser.CloseAsync();
        }
        async public Task OpenPage(string URL)
        {

            while (browser == null)
            {
                await Task.Delay(100); // Wait until browser is initialized
            }
            var page = await browser.NewPageAsync(); // initialize

            //делает медленнее await page.SetRequestInterceptionAsync(true);
            //--making slower await page.GoToAsync(URL, WaitUntilNavigation.DOMContentLoaded ); // go to url

            //speed up (CPU++)
            /*await page.SetViewportAsync(new ViewPortOptions
            {
                Width = 0,
                Height = 0
            });*/

            //compress staff
            await page.SetExtraHttpHeadersAsync(new Dictionary<string, string> { { "Accept-Encoding", "gzip, deflate, br" } });

            await page.GoToAsync(URL); // go to url
            pages.Add(new PagePuppetter(page, loginEmail.Split('@')[0], password)); // save access to this page

        }

        async public void ClosePage(string URL)
        {

            while (browser == null)
            {
                await Task.Delay(100); // Wait until browser is initialized
            }
            pages.Where(x => x.URL.Contains(URL)).ToList().ForEach(x => x.Close()); //close them

        }
        async public void UpdatePageList()
        {
            var new_pages = await this.browser.PagesAsync();
            pages = new_pages.Select(x => new PagePuppetter(x, loginEmail.Split('@')[0], password)).ToList();
        }
    }
}
