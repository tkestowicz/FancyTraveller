using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using FancyTraveller.Domain.Infrastracture;
using FancyTraveller.Domain.Logic;
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
            this.service = new RouteService(new VerticesRepository(ConfigurationManager.AppSettings, new FileReader()), new DijkstraRouteFinder());
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
            IList<string> noCititesToSkip = new string[0];

            var result = service.LoadDistancesBetweenCities(noCititesToSkip);

            var expectedListOfVerticies = RouteServiceTestingData.Vertices;

            result.ToJson().ShouldEqual(expectedListOfVerticies.ToJson());
        }

        [Test]
        public void get_verticies___cities_to_skip_passed___list_of_verticies_without_cities_to_skip()
        {
            var cititesToSkip = new[] { "Amsterdam", "Madrid", "Turin" };

            var result = service.LoadDistancesBetweenCities(cititesToSkip);

            var allVerticies = RouteServiceTestingData.Vertices;
            var expectedListOfVerticies =
                allVerticies.Where(v => cititesToSkip.Contains(v.DestinationCity.Name) == false)
                    .Where(v => cititesToSkip.Contains(v.SourceCity.Name) == false).Select(v => v);

            result.ToJson().ShouldEqual(expectedListOfVerticies.ToJson());
        }

        [TestCase("Madrid", 40.4167754, -3.7037902)]
        [TestCase("Cologne", 50.937531, 6.9602786)]
        [TestCase("Zurich", 47.3686498, 8.539182499999999)]
        [TestCase("NoExistingLocation", 0, 0)]
        public void get_location___happy_path___location_is_returned(string city, double expectedLatitude, double expectedLongitude)
        {
            var result = service.GetLocationOf(city);

            result.Latitude.ShouldEqual(expectedLatitude);
            result.Longitude.ShouldEqual(expectedLongitude);
        }
    }
}
