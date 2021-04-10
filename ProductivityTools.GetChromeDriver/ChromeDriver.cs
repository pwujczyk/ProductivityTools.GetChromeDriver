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
        
        public static void DownloadLatestVersion()
        {
            try
            {
                string version = GetLatestChromeDriverVersion();
                var windowsDownladLink = $"https://chromedriver.storage.googleapis.com/{version}/chromedriver_win32.zip";

                RemoveFileIfExists(ChromeDriverName);
                RemoveFileIfExists(ChromeDriverExeName);

                DowlnoadChromeZip(windowsDownladLink);
                ExtractZip();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string GetLatestChromeDriverVersion()
        {
            HttpClient client = new HttpClient();
            var response = client.GetStringAsync(ChromeDriverUrl).Result;

            //https://chromedriver.storage.googleapis.com/index.html?path=89.0.4389.23/" 
            string pattern = @"\b(https://chromedriver.storage.googleapis.com/index.html)\S+\b";
            Regex reg = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var match = reg.Match(response);
            var latestStableVersionLink = match.Value;


            pattern = @"\b((\d+).(\d+).(\d+).(\d+))\b";
            reg = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            match = reg.Match(latestStableVersionLink);
            var version = match.Value;
            return version;
        }

        private static void ExtractZip()
        {
            ZipFile.ExtractToDirectory(ChromeDriverName, ".");
        }

        private static void DowlnoadChromeZip(string windowsDownladLink)
        {
            using (var webclient = new WebClient())
            {
                webclient.DownloadFile(windowsDownladLink, ChromeDriverName);
            }
        }

        private static void RemoveFileIfExists(string fileName)
        {

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }
    }
}
