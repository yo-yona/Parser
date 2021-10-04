using System;
using System.Net;
using HtmlAgilityPack;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace SimbirSoftParser
{
    class WebAgent
    {
        HtmlDocument doc { get; set; }
        WordsCounter wordsCounter { get; set; }
        public WebAgent(string html)
        {

            HtmlWeb web = new HtmlWeb();
            doc = web.Load(html);
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
            wordsCounter?.PrintWordsCounts();
        }
    }
    class WordsCounter
    {
        private Dictionary<string, uint> wordStatistics { get; set; }
        
        public WordsCounter()
        {
            wordStatistics = new Dictionary<string, uint>();
        }
        private void SafeCountIncrement(string word)
        {
            if (word.All(c => Char.IsLetter(c)))
            {
                if (wordStatistics.ContainsKey(word))
                {
                    wordStatistics[word]++;
                }
                else
                {
                    wordStatistics.Add(word, 1);
                }
            }
        }
            public void ExtractWords(string incoming)
        {
            char[] metaChar = { ' ', ',', '.', '!', '?', '"', ';', ':', '[', ']', '(', ')', '\n', '\r', '\t', '/', '<', '>', '\'', '«', '»' };

            string[] nodeTextDividedIntoWords = incoming.ToUpper().Split(metaChar, StringSplitOptions.RemoveEmptyEntries);
            foreach (var word in nodeTextDividedIntoWords)
            {
                SafeCountIncrement(word);
            }
        }
        public void PrintWordsCounts()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("===============DICTIONARY==============\n\n");
            foreach (var entry in wordStatistics.OrderByDescending(kvPair => kvPair.Value))
            {
                Console.WriteLine($"{entry.Value} \t {entry.Key}");
            }
        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            WebAgent webConnection = new WebAgent("https://simbirsoft.com/"); //@?
            webConnection.CountStatistics();
            webConnection.PrintStatistics();
        }
    }
}
