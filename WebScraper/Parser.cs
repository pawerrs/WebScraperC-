using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Viewer.Models;

namespace WebScraper
{
    /*
     Parser strony kulturalnie.waw.pl
     */
    public class Parser
    {
        HTTPAgent httpAgent = new HTTPAgent();
        public List<Event> events = new List<Event>();
        string[] urls = {
            "http://kulturalnie.waw.pl/",
            "http://kulturalnie.waw.pl/2/",
            "http://kulturalnie.waw.pl/3/"
        };

        public void Pars()
        {
            foreach (var url in urls)
            {

                Task<string> data = httpAgent.GetAllEvents(url);

                var result = data.Result;

                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(result);

                int i = 0;
                foreach (HtmlNode node in htmlDocument.DocumentNode.SelectNodes("//li[@class='event']"))
                {

                    Event singleEvent = new Event();
                    singleEvent.Id = i;
                    singleEvent.Date = node.SelectSingleNode(".//span[@class='date']").InnerText.Trim();
                    singleEvent.Category = node.SelectSingleNode(".//li[@class='category']").InnerText.Trim();
                    singleEvent.Name = node.SelectSingleNode(".//h2[@itemprop='name']").InnerText.Trim();
                    singleEvent.Place = node.SelectSingleNode(".//li[@class='location']").InnerText.Trim();
                    singleEvent.Information = node.SelectSingleNode(".//div[@class='eventDescription']").InnerText.TrimStart();
                    singleEvent.Category = node.SelectSingleNode(".//li[@class='tickets']").InnerText.Trim();
                    events.Add(singleEvent);
                    i++;
                }
            }

            //foreach (var eventcik in events)
            //{
            //    Event eventt = events[0];
            //    //    Console.WriteLine(eventt.Id + " " + eventt.Name + " " + eventt.Information + " " + eventt.Place + " " +
            //    //        eventt.Category + " " + eventt.Date + " " + eventt.Price);
            //    //}

            //    File.WriteAllText("E:\\TEST.txt", eventt.Information);
            //    //Console.Write(eventt.Information);
            //    Console.ReadLine();
            //}
        }
    }
}
