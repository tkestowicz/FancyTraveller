using System.Collections.Generic;
using FancyTraveller.Domain.POCO;

namespace FancyTraveller.Domain.Model
{
    public interface IVertexRepository
    {
        IEnumerable<Vertex> GetAll();
    }
}