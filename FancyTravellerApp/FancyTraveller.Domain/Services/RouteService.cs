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

        public IEnumerable<Vertex> DistancesBetweenCitites(IEnumerable<string> listOfCititesToSkip)
        {
            if (listOfCititesToSkip == null || !listOfCititesToSkip.Any())
                return vertexRepository.GetAll();
            
            return VerticesWithoutCititesToSkip(listOfCititesToSkip);
        }

        public int FindShortestRoute(int source, int destination, int numberOfAllVertices, IEnumerable<IEnumerable<Vertex>> vertices/*IEnumerable<string> cititesToSkip*/)
        {
            return routeFinder.FindShortestRoute(source, destination, numberOfAllVertices, vertices/*DistancesBetweenCitites(cititesToSkip)*/);
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

        public IEnumerable<IEnumerable<Vertex>> LoadDistancesBetweenCities()
        {
            throw new NotImplementedException();
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