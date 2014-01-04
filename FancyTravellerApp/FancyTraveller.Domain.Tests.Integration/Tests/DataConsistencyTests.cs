using System.Configuration;
using System.Linq;
using FancyTraveller.Domain.Infrastracture;
using FancyTraveller.Domain.Logic;
using FancyTraveller.Domain.Model;
using FancyTraveller.Domain.POCO;
using FancyTraveller.Domain.Services;
using FancyTraveller.Domain.Tests.Integration.TestingData;
using NUnit.Framework;
using Should;

namespace FancyTraveller.Domain.Tests.Integration.Tests
{
    [TestFixture]
    public class DataConsistencyTests
    {
        private IVertexRepository repository;
        private RouteService service;

        [SetUp]
        public void initialize_repository()
        {
            repository = new VerticesRepository(ConfigurationManager.AppSettings, new FileReader());
            service = new RouteService(new VerticesRepository(ConfigurationManager.AppSettings, new FileReader()), new DijkstraRouteFinder());
        }

        [Test]
        public void get_all___vertices_loaded_within_service___each_list_should_has_the_same_length()
        {
            var result = service.LoadDistancesBetweenCities(new int[0]);

            var expectedLengthOfEachList = result.First().Value.Count; 

            foreach (var entry in result)
            {
                entry.Value.Count.ShouldEqual(expectedLengthOfEachList);
            }
        }

        [Test]
        public void get_all___vertices_passed_where_source_and_destination_are_the_same___pair_should_not_exist()
        {
            var vertices = repository.GetAll();

            var result = vertices.Count(v => v.SourceCity.Equals(v.DestinationCity));

            const int expectedNumberOfTheSamePairs = 0;

            result.ShouldEqual(expectedNumberOfTheSamePairs);
        }

        [Test, TestCaseSource(typeof(RouteServiceTestingData), RouteServiceTestingData.DifferentPairsCollection)]
        public void get_all___a_pair_of_two_cities_is_given___two_oposite_pairs_should_exist_in_repository(Vertex pair)
        {
            var firstPair = repository.GetAll().First(v => v.Equals(pair));
            var opositePair = repository.GetAll().First(v => v.DestinationCity.Equals(pair.SourceCity) && v.SourceCity.Equals(pair.DestinationCity));

            firstPair.SourceCity.ShouldEqual(opositePair.DestinationCity);
            firstPair.DestinationCity.ShouldEqual(opositePair.SourceCity);
            firstPair.Distance.ShouldEqual(opositePair.Distance);
        }
    }
}
