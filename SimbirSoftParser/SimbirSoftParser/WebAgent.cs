using System;
using System.Net;
using System.Text.RegularExpressions;

namespace SimbirSoftParser
{
    class WebAgent
    {
        private WordsCounter wordsCounter { get; set; }
        private string site { get; }
        private string content { get; }
        public WebAgent(string path)
        {
            site = path;
            using var client = new WebClient();
            client.Headers.Add("User-Agent", "C# parsing program");
            content = client.DownloadString(site);
            content = DropInternalsOf(content, "script", "style");
            content = StripHTML(content);
        }
        public string StripHTML(string HTMLText)
        {
            Regex reg = new Regex("<[^>]+>", RegexOptions.IgnoreCase);
            return reg.Replace(HTMLText, "");
        }

        public string DropInternalsOf(string HTMLText, params string[] TagNames)
        {
            foreach (var TagName in TagNames)
            {
                Regex reg = new Regex($@"(?<=^|\s)<{TagName}.*>[\S\s]+?</{TagName}>(?=\s|$)", RegexOptions.IgnoreCase);
                HTMLText = reg.Replace(HTMLText, "");
            }
            return HTMLText;
        }

        public void CountStatistics()
        {
            wordsCounter = new WordsCounter();
            wordsCounter.ExtractWords(content);
        }
        
        public void PrintStatistics()
        {
            wordsCounter?.PrintWordsCounts(site);
        }
    }
}
