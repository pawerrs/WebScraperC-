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
    public class Parser
    {
        HTTPAgent httpAgent = new HTTPAgent();
        Geolocation geolocation = new Geolocation();
        public List<Event> events = new List<Event>();
        string[] urls = {
            "http://kulturalnie.waw.pl/",
            "http://kulturalnie.waw.pl/2/",
            "http://kulturalnie.waw.pl/3/"

        };

        public async void ParsKulturalnie()
        {
            foreach (var url in urls)
            {

                Task<string> data = httpAgent.GetString(url);

                var result = data.Result;

                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(result);

                int i = 0;
                foreach (HtmlNode node in htmlDocument.DocumentNode.SelectNodes("//li[@class='event']"))
                {

                    Event singleEvent = new Event();
                    singleEvent.Id = i;
                    singleEvent.Place = node.SelectSingleNode(".//li[@class='location']").InnerText.Trim();
                    //var eventLatLng = await geolocation.GetLatitudeandLongtitude(singleEvent.Place);
                    // singleEvent.Latitude = eventLatLng.latitude;
                    //singleEvent.Longtitude = eventLatLng.longtitude;
                    singleEvent.Date = node.SelectSingleNode(".//span[@class='date']").InnerText.Trim();
                    singleEvent.Category = node.SelectSingleNode(".//li[@class='category']").InnerText.Trim();
                    singleEvent.Name = node.SelectSingleNode(".//h2[@itemprop='name']").InnerText.Trim();

                    singleEvent.Information = node.SelectSingleNode(".//div[@class='eventDescription']").InnerText.TrimStart();
                    singleEvent.Category = node.SelectSingleNode(".//li[@class='tickets']").InnerText.Trim();
                    events.Add(singleEvent);
                    i++;

                }
            }

        }
    }
}
