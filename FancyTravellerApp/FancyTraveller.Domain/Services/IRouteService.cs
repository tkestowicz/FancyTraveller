using System.Collections.Generic;
using FancyTraveller.Domain.POCO;

namespace FancyTraveller.Domain.Services
{
    public interface IRouteService
    {
        IEnumerable<string> AvailableCities { get; }
        IEnumerable<Vertex> DistancesBetweenCitites(IEnumerable<string> listOfCititesToSkip);
    }
}