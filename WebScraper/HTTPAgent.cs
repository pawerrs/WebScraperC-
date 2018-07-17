using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Viewer.Models;

namespace WebScraper
{
    public  class HTTPAgent
    {
        private readonly HttpClient _httpclient = new HttpClient();

        public async Task<string> GetString(string URL)
        {
            var response = await _httpclient.GetStringAsync(URL);
            return response;
        }

    }
}
