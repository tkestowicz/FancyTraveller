using System.Collections.Generic;
using FancyTraveller.Domain.POCO;

namespace FancyTraveller.Domain.Logic
{
    public interface IRouteFinder
    {
        int FindShortestRoute(string source, string destination, IEnumerable<Vertex> vertices);
    }
}