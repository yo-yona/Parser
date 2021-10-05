using HtmlAgilityPack;

namespace SimbirSoftParser
{
    class WebAgent
    {
        private HtmlDocument doc { get; set; }
        private WordsCounter wordsCounter { get; set; }
        private string path { get; }
        public WebAgent(string html)
        {
            path = html;
            HtmlWeb web = new HtmlWeb();
            doc = web.Load(path);
        }
        
        public void CountStatistics()
        {
            wordsCounter = new WordsCounter();
            var rule = "//text()[not(parent::script) and not(parent::style)]";

            foreach (HtmlNode node in doc.DocumentNode.SelectNodes(rule))
            {
                wordsCounter.ExtractWords(node.InnerText);
            }
        }
        
        public void PrintStatistics()
        {
            wordsCounter?.PrintWordsCounts(path);
        }
    }
}
