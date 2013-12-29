using System;
using System.Collections.Generic;
using System.Linq;
using FancyTraveller.Domain.POCO;

namespace FancyTraveller.Domain.Logic
{
    public class DijkstraRouteFinder : IRouteFinder
    {
        private int verticeNeighbour;
        private int toNeighbourCost;
        private int verticePickedFromQueue;
        private readonly Dictionary<int, double> listOfDistances = new Dictionary<int, double>();
        private readonly Queue<int> allVerticesQueue = new Queue<int>();

        public int FindShortestRoute(int sourceTop, int destinationTop, IDictionary<int, IList<Vertex>> vertices)
        {
            foreach (var cityId in vertices.Keys)
            {
                listOfDistances.Add(cityId, double.PositiveInfinity);
                allVerticesQueue.Enqueue(cityId);
            }
            
            listOfDistances[sourceTop] = 0;

            do
            {
                verticePickedFromQueue = allVerticesQueue.Peek();

                if (verticePickedFromQueue != 0)
                {
                    for (int i = 0; i < vertices[verticePickedFromQueue].Count(); ++i)
                    {
                        verticeNeighbour = vertices[verticePickedFromQueue].ElementAt(i).DestinationCity.Id;

                        toNeighbourCost = vertices[verticePickedFromQueue].ElementAt(i).Distance;

                        if (listOfDistances[verticeNeighbour] > listOfDistances[verticePickedFromQueue] + toNeighbourCost)
                        {
                            listOfDistances[verticeNeighbour] = listOfDistances[verticePickedFromQueue] + toNeighbourCost;
                            allVerticesQueue.Enqueue(verticeNeighbour);
                        }
                    }
                }

                allVerticesQueue.Dequeue();

            } while (allVerticesQueue.Count > 0);

            return Convert.ToInt32(listOfDistances[destinationTop]);
        }

    }
}