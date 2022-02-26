using HtmlAgilityPack;

namespace DownloadInstaller
{
    internal static class GimpWebSiteUtil
    {
        public static DownloadLinkInfo GetWindowsSetupDownloadLinkInfo()
        {
            var downloadPage = "https://www.gimp.org/downloads/";
            var linkXPath = "//a[@id='win-download-link']";

            var webPage = new HtmlWeb();
            var htmlDocument = webPage.Load(downloadPage);
            var linkNode = htmlDocument.DocumentNode.SelectSingleNode(linkXPath);

            if (linkNode == null)
            {
                throw new Exception($"Cannot find link at {downloadPage} using XPath {linkXPath}.");
            }

            string link = linkNode.GetAttributeValue("href", "");
            if (link.StartsWith("//"))
                link = "https:" + link;

            Console.WriteLine($"URL: {link}");

            int startPos = link.IndexOf('-') + 1;
            int endPos = link.LastIndexOf('-');
            string version = link.Substring(startPos, endPos - startPos);
            string filename = link.Substring(link.LastIndexOf('/') + 1);
            Log.Info($"Version: {version}");

            return new DownloadLinkInfo { Link = link, FileName = filename };
        }

        public static async Task<string> Download(string url, string folder, string fileName)
        {
            Log.Debug($"Downloading {url} into {folder} with name {fileName}");

            var destinationFile = Path.Combine(folder, fileName);
            if (File.Exists(destinationFile))
            {
                Log.Info("File is already downloaded. Download is skipped.");
                return destinationFile;
            }

            var destinationFileDownload = destinationFile + "_";
            if (File.Exists(destinationFileDownload))
                File.Delete(destinationFileDownload);

            Log.Debug($"Downloading!\nPlease wait.");
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

            Log.Debug($"Setup is downloaded.");
            File.Move(destinationFileDownload, destinationFile);
            return destinationFile;
        }
    }
}
