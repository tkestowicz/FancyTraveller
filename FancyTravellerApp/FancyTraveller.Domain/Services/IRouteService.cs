using System.Collections.Generic;
using FancyTraveller.Domain.POCO;

namespace FancyTraveller.Domain.Services
{
    public interface IRouteService
    {
        IEnumerable<string> AvailableCities { get; }

        //TODO: change tests!!
        IEnumerable<City> AvailableCitiesNew { get; } 

        IList<int> FindShortestRoute(int source, int destination, IDictionary<int, IList<Vertex>> vertices);
        IDictionary<int, IList<Vertex>> LoadDistancesBetweenCities(IList<string> citiesToSkip);
        Location GetLocationOf(string city);
    }
}