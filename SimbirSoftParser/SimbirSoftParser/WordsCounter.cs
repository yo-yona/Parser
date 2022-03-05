using System;
using System.Collections.Generic;
using System.Linq;

namespace SimbirSoftParser
{
    class WordsCounter
    {
        private Dictionary<string, uint> wordStatistics;

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
        
        private void ExtractWords(string incoming)
        {
            Console.WriteLine("WordsExtracter начал работу...");
            char[] metaChar = { ' ', ',', '.', '!', '?', '"', ';', ':', '[', ']', '(', ')', '\n', '\r', '\t', '/', '<', '>', '\'', '«', '»', '?' };

            string[] textDividedIntoWords = incoming?.ToUpper().Split(metaChar, StringSplitOptions.RemoveEmptyEntries);
            if (textDividedIntoWords != null)
            {
                    foreach (var word in textDividedIntoWords)
                    {
                        SafeCountIncrement(word);
                    }
            }
        }
        
        public void PrintWordsCounts(string site, string content)
        {
            DBManager dbManager = new DBManager(site);
            dbManager.CheckInDB(site, ref wordStatistics);
            if (wordStatistics.Count==0)
            {
                this.ExtractWords(content);
                dbManager.PushToDB(wordStatistics);
            }
            else
                Console.WriteLine($"Веб-страница {site} уже посещалась. Загружаю статистику из памяти...");
            
            //Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine($"\n\nСтатистика слов для сайта {site}");
            Console.WriteLine($"======================================");
            foreach (var entry in wordStatistics.OrderByDescending(kvPair => kvPair.Value))
            {
                Console.WriteLine($"\n\n{entry.Value} \t {entry.Key}");
                foreach (var letter in System.Text.Encoding.UTF8.GetBytes(entry.Key))
                    Console.Write($" {letter}");
            }
        }
    }
}
