using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraper
{
    public static class Queries
    {

        public static string insertEventsQuery = @"INSERT INTO [EVENTS] VALUES (@id, @Name, @Date, @Place, @Price, @Category, @Info)";

        public static string deleteAllEvents = @"DELETE FROM [EVENTS]";

        public static string deleteAllCoordinates = @"DELETE FROM [GeographicCoordinates]";

        public static string insertLatAndLong = @"INSERT INTO [GeographicCoordinates] VALUES (@Id, @Latitude, @Longtitude)";
    }
}
