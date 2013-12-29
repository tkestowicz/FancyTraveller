using System.Collections.Generic;
using FancyTraveller.Domain.POCO;

namespace FancyTraveller.Domain.Services
{
    public interface IRouteService
    {
        IEnumerable<string> AvailableCities { get; }
        IEnumerable<Vertex> DistancesBetweenCitites(IEnumerable<string> listOfCititesToSkip);
        int FindShortestRoute(int source, int destination, int numberOfAllVertices, IEnumerable<IEnumerable<Vertex>> vertices);//IEnumerable<string> cititesToSkip);
        IEnumerable<IEnumerable<Vertex>> LoadDistancesBetweenCities();
        Location GetLocationOf(string city);
    }
}