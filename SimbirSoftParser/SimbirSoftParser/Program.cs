using System;

namespace SimbirSoftParser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите сайт для проверки");
            string site = Console.ReadLine();
            if (String.IsNullOrWhiteSpace(site))
                site = "https://simbirsoft.com/";
            WebAgent webConnection = new WebAgent(site);
            webConnection.CountStatistics();
            webConnection.PrintStatistics();
        }
    }
}
