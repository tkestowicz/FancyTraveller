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
            const int source = 1;
            const int destination = 2;
            const int numberOfCities = 4;
            var preparedData = new List<Vertex>()
            {
                new Vertex(){ SourceCity = new City(){ Id = 1, Name = "Amsterdam" }, DestinationCity = new City(){ Id = 2, Name = "Wroclaw"}, Distance = 1200},
                new Vertex(){ SourceCity = new City(){ Id = 2, Name = "Wroclaw" }, DestinationCity = new City(){ Id = 1, Name = "Amsterdam"}, Distance = 1200},
                new Vertex(){ SourceCity = new City(){ Id = 1, Name = "Amsterdam" }, DestinationCity = new City(){ Id = 3, Name = "Hannover"}, Distance = 450},
                new Vertex(){ SourceCity = new City(){ Id = 3, Name = "Hannover" }, DestinationCity = new City(){ Id = 1, Name = "Amsterdam"}, Distance = 450},
                new Vertex(){ SourceCity = new City(){ Id = 3, Name = "Hannover" }, DestinationCity = new City(){ Id = 2, Name = "Wroclaw"}, Distance = 500},
                new Vertex(){ SourceCity = new City(){ Id = 2, Name = "Wroclaw" }, DestinationCity = new City(){ Id = 3, Name = "Hannover"}, Distance = 500},
                new Vertex(){ SourceCity = new City(){ Id = 1, Name = "Amsterdam" }, DestinationCity = new City(){ Id = 4, Name = "Berlin"}, Distance = 900},
                new Vertex(){ SourceCity = new City(){ Id = 4, Name = "Berlin" }, DestinationCity = new City(){ Id = 1, Name = "Amsterdam"}, Distance = 900},
                new Vertex(){ SourceCity = new City(){ Id = 4, Name = "Berlin" }, DestinationCity = new City(){ Id = 2, Name = "Wroclaw"}, Distance = 350},
                new Vertex(){ SourceCity = new City(){ Id = 2, Name = "Wroclaw" }, DestinationCity = new City(){ Id = 4, Name = "Berlin"}, Distance = 350}
            };

            IDictionary<int, IList<Vertex>> listOfNeighboursDistance = new Dictionary<int, IList<Vertex>>();

            foreach(var p in preparedData)
            {
                var nextVertex = new Vertex() { SourceCity = new City(){ Id = p.SourceCity.Id, Name = p.SourceCity.Name }, DestinationCity = new City(){ Id = p.DestinationCity.Id, Name = p.DestinationCity.Name}, Distance = p.Distance};

                if (listOfNeighboursDistance.ContainsKey(p.SourceCity.Id))
                    listOfNeighboursDistance[p.SourceCity.Id].Add(nextVertex);
                else
                    listOfNeighboursDistance.Add(p.SourceCity.Id, new List<Vertex>() { nextVertex });
            }
          
            var noCititesToSkip = Enumerable.Empty<string>();

            A.CallTo(() => repositoryMock.GetAll()).Returns(preparedData);
             
            var result = service.FindShortestRoute(source, destination, listOfNeighboursDistance);//noCititesToSkip);

            const int expectedDistance = 950;

            int item = result[result.Count - 1];
            item.ShouldEqual(expectedDistance);
        }
    }
}
