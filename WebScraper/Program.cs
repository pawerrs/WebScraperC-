using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Viewer.Models;

namespace WebScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser parser = new Parser();
            parser.ParsKulturalnie();
            Console.ReadKey();
        }
    }
}
