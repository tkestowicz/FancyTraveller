using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using FancyTraveller.Domain.Logic;
using FancyTraveller.Domain.Model;
using FancyTraveller.Domain.POCO;
using FancyTraveller.Domain.Services;
using NUnit.Framework;
using Should;

namespace FancyTraveller.Domain.Tests.Unit.Tests
{
    [TestFixture]
    public class RouteServiceTests_Using_Dijkstra
    {
        private IRouteService service;
        private IVertexRepository repositoryMock;


        [SetUp]
        public void InitializeContext()
        {
            repositoryMock = A.Fake<IVertexRepository>();
            this.service = new RouteService(repositoryMock, new DijkstraRouteFinder());
        }


        [Test]
        public void get_shortest_route___with_no_cities_to_skip___shortest_path_is_returned()
        {
            const string source = "Amterdam";
            const string destination = "Wroclaw";
            var preparedData = new List<Vertex>(4)
            {
                new Vertex(){ SourceCity = new City(){ Name = "Amsterdam" }, DestinationCity = new City(){ Name = "Wroclaw"}, Distance = 1200},
                new Vertex(){ SourceCity = new City(){ Name = "Amsterdam" }, DestinationCity = new City(){ Name = "Hannover"}, Distance = 450},
                new Vertex(){ SourceCity = new City(){ Name = "Hannover" }, DestinationCity = new City(){ Name = "Wroclaw"}, Distance = 500},
                new Vertex(){ SourceCity = new City(){ Name = "Amsterdam" }, DestinationCity = new City(){ Name = "Berlin"}, Distance = 900},
                new Vertex(){ SourceCity = new City(){ Name = "Berlin" }, DestinationCity = new City(){ Name = "Wroclaw"}, Distance = 350}
            };

            var noCititesToSkip = Enumerable.Empty<string>();

            A.CallTo(() => repositoryMock.GetAll()).Returns(preparedData);
            
            var result = service.FindShortestRoute(source, destination, noCititesToSkip);

            const int expectedDistance = 950;

            result.ShouldEqual(expectedDistance);
        }

        [Test]
        public void get_shortest_route___with_city_to_skip___shortest_path_is_returned()
        {
            const string source = "Amterdam";
            const string destination = "Wroclaw";
            var preparedData = new List<Vertex>(4)
            {
                new Vertex(){ SourceCity = new City(){ Name = "Amsterdam" }, DestinationCity = new City(){ Name = "Wroclaw"}, Distance = 1200},
                new Vertex(){ SourceCity = new City(){ Name = "Amsterdam" }, DestinationCity = new City(){ Name = "Hannover"}, Distance = 450},
                new Vertex(){ SourceCity = new City(){ Name = "Hannover" }, DestinationCity = new City(){ Name = "Wroclaw"}, Distance = 500},
                new Vertex(){ SourceCity = new City(){ Name = "Amsterdam" }, DestinationCity = new City(){ Name = "Berlin"}, Distance = 900},
                new Vertex(){ SourceCity = new City(){ Name = "Berlin" }, DestinationCity = new City(){ Name = "Wroclaw"}, Distance = 350}
            };
            var cititesToSkip = new[] { "Hannover" };

            A.CallTo(() => repositoryMock.GetAll()).Returns(preparedData);

            var result = service.FindShortestRoute(source, destination, cititesToSkip);

            const int expectedDistance = 1200;

            result.ShouldEqual(expectedDistance);
        }
    }
}
