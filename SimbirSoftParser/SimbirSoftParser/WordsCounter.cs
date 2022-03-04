using System;
using System.Collections.Generic;
using System.Linq;

namespace SimbirSoftParser
{
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
        
        public void PrintWordsCounts(string site)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine($"STATISTICS for {site}\n");
            foreach (var entry in wordStatistics.OrderByDescending(kvPair => kvPair.Value))
            {
                Console.WriteLine($"{entry.Value} \t {entry.Key}");
            }
        }
    }
}
