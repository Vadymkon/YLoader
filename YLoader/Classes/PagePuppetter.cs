using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YLoader.Classes
{
    public class PagePuppetter 
    {
        public PuppeteerSharp.IPage page;
        String login, password;

        public string URL { get => page.Url; }


        public PagePuppetter(PuppeteerSharp.IPage page, String login, String password)
        {
            this.page = page;
            this.login = login;
            this.password = password;
        }

        async public void Refresh()
        {
            await page.ReloadAsync(30000); //reload

            // --making slower await
            //page.ReloadAsync(30000, new WaitUntilNavigation[1] { WaitUntilNavigation.DOMContentLoaded }); //reload
        }

        public async Task ClickOn(string CSSclass, string element = "div")
        {
            try
            {
                await this.page.WaitForSelectorAsync(element + "." + CSSclass);
                var submitElement = await this.page.QuerySelectorAsync(element + "." + CSSclass);
                await submitElement.ClickAsync();
            }
            catch (Exception ex)
            {
            }

        }
        public async Task WriteOnInput(string write, string CSSclass)
        {
            try
            {
                await page.WaitForSelectorAsync("input#" + CSSclass);
                var field = await page.QuerySelectorAsync("input#" + CSSclass);
                await field.TypeAsync(write);
            }
            catch (Exception ex)
            {
            }

        }

        async public Task<bool> Check()
        {
            var button = await this.page.WaitForSelectorAsync("button.UNPbK.wgwPg.oZjiQ.FSc9M.FTeqR", new WaitForSelectorOptions { Timeout = 15000 }); //wait for button
            return button != null;
        }
        async public void Close()
        {
            await this.page.CloseAsync();
        }
        public void Auth()
        {
            
        }
    }
}
