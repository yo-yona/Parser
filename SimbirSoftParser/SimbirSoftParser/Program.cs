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
        private void SafeDictCount(Dictionary<string, uint> wordStatistics, string word, uint count)
        {
            if (wordStatistics.ContainsKey(word))
            {
                Console.Write($"'{wordStatistics[word]}. Next is this ++");
                wordStatistics[word]++;
                //Console.WriteLine($"'{substring}' met {wordStatistics[substring]} times. {wordStatistics.ContainsKey(substring)}");
            }
            else
            {
                wordStatistics.Add(word, 1);
                //Console.WriteLine($"'{substring}' added. {wordStatistics[substring]}. {wordStatistics.ContainsKey(substring)}");
            }
        }
            public static void ExtractWords(Dictionary<string, uint> wordStatistics, string incoming)
        {
            char[] metaChar = { ' ', ',', '.', '!', '?', '"', ';', ':', '[', ']', '(', ')', '\n', '\r', '\t' };

            string[] result = incoming.ToUpper().Split(metaChar, StringSplitOptions.RemoveEmptyEntries);


            foreach (var substring in result)
            {
                if (wordStatistics.ContainsKey(substring))
                {
                    Console.Write($"'{wordStatistics[substring]}. Next is this ++");
                    wordStatistics[substring]++;
                    Console.WriteLine($"'{substring}' met {wordStatistics[substring]} times. {wordStatistics.ContainsKey(substring)}");
                }
                else
                {
                    wordStatistics.Add(substring, 1);
                    Console.WriteLine($"'{substring}' added. {wordStatistics[substring]}. {wordStatistics.ContainsKey(substring)}");
                }
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var html = @"https://simbirsoft.com/";

            HtmlWeb web = new HtmlWeb();

            HtmlDocument doc = web.Load(html);

            Dictionary<string, uint> wordStatistics = new Dictionary<string, uint>();

            foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//text()[not(parent::script) and not(parent::style)]"))
            {
                WordsCounter.ExtractWords(wordStatistics, node.InnerText);
                //Console.Write(node.InnerText + " ");
            }

            Console.WriteLine("\n\n===============DICTIONARY==============\n\n");
            foreach (var entry in wordStatistics)
            {
                Console.WriteLine($"{entry.Key} - {entry.Value}");
            }
        }
    }
}
