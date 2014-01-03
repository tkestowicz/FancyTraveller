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
    }
}