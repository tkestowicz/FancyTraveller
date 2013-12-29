using System.Collections.Generic;
using FancyTraveller.Domain.POCO;

namespace FancyTraveller.Domain.Services
{
    public interface IRouteService
    {
        IEnumerable<string> AvailableCities { get; }
        int FindShortestRoute(int source, int destination, int numberOfAllVertices, IDictionary<int, IList<Vertex>> vertices);
        IDictionary<int, IList<Vertex>> LoadDistancesBetweenCities(IList<string> citiesToSkip);
        Location GetLocationOf(string city);
    }
}