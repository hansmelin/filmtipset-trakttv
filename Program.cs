using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;

namespace filmtipset.trakttv
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            string[] readAllLines = File.ReadAllLines(@"C:\filmtipset.csv", Encoding.Default);


            using (FileStream fs = File.Create(@"C:\imbd.csv"))
            using (var sw = new StreamWriter(fs, Encoding.Default))
            {
                int i = 0;

                foreach (string line in readAllLines)
                {
                    if (0 == i)
                    {
                        sw.WriteLine(
                            "\"position\",\"const\",\"created\",\"modified\",\"description\",\"Title\",\"Title type\",\"Directors\",\"You rated\",\"IMDb Rating\",\"Runtime (mins)\",\"Year\",\"Genres\",\"Num. Votes\",\"Release Date (month/day/year)\",\"URL\"");
                    }
                    else
                    {
                        string[] split = line.Split(';');
                        string swedishTitle = split[0];
                        string englishTitle = split[1];
                        string year = split[2];
                        string directors = split[3];
                        int rating = int.Parse(split[4]);
                        string date = split[5];
                        string imdbId = split[6];

                        // format imbd id tt0088247
                        string newImdbId = "tt" + imdbId.PadLeft(7, '0');

                        // fix date from 2015-03-30 21:20:38 to Sat Apr 18 00:00:00 2015
                        DateTime time = DateTime.Parse(date);
                        string dateTime = time.ToString("ddd MMM ") + time.ToString("%d").PadLeft(2, ' ') + " 00:00:00 " +
                                          time.ToString("yyyy");

                        sw.WriteLine(
                            "\"{0}\",\"{1}\",\"{2}\",\"\",\"\",\"{3}\",\"Feature Film\",\"{4}\",\"{5}\",\"5.0\",\"100\",\"{6}\",\"crime\",\"100\",\"1979-01-01\",\"http://www.imdb.com/title/{1}/\"",
                            i, newImdbId, dateTime, englishTitle, directors, rating*2, year);
                    }


                    i++;
                }
            }
        }
    }
}