using System.Collections.Generic;
using FancyTraveller.Domain.POCO;

namespace FancyTraveller.Domain.Logic
{
    public interface IRouteFinder
    {
        IList<int> FindShortestRoute(int sourceTop, int destinationTop, IDictionary<int, IList<Vertex>> vertices);
    }
}