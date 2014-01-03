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
        private IEnumerable<City> availableCitiesNew;

        public RouteService(IVertexRepository vertexRepository, IRouteFinder routeFinder)
        {
            this.vertexRepository = vertexRepository;
            this.routeFinder = routeFinder;
        }

        #region Implementation of IRouteService

        public IList<City> AvailableCities
        {
            get { return vertexRepository.GetAll().Select(vertex => vertex.SourceCity).Distinct(new CityEqualityComparer()).ToList(); }
        }

        public IList<int> FindShortestRoute(int source, int destination, IDictionary<int, IList<Vertex>> vertices)
        {
            return routeFinder.FindShortestRoute(source, destination, vertices);
        }

        public IDictionary<int, IList<Vertex>> LoadDistancesBetweenCities(int[] citiesToSkip)
        {
            var dataFromCitiesFile = vertexRepository.GetAll();
            IDictionary<int, IList<Vertex>> listOfNeighboursDistance = new Dictionary<int, IList<Vertex>>();

            foreach (var d in dataFromCitiesFile)
            {
                if(ShouldBeSkipped(d, citiesToSkip)) continue;
                
                if (listOfNeighboursDistance.ContainsKey(d.SourceCity.Id))
                    listOfNeighboursDistance[d.SourceCity.Id].Add(d);
                else
                    listOfNeighboursDistance.Add(d.SourceCity.Id, new List<Vertex>() { d });
            }

            return listOfNeighboursDistance;
        }

        private bool ShouldBeSkipped(Vertex vertex, int[] citiesToSkip)
        {
            return citiesToSkip.Contains(vertex.DestinationCity.Id) || citiesToSkip.Contains(vertex.SourceCity.Id);
        }

        #endregion
    }
}