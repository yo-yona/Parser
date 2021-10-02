using System;
using System.Net;
using HtmlAgilityPack;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SimbirSoftParser
{
    class WordsCounter
    {
        //Сделай лучше расширением к dictionary чтобы не передавать параметр!
        static private void SafeCountIncrement(Dictionary<string, uint> wordStatistics, string word)
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
            public static void ExtractWords(Dictionary<string, uint> nodeInnerText, string incoming)
        {
            char[] metaChar = { ' ', ',', '.', '!', '?', '"', ';', ':', '[', ']', '(', ')', '\n', '\r', '\t', '/', '<', '>', '\'' };

            string[] nodeTextDividedIntoWords = incoming.ToUpper().Split(metaChar, StringSplitOptions.RemoveEmptyEntries);

            foreach (var word in nodeTextDividedIntoWords)
            {
                SafeCountIncrement(nodeInnerText, word);
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var html = "https://simbirsoft.com/"; //@?
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(html);

            Dictionary<string, uint> wordStatistics = new Dictionary<string, uint>();

            foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//text()[not(parent::script) and not(parent::style)]"))
            {
                WordsCounter.ExtractWords(wordStatistics, node.InnerText);
            }

            Console.WriteLine("\n\n===============DICTIONARY==============\n\n");
            foreach (var entry in wordStatistics.OrderByDescending(kvPair => kvPair.Value))
            {
                Console.WriteLine($"{entry.Key} - {entry.Value}");
            }
        }
    }
}
