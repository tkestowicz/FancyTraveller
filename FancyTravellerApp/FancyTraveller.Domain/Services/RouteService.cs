﻿using System;
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
            
            return VerticiesWithoutCititesToSkip(listOfCititesToSkip);
        }

        public int FindShortestRoute(string source, string destination, IEnumerable<string> cititesToSkip)
        {
            return routeFinder.FindShortestRoute(source, destination, DistancesBetweenCitites(cititesToSkip));
        }

        private IEnumerable<Vertex> VerticiesWithoutCititesToSkip(IEnumerable<string> citiesToSkip)
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