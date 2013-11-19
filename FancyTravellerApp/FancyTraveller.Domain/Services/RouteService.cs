using System;
using System.Collections.Generic;
using System.Linq;
using FancyTraveller.Domain.Model;
using FancyTraveller.Domain.POCO;

namespace FancyTraveller.Domain.Services
{
    public class RouteService : IRouteService
    {
        private readonly IVertexRepository vertexRepository;

        public RouteService(IVertexRepository vertexRepository)
        {
            this.vertexRepository = vertexRepository;
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
            return vertexRepository.GetAll();
        }

        private IEnumerable<string> ReadAllCities(Func<Vertex, string> field)
        {
            return vertexRepository.GetAll().Select(field);
        }

        #endregion
    }
}