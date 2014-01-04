using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;
using FancyTraveller.Domain.POCO;

namespace FancyTraveller.Domain.Tests.Integration.TestingData
{
    public class RouteServiceTestingData
    {

        private static readonly IEnumerable<Vertex> vertices;

        static RouteServiceTestingData()
        {
            var verticiesJson = File.ReadAllText("App_Data/cities.txt");

            vertices = DeserializeVerticesJsonToEnumerable(verticiesJson);
            AvailableCities = DeserializeCitiesJsonToList(verticiesJson);
        }

        public static IDictionary<int, IList<Vertex>> Vertices
        {
            get
            {
                var result = new Dictionary<int, IList<Vertex>>();

                foreach(var vertex in vertices)
                {
                    if (result.ContainsKey(vertex.SourceCity.Id) == false)
                        result.Add(vertex.SourceCity.Id, new List<Vertex>() {vertex});
                    else
                        result[vertex.SourceCity.Id].Add(vertex);
                }

                return result;
            }
        }

        private static IList<City> DeserializeCitiesJsonToList(string verticiesJson)
        {
            var result = new List<City>();

            foreach (var vertex in DeserializeVerticesJsonToEnumerable(verticiesJson))
            {
                if(result.Contains(vertex.SourceCity) == false)
                    result.Add(vertex.SourceCity);
            }

            return result.OrderBy(s => s.Name).ToList();
        }

        private static IEnumerable<Vertex> DeserializeVerticesJsonToEnumerable(string verticiesJson)
        {
            var serializer = new JavaScriptSerializer();

            return serializer.Deserialize<List<Vertex>>(verticiesJson).ToList();
        }

        public static IList<City> AvailableCities { get; private set; }

        public const string DifferentPairsCollection = "DifferentPairsTestData";
        public static Vertex[] DifferentPairsTestData =
        {
            new Vertex(){ 
                DestinationCity = new City(){ Id = 2, Location = new Location(){ Latitude = 51.2194475, Longitude = 4.4024643}, Name = "Antwerp"}, 
                SourceCity = new City(){Id = 4, Location = new Location(){ Latitude = 41.3850639, Longitude = 2.1734035}, Name = "Barcelona"},
                Distance = 1465
            }, 
            new Vertex(){ 
                DestinationCity = new City(){ Id = 18, Location = new Location(){ Latitude = 51.51121389999999, Longitude = -0.1198244}, Name = "London"}, 
                SourceCity = new City(){Id = 28, Location = new Location(){ Latitude = 50.0755381, Longitude = 14.4378005}, Name = "Prague"},
                Distance = 1204
            }, 
            new Vertex(){ 
                DestinationCity = new City(){ Id = 16, Location = new Location(){ Latitude = 49.49437, Longitude = 0.107929}, Name = "Le Havre"}, 
                SourceCity = new City(){Id = 24, Location = new Location(){ Latitude = 48.1351253, Longitude = 11.5819806}, Name = "Munich"},
                Distance = 1038
            }, 
            new Vertex(){ 
                DestinationCity = new City(){ Id = 14, Location = new Location(){ Latitude = 44.4056499, Longitude = 8.946256}, Name = "Genoa"}, 
                SourceCity = new City(){Id = 34, Location = new Location(){ Latitude = 45.07098200000001, Longitude = 7.685676}, Name = "Turin"},
                Distance = 186
            }, 
            new Vertex(){ 
                DestinationCity = new City(){ Id = 12, Location = new Location(){ Latitude = 50.1109221, Longitude = 8.6821267}, Name = "Frankfurt"}, 
                SourceCity = new City(){Id = 30, Location = new Location(){ Latitude = 51.92421599999999, Longitude = 4.481776}, Name = "Rotterdam"},
                Distance = 444
            }, 
        };
    }
}