using System.Collections.Generic;
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
            service = new RouteService(repositoryMock, new DijkstraRouteFinder());
        }


        [Test]
        public void get_shortest_route___with_no_cities_to_skip___shortest_path_is_returned()
        {
            const int source = 2;
            const int destination = 32;
            
            var preparedData = new List<Vertex>()
            {
                new Vertex(){ SourceCity = new City(){ Id = 2, Name = "Antwerp" }, DestinationCity = new City(){ Id = 8, Name = "Calais"}, Distance = 211},
                new Vertex(){ SourceCity = new City(){ Id = 8, Name = "Calais" }, DestinationCity = new City(){ Id = 2, Name = "Antwerp"}, Distance = 211},
                new Vertex(){ SourceCity = new City(){ Id = 2, Name = "Antwerp" }, DestinationCity = new City(){ Id = 6, Name = "Bern"}, Distance = 704},
                new Vertex(){ SourceCity = new City(){ Id = 6, Name = "Bern" }, DestinationCity = new City(){ Id = 2, Name = "Antwerp"}, Distance = 704},
                new Vertex(){ SourceCity = new City(){ Id = 2, Name = "Antwerp" }, DestinationCity = new City(){ Id = 12, Name = "Frankfurt"}, Distance = 427},
                new Vertex(){ SourceCity = new City(){ Id = 12, Name = "Frankfurt" }, DestinationCity = new City(){ Id = 2, Name = "Antwerp"}, Distance = 427},
                new Vertex(){ SourceCity = new City(){ Id = 12, Name = "Frankfurt" }, DestinationCity = new City(){ Id = 32, Name = "Stuttgart"}, Distance = 205},
                new Vertex(){ SourceCity = new City(){ Id = 32, Name = "Stuttgart" }, DestinationCity = new City(){ Id = 12, Name = "Frankfurt"}, Distance = 205},
                new Vertex(){ SourceCity = new City(){ Id = 8, Name = "Calais" }, DestinationCity = new City(){ Id = 12, Name = "Frankfurt"}, Distance = 621},
                new Vertex(){ SourceCity = new City(){ Id = 12, Name = "Frankfurt" }, DestinationCity = new City(){ Id = 8, Name = "Calais"}, Distance = 621},
                new Vertex(){ SourceCity = new City(){ Id = 12, Name = "Frankfurt" }, DestinationCity = new City(){ Id = 6, Name = "Bern"}, Distance = 424},
                new Vertex(){ SourceCity = new City(){ Id = 6, Name = "Bern" }, DestinationCity = new City(){ Id = 12, Name = "Frankfurt"}, Distance = 424},
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

            A.CallTo(() => repositoryMock.GetAll()).Returns(preparedData);
             
            var result = service.FindShortestRoute(source, destination, listOfNeighboursDistance);

            const int expectedDistance = 632;
            var expectedCitiesToBeVisited = new List<City>()
            {
                new City() {Id = 12, Name = "Frankfurt"}
            };

            result.Distance.ShouldEqual(expectedDistance);
            result.VisitedCities.ShouldEqual(expectedCitiesToBeVisited);
        }
    }
}
