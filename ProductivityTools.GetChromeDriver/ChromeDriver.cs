using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace ProductivityTools.GetChromeDriver
{
    public class ChromeDriver
    {
        private const string ChromeDriverUrl = "https://chromedriver.chromium.org/home/";
        private const string ChromeDriverName = "chromedriver_win32.zip";
        private const string ChromeDriverExeName = "chromedriver.exe";
        

        // static readonly HttpClient client = new HttpClient();

        public static void DownloadLatestVersion()
        {
            try
            {
                HttpClient client = new HttpClient();
                var response = client.GetStringAsync(ChromeDriverUrl).Result;

                //https://chromedriver.storage.googleapis.com/index.html?path=89.0.4389.23/" 
                //path=89.0.4389.23/" 
                // string pattern = @"^(http|https|ftp|)\://|[a-zA-Z0-9\-\.]+\.[a-zA-Z](:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*[^\.\,\)\(\s]$";
                string pattern = @"\b(https://chromedriver.storage.googleapis.com/index.html)\S+\b";
                Regex reg = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                var match= reg.Match(response);
                var latestStableVersionLink=match.Value+"/";

                //var directoryListing = client.GetAsync(latestStableVersionLink).Result;

                pattern = @"\b((\d+).(\d+).(\d+).(\d+))\b";

                reg = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                match = reg.Match(latestStableVersionLink);
                var version = match.Value;
                var windowsDownladLink = $"https://chromedriver.storage.googleapis.com/{version}/chromedriver_win32.zip";

                if (File.Exists(ChromeDriverName))
                {
                    File.Delete(ChromeDriverName);
                }

                if (File.Exists(ChromeDriverExeName))
                {
                    File.Delete(ChromeDriverExeName);
                }

                using (var webclient = new WebClient())
                {
                    webclient.DownloadFile(windowsDownladLink, "chromedriver_win32.zip");
                }
              
                ZipFile.ExtractToDirectory("chromedriver_win32.zip", ".");
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
