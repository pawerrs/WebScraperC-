using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Viewer.Models;

namespace WebScraper
{
    public class Geolocation
    {
        HTTPAgent httpAgent = new HTTPAgent();
        public async Task<(double latitude, double longtitude)> GetLatitudeandLongtitude(string locationName)
        {
            double latitude;
            double longtitude;
            try
            {
                
                var response = await httpAgent.GetString($"https://maps.googleapis.com/maps/api/geocode/json?address=Warsaw,{locationName},+CA&key=AIzaSyDJJJ6Dww9ixqfaLm2J186cPPM1uRNL-5A");
                GeoCoderModel dataResult = JsonConvert.DeserializeObject<GeoCoderModel>(response);
                latitude = dataResult.results[0].geometry.location.lat;
                longtitude = dataResult.results[0].geometry.location.lng;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                latitude = 0;
                longtitude = 0;
            }

            return (latitude, longtitude);
        }

    }
}
