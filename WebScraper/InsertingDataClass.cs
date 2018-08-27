using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using Viewer.Models;

namespace WebScraper
{
    public class InsertingDataClass
    {
        public void InsertEventsData(List<Event> events)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connString"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    using (var deleteCommand = new SqlCommand(Queries.deleteAllEvents, conn))
                    {
                        deleteCommand.ExecuteNonQuery();
                    }

                    using (var insertCommand = new SqlCommand(Queries.insertEventsQuery, conn))
                    {
                        foreach (var eventObject in events)
                        {
                            insertCommand.Parameters.AddWithValue("@id", eventObject.Id);
                            insertCommand.Parameters.AddWithValue("@Name", eventObject.Name);
                            insertCommand.Parameters.AddWithValue("@Date", eventObject.Date);
                            insertCommand.Parameters.AddWithValue("@Place", eventObject.Place);
                            insertCommand.Parameters.AddWithValue("@Price", eventObject.Price);
                            insertCommand.Parameters.AddWithValue("@Category", eventObject.Category);
                            insertCommand.Parameters.AddWithValue("@Info", eventObject.Information);
                            insertCommand.ExecuteNonQuery();
                            insertCommand.Parameters.Clear();
                        }
                    }

                    using (var insertCommand = new SqlCommand(Queries.insertLatAndLong, conn))
                    {
                        foreach (var eventObject in events)
                        {
                            insertCommand.Parameters.AddWithValue("@id", eventObject.Id);
                            insertCommand.Parameters.AddWithValue("@Latitude", eventObject.Latitude);
                            insertCommand.Parameters.AddWithValue("@Longtitude", eventObject.Longtitude);
                            insertCommand.ExecuteNonQuery();
                            insertCommand.Parameters.Clear();
                        }
                    }

                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public void InsertGeographicCoordinates(List<Event> events)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connString"].ConnectionString))
            {
                try
                {

                    conn.Open();
                    using (var deleteCommand = new SqlCommand(Queries.deleteAllCoordinates, conn))
                    {
                        deleteCommand.ExecuteNonQuery();
                    }

                    using (var insertCommand = new SqlCommand(Queries.insertLatAndLong, conn))
                    {
                        foreach (var eventObject in events)
                        {
                            insertCommand.Parameters.AddWithValue("@id", eventObject.Id);
                            insertCommand.Parameters.AddWithValue("@Latitude", eventObject.Latitude);
                            insertCommand.Parameters.AddWithValue("@Longtitude", eventObject.Longtitude);
                            insertCommand.ExecuteNonQuery();
                            insertCommand.Parameters.Clear();
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

        }
    }
}
