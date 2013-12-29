using System;
using System.Collections.Generic;
using System.Linq;
using FancyTraveller.Domain.Logic;
using FancyTraveller.Domain.Model;
using FancyTraveller.Domain.POCO;

namespace FancyTraveller.Domain.Services
{
    public class RouteService : IRouteService
    {
        private readonly IVertexRepository vertexRepository;
        private readonly IRouteFinder routeFinder;

        public RouteService(IVertexRepository vertexRepository, IRouteFinder routeFinder)
        {
            this.vertexRepository = vertexRepository;
            this.routeFinder = routeFinder;
        }

        #region Implementation of IRouteService

        public IEnumerable<string> AvailableCities
        {
            get
            {
                return ReadAllCities(c => c.DestinationCity.Name).Concat(ReadAllCities(c => c.SourceCity.Name)).Distinct().OrderBy(s => s);
            }
        }

        public int FindShortestRoute(int source, int destination, int numberOfAllVertices, IDictionary<int, IList<Vertex>> vertices)
        {
            return routeFinder.FindShortestRoute(source, destination, vertices);
        }

        public Location GetLocationOf(string city)
        {
            var vertex = vertexRepository.GetAll().FirstOrDefault(v => v.DestinationCity.Name == city || v.SourceCity.Name == city);

            if(vertex == null)
                return new Location(){ Latitude = 0, Longitude = 0 };

            if (vertex.SourceCity.Name == city)
                return vertex.SourceCity.Location;

            return vertex.DestinationCity.Location;
        }

        public IDictionary<int, IList<Vertex>> LoadDistancesBetweenCities(IList<string> citiesToSkip)
        {
            var dataFromCitiesFile = vertexRepository.GetAll();
            IDictionary<int, IList<Vertex>> listOfNeighboursDistance = new Dictionary<int, IList<Vertex>>();

            foreach (var d in dataFromCitiesFile)
            {
                if(ShouldBeSkipped(d, citiesToSkip)) continue;
                
                var nextVertex = new Vertex() { SourceCity = new City() { Id = d.SourceCity.Id, Name = d.SourceCity.Name }, DestinationCity = new City() { Id = d.DestinationCity.Id, Name = d.DestinationCity.Name }, Distance = d.Distance };

                if (listOfNeighboursDistance.ContainsKey(d.SourceCity.Id))
                    listOfNeighboursDistance[d.SourceCity.Id].Add(nextVertex);
                else
                    listOfNeighboursDistance.Add(d.SourceCity.Id, new List<Vertex>() { nextVertex });
            }

            return listOfNeighboursDistance;
        }

        private bool ShouldBeSkipped(Vertex vertex, IList<string> citiesToSkip)
        {
            return citiesToSkip.Contains(vertex.DestinationCity.Name) || citiesToSkip.Contains(vertex.SourceCity.Name);
        }

        private IEnumerable<Vertex> VerticesWithoutCititesToSkip(IEnumerable<string> citiesToSkip)
        {
            return
                vertexRepository.GetAll()
                    .Where(v => !citiesToSkip.Contains(v.DestinationCity.Name) && !citiesToSkip.Contains(v.SourceCity.Name))
                    .Select(v => v);
        }

        private IEnumerable<string> ReadAllCities(Func<Vertex, string> field)
        {
            return vertexRepository.GetAll().Select(field);
        }

        #endregion
    }
}