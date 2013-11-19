using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web.Script.Serialization;
using NUnit.Framework;
using Should;

namespace FancyTraveller.Domain.Tests.Integration.Tests
{
    [TestFixture]
    public class RouteServiceTests
    {
        private IRouteService service;

        [SetUp]
        public void InitializeContext()
        {
            this.service = new RouteService(new VerticesRepository(ConfigurationManager.AppSettings));
        }

        [Test]
        public void get_available_cities__data_context_exists__list_with_all_available_cities_is_returned()
        {
            var result = service.AvailableCities;

            var expectedListOfCitites = RouteServiceTestingData.AvailableCities;
            
            result.ShouldEqual(expectedListOfCitites);
        }
    }

    public class RouteService : IRouteService
    {
        public RouteService(IVertexRepository vertexRepository)
        {
        }

        #region Implementation of IRouteService

        public IEnumerable<string> AvailableCities { get; private set; }

        #endregion
    }

    public class Location
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class City
    {
        public string Name { get; set; }
        public Location Location { get; set; }
    }

    public class Vertex
    {
        public City SourceCity { get; set; }
        public City DestinationCity { get; set; }
        public int Distance { get; set; }
    }

    public interface IVertexRepository
    {
        IEnumerable<Vertex> GetAll();
    }

    public class VerticesRepository : IVertexRepository
    {
        private readonly IDataReader dataReader;
        private IEnumerable<Vertex> allVertices;

        private const string DataFileKey = "citiesDataFile";
        private string dataFilePath;

        public VerticesRepository(NameValueCollection appSettings, IDataReader dataReader)
        {
            this.dataReader = dataReader;
            dataFilePath = ReadFile(appSettings);
        }

        private IEnumerable<Vertex> ReadFilePath(NameValueCollection appSettings)
        {
            if(appSettings == null || appSettings.Get(DataFileKey) == null)
                throw new ConfigurationErrorsException(string.Format("Path to the data file under '{0}' key is not set in configuration.", DataFileKey));

            var dataInJson = dataReader.ReadData(appSettings.Get(DataFileKey));

            var deserializedJson = DeserializeJson(dataInJson);
        }

        private dynamic DeserializeJson(string dataInJson)
        {
            return new JavaScriptSerializer().Deserialize<dynamic>(dataInJson);
        }

        #region Implementation of IVertexRepository

        public IEnumerable<Vertex> GetAll()
        {
            yield break;
        }

        #endregion
    }

    public interface IDataReader
    {
        string ReadData(string resource);
    }

    public class RouteServiceTestingData
    {
        public class City
        {
            public string Name { get; set; }
        }

        static RouteServiceTestingData()
        {
            var availableCitiesJson = File.ReadAllText("App_Data/availableCities.txt");

            AvailableCities = DeserializeJsonToEnumerable(availableCitiesJson);
        }

        private static IEnumerable<string> DeserializeJsonToEnumerable(string availableCitiesJson)
        {
            var serializer = new JavaScriptSerializer();

            return serializer.Deserialize<List<City>>(availableCitiesJson).Select(c => c.Name).ToList();
        }

        public static IEnumerable<string> AvailableCities { get; private set; }
    }

    internal interface IRouteService
    {
        IEnumerable<string> AvailableCities { get; }
    }
}
