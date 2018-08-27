using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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
        InsertingDataClass dataAccess = new InsertingDataClass();
        public List<Event> events = new List<Event>();
        string[] urlsKulturalnie = {
            "http://kulturalnie.waw.pl/",
            "http://kulturalnie.waw.pl/2/",
            "http://kulturalnie.waw.pl/3/",
            
        };
        int counter = 1;

        string urlGoingApp = "https://goingapp.pl/calendar/1/warszawa/1/any/any";
        public void ParseKulturalnie()
        {

            foreach (var url in urlsKulturalnie)
            {
                Task<string> data = httpAgent.GetString(url);
                var result = data.Result;
                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(result);

                foreach (HtmlNode node in htmlDocument.DocumentNode.SelectNodes("//li[@class='event']"))
                {

                    Event singleEvent = new Event();
                    singleEvent.Place = node.SelectSingleNode(".//li[@class='location']").InnerText.Trim().Replace('|', ' ');
                    singleEvent.Date = node.SelectSingleNode(".//span[@class='date']").InnerText.Trim();
                    singleEvent.Category = node.SelectSingleNode(".//li[@class='category']").InnerText.Trim().Replace('|', ' ');
                    singleEvent.Name = node.SelectSingleNode(".//h2[@itemprop='name']").InnerText.Trim();
                    singleEvent.Id = counter;
                    singleEvent.Information = node.SelectSingleNode(".//div[@class='eventDescription']").InnerText.TrimStart();
                    singleEvent.Price = node.SelectSingleNode(".//li[@class='tickets']").InnerText.Trim();
                    events.Add(singleEvent);
                    counter++;
                }
                Console.WriteLine(events.Count);
            }

        }

        public void ParseGoingApp()
        {
            Task<string> data = httpAgent.GetString(urlGoingApp);
            var result = data.Result;
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(result);

            foreach (HtmlNode node in htmlDocument.DocumentNode.SelectNodes("//div[@class='event_list_item_inner']"))
            {
                
                Event singleEvent = new Event();
                try
                {
                    singleEvent.Id = counter;
                    singleEvent.Category = node.SelectSingleNode(".//a[@class='event_category_label_link']").InnerText.Trim();
                    singleEvent.Name = node.SelectSingleNode(".//p[@class='event_info_box_title']").InnerText.Trim();

                    string[] placeAndDate = node.SelectSingleNode(".//p[@class='event_info_box_info']").
                            InnerText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                    singleEvent.Place = placeAndDate[2].Trim();
                    singleEvent.Date = placeAndDate[1].Trim();
                    singleEvent.Information = node.SelectSingleNode(".//div[@class='event_desc']").InnerText.TrimStart();
                    singleEvent.Price = node.SelectSingleNode(".//div[@class='event_info_box_right_inner_container']").InnerText.Trim();
                    events.Add(singleEvent);
                    counter++;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.ReadKey();
                }
            }
            Console.WriteLine(events.Count);
            dataAccess.InsertEventsData(events);
            AddCoordinatesToEvents();
        }

        public async void AddCoordinatesToEvents()
        {
            foreach(Event singleEvent in events)
            {
                var eventLatLng = await  geolocation.GetLatitudeandLongtitude(singleEvent.Place);
                singleEvent.Latitude = eventLatLng.latitude;
                singleEvent.Longtitude = eventLatLng.longtitude;
            }
            dataAccess.InsertGeographicCoordinates(events);

        }
        
    }
}
