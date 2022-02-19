using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadInstaller
{
    class WebSiteLinkInfo
    {
        public string Link { get; set; }
        public string Version { get; set; }
        public string FileName { get; set; }
    }

    internal static class GimpWebSiteUtil
    {
        public static async Task<WebSiteLinkInfo> GetDownloadLink()
        {
            Log.Info("Getting URL of latest Windows GIMP Setup");

            var url = "https://www.gimp.org/downloads/";
            var linkXPath = "//a[@id='win-download-link']";
            var web = new HtmlWeb();
            var doc = web.Load(url);
            var installerDownloadLink = doc.DocumentNode.SelectSingleNode(linkXPath);

            if (installerDownloadLink == null)
            {
                Log.Error($"Cannot find link at {url} using XPath {linkXPath}.");
                return null;
            }

            string href = installerDownloadLink.GetAttributeValue("href", "");
            if (href.StartsWith("//"))
                href = "https:" + href;

            Log.Confirm($"Download link: {href}");

            Log.Info($"Extracting version from the link");
            int startPos = href.IndexOf('-') + 1;
            int endPos = href.LastIndexOf('-');
            string version = href.Substring(startPos, endPos - startPos);
            string filename = href.Substring(href.LastIndexOf('/') + 1);
            Log.Confirm($"Version: {version}");

            return new WebSiteLinkInfo { Link = href, Version = version, FileName = filename };
        }

        public static async Task<string> Download(string url, string folder, string fileName)
        {
            Log.Info($"Downloading {url} into {folder} with name {fileName}");

            var destinationFile = Path.Combine(folder, fileName);
            if (File.Exists(destinationFile))
            {
                Log.Info("File is already downloaded. Download is skipped.");
                return destinationFile;
            }

            var destinationFileDownload = destinationFile + "_";
            if (File.Exists(destinationFileDownload))
                File.Delete(destinationFileDownload);

            Log.Info($"Downloading!\nPlease wait.");
            using (HttpClient client = new HttpClient())
            {
                using (var response = await client.GetAsync(url))
                {
                    using (var fs = new FileStream(destinationFileDownload, FileMode.CreateNew))
                    {
                        await response.Content.CopyToAsync(fs);
                    }
                }
            }

            Log.Info($"Setup is downloaded.");
            File.Move(destinationFileDownload, destinationFile);
            return destinationFile;
        }
    }
}
