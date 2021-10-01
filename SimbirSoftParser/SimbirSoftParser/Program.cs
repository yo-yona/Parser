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
        public static string ExtractText(string html)
        {
            if (html == null)
            {
                throw new ArgumentNullException("html");
            }

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            var chunks = new List<string>();

            foreach (var item in doc.DocumentNode.DescendantsAndSelf())
            {
                if (item.NodeType == HtmlNodeType.Text)
                {
                    if (item.InnerText.Trim() != "")
                    {
                        chunks.Add(item.InnerText.Trim());
                    }
                }
            }
            return String.Join(" ", chunks);
        }
            public static void PrintWords(string incoming)
        {
            char[] metaChar = { ' ', ',', '.', '!', '?', '"', ';', ':', '[', ']', '(', ')', '\n', '\r', '\t' };

            string[] result = incoming.Split(metaChar, StringSplitOptions.RemoveEmptyEntries);

            foreach (var substring in result)
            {
                Console.WriteLine($"Substring: {substring}");
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var html = @"https://en.wikipedia.org/wiki/Shellsort";

            HtmlWeb web = new HtmlWeb();

            HtmlDocument doc = web.Load(html);

            foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//text()"))
            {
                Console.Write(node.Name + node.InnerText + " ");
            }
        }
    }
}
