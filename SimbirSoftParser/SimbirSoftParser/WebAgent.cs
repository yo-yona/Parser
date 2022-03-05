using System;
using System.Net;
using System.Text.RegularExpressions;

namespace SimbirSoftParser
{
    /// <summary>
    /// Взаимодействие с веб-страницей. Вся функциональность тут
    /// </summary>
    class WebAgent
    {
        private WordsCounter wordsCounter { get; set; }
        private string site { get; }
        /// <summary>
        /// HTML url'а
        /// </summary>
        private string content { get; }
        /// <summary>
        /// Сразу избавляемся от всего не предназначенного для пользователя и остаток передаем на подсчет статистики слов
        /// </summary>
        public WebAgent(string path)
        {
            site = path;
            using var client = new WebClient();
            client.Headers.Add("User-Agent", "C# parsing program");
            content = client.DownloadString(site);
            content = DropInternalsOf(content, "script", "style");
            content = StripHTML(content);
            this.PrintStatistics();
        }
        /// <summary>
        /// Избавляется от всех тэгов
        /// </summary>
        private string StripHTML(string HTMLText)
        {
            Regex reg = new Regex("<[^>]+>", RegexOptions.IgnoreCase);
            return reg.Replace(HTMLText, "");
        }
        /// <summary>
        /// Избавляется от указанных тэгов и их содержимых
        /// </summary>
        private string DropInternalsOf(string HTMLText, params string[] TagNames)
        {
            foreach (var TagName in TagNames)
            {
                Regex reg = new Regex($@"(?<=^|\s)<{TagName}.*>[\S\s]+?</{TagName}>(?=\s|$)", RegexOptions.IgnoreCase);
                HTMLText = reg.Replace(HTMLText, "");
            }
            return HTMLText;
        }
        /// <summary>
        /// Подсчет статистики
        /// </summary>
        private void PrintStatistics()
        {
            wordsCounter = new WordsCounter();
            wordsCounter.PrintWordsCounts(site, content);
        }
    }
}
