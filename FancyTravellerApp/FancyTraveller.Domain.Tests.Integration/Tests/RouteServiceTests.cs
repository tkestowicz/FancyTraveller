using System.Configuration;
using System.Linq;
using System.Web.Script.Serialization;
using FancyTraveller.Domain.Infrastracture;
using FancyTraveller.Domain.Model;
using FancyTraveller.Domain.Services;
using FancyTraveller.Domain.Tests.Integration.Helpers;
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
            var noCititesToSkip = Enumerable.Empty<string>();

            var result = service.DistancesBetweenCitites(noCititesToSkip);

            var expectedListOfVerticies = RouteServiceTestingData.Verticies;

            result.ToJson().ShouldEqual(expectedListOfVerticies.ToJson());
        }

        [Test]
        public void get_verticies___cities_to_skip_passed___list_of_verticies_without_cities_to_skip()
        {
            var cititesToSkip = new[] { "Amsterdam", "Madrid", "Turin" };

            var result = service.DistancesBetweenCitites(cititesToSkip);

            var allVerticies = RouteServiceTestingData.Verticies;
            var expectedListOfVerticies =
                allVerticies.Where(v => cititesToSkip.Contains(v.DestinationCity.Name) == false)
                    .Where(v => cititesToSkip.Contains(v.SourceCity.Name) == false).Select(v => v);

            result.ToJson().ShouldEqual(expectedListOfVerticies.ToJson());
        }
    }
}
