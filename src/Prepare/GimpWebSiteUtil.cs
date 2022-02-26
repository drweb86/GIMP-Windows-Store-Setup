using HtmlAgilityPack;

namespace DownloadInstaller
{
    internal static class GimpWebSiteUtil
    {
        public static string GetWindowsSetupUrl()
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

            return link;
        }

        public static async Task<string> Download(string url, string folder)
        {
            var destinationFile = Path.Combine(folder, $"setup-{url.GetHashCode()}.exe");

            Console.WriteLine($"Downloading file {url} to {destinationFile}");
            
            if (File.Exists(destinationFile))
            {
                Console.WriteLine("File is already downloaded. Skipping.");
                return destinationFile;
            }

            var temporaryDownloadFile = destinationFile + "_";
            if (File.Exists(temporaryDownloadFile))
                File.Delete(temporaryDownloadFile);

            Console.WriteLine($"Downloading! Please wait...");
            using (var httpClient = new HttpClient())
                using (var httpResponse = await httpClient.GetAsync(url))
                    using (var fileStream = new FileStream(temporaryDownloadFile, FileMode.CreateNew))
                        await httpResponse.Content.CopyToAsync(fileStream);

            Console.WriteLine($"Downloaded is completed.");
            File.Move(temporaryDownloadFile, destinationFile);

            return destinationFile;
        }
    }
}
