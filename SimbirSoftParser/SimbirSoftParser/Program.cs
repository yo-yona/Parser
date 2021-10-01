using System;
using System.Net;
using HtmlAgilityPack;
using System.IO;
using System.Collections.Generic;
using System.Globalization;

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

        //(HttpWebRequest)
        //https://docs.microsoft.com/en-us/dotnet/framework/network-programming/how-to-request-data-using-the-webrequest-class
        static void Main(string[] args)
        {
            try
            {
                // Create a request for the URL.
                var request = WebRequest.Create("https://www.simbirsoft.com/");

                // Get the response.
                WebResponse response = request.GetResponse();
                // Display the status.
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);

                // Get the stream containing content returned by the server.
                // The using block ensures the stream is automatically closed.
                using (Stream dataStream = response.GetResponseStream())
                {
                    // Open the stream using a StreamReader for easy access.
                    StreamReader reader = new StreamReader(dataStream);
                    // Read the content.
                    string responseFromServer = reader.ReadToEnd();
                    // Display the content.
                    //Console.WriteLine(responseFromServer);
                    var countResult = WordsCounter.ExtractText(responseFromServer);
                    Console.WriteLine(countResult);
                    //WordsCounter.PrintWords(countResult);
                }

                // Close the response.
                response.Close();
            }
            catch (WebException e)
            {
                Console.WriteLine("\r\nWebException Raised. The following error occurred : {0}", e.Status);
            }
            catch (Exception e)
            {
                Console.WriteLine("\nThe following Exception was raised : {0}", e.Message);
            }
        }
    }
}
