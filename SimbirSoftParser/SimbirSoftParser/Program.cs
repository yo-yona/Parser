using System;
using System.Text.RegularExpressions;

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
            try
            {
                WebAgent webConnection = new WebAgent(site);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
