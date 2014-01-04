using System.Collections.Generic;
using FancyTraveller.Domain.POCO;

namespace FancyTraveller.Domain.Services
{
    public interface IRouteService
    {
        IList<City> AvailableCities { get; } 
        Result FindShortestRoute(int source, int destination, IDictionary<int, IList<Vertex>> vertices);
        IDictionary<int, IList<Vertex>> LoadDistancesBetweenCities(int[] citiesToSkip);
    }
}