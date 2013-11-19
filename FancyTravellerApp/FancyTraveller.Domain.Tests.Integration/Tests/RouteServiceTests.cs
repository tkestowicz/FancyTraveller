using System.Configuration;
using System.Linq;
using System.Web.Script.Serialization;
using FancyTraveller.Domain.Infrastracture;
using FancyTraveller.Domain.Model;
using FancyTraveller.Domain.Services;
using FancyTraveller.Domain.Tests.Integration.TestingData;
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
            this.service = new RouteService(new VerticesRepository(ConfigurationManager.AppSettings, new FileReader()));
        }

        [Test]
        public void get_available_cities__happy_path__list_of_available_cities_is_returned()
        {
            var result = service.AvailableCities;

            var expectedListOfCitites = RouteServiceTestingData.AvailableCities;
            
            result.ShouldEqual(expectedListOfCitites);
        }

        [Test]
        public void get_all_verticies___happy_path_with_no_citites_to_skip___list_of_verticies_is_returned()
        {
            var result = service.DistancesBetweenCitites(Enumerable.Empty<string>());

            var expectedListOfVerticies = RouteServiceTestingData.Verticies;

            var serializer = new JavaScriptSerializer();
            var serializedResult = serializer.Serialize(result);
            var serializedExptectedResult = serializer.Serialize(expectedListOfVerticies);

            serializedResult.ShouldEqual(serializedExptectedResult);
        }
    }
}
