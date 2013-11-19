using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;
using FancyTraveller.Domain.POCO;

namespace FancyTraveller.Domain.Tests.Integration.TestingData
{
    public class RouteServiceTestingData
    {
        public class City
        {
            public string Name { get; set; }
        }

        static RouteServiceTestingData()
        {
            var availableCitiesJson = File.ReadAllText("App_Data/availableCities.txt");
            var verticiesJson = File.ReadAllText("App_Data/cities.txt");

            Verticies = DeserializeVerticiesJsonToEnumerable(verticiesJson);
            AvailableCities = DeserializeCitiesJsonToEnumerable(availableCitiesJson);
        }

        public static IEnumerable<Vertex> Verticies { get; set; }

        private static IEnumerable<string> DeserializeCitiesJsonToEnumerable(string availableCitiesJson)
        {
            var serializer = new JavaScriptSerializer();

            return serializer.Deserialize<List<City>>(availableCitiesJson).Select(c => c.Name).ToList().OrderBy(s => s);
        }

        private static IEnumerable<Vertex> DeserializeVerticiesJsonToEnumerable(string verticiesJson)
        {
            var serializer = new JavaScriptSerializer();

            return serializer.Deserialize<List<Vertex>>(verticiesJson).ToList();
        }

        public static IEnumerable<string> AvailableCities { get; private set; }
    }
}