namespace SimbirSoftParser
{
    class Program
    {
        static void Main(string[] args)
        {
            WebAgent webConnection = new WebAgent("https://simbirsoft.com/");
            webConnection.CountStatistics();
            webConnection.PrintStatistics();
        }
    }
}
