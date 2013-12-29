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
        private List<double> listOfDistances = new List<double>();
        private Dictionary<double, double> listOfDistancesTemp = new Dictionary<double, double>();
        private Queue<int> allVerticesQueue = new Queue<int>();

        //public int FindShortestRoute(int sourceTop, int destinationTop, int allVertices, IEnumerable<IEnumerable<Vertex>> vertices)
        //{
        //    List<List<Vertex>> neighbourDistances = vertices as List<List<Vertex>>;
        //    for (int i = 0; i < allVertices; ++i)
        //    {
        //        listOfDistances.Add(i);
        //        listOfDistances[i] = double.PositiveInfinity;
        //        allVerticesQueue.Enqueue(i);
        //    }

        //    listOfDistances[sourceTop] = 0;

        //    do
        //    {
        //        verticePickedFromQueue = allVerticesQueue.Peek();

        //        if (verticePickedFromQueue != 0)
        //        {
        //            for (int i = 0; i < neighbourDistances[verticePickedFromQueue].Count; ++i)
        //            {

        //                verticeNeighbour = neighbourDistances[verticePickedFromQueue][i].DestinationCity.CityId;
        //                toNeighbourCost = neighbourDistances[verticePickedFromQueue][i].Distance;

        //                if (listOfDistances[verticeNeighbour] > listOfDistances[verticePickedFromQueue] + toNeighbourCost)
        //                {
        //                    listOfDistances[verticeNeighbour] = listOfDistances[verticePickedFromQueue] + toNeighbourCost;
        //                    allVerticesQueue.Enqueue(verticeNeighbour);
        //                }
        //            }
        //        }

        //        allVerticesQueue.Dequeue();

        //    } while (allVerticesQueue.Count > 0);

        //    return Convert.ToInt32(listOfDistances[destinationTop]);
        //}

        public int FindShortestRoute(int sourceTop, int destinationTop, int allVertices, IEnumerable<IEnumerable<Vertex>> vertices)
        {
            List<List<Vertex>> neighbourDistances = vertices as List<List<Vertex>>;
            for (int i = 0; i < allVertices; ++i)
            {
                listOfDistancesTemp.Add(i, double.PositiveInfinity);
                allVerticesQueue.Enqueue(i);
            }

            listOfDistancesTemp[sourceTop] = 0;

            do
            {
                verticePickedFromQueue = allVerticesQueue.Peek();

                if (verticePickedFromQueue != 0)
                {
                    for (int i = 0; i < neighbourDistances[verticePickedFromQueue].Count; ++i)
                    {

                        verticeNeighbour = neighbourDistances[verticePickedFromQueue][i].DestinationCity.Id;
                        toNeighbourCost = neighbourDistances[verticePickedFromQueue][i].Distance;

                        if (listOfDistancesTemp[verticeNeighbour] > listOfDistancesTemp[verticePickedFromQueue] + toNeighbourCost)
                        {
                            listOfDistancesTemp[verticeNeighbour] = listOfDistancesTemp[verticePickedFromQueue] + toNeighbourCost;
                            allVerticesQueue.Enqueue(verticeNeighbour);
                        }
                    }
                }

                allVerticesQueue.Dequeue();

            } while (allVerticesQueue.Count > 0);

            return Convert.ToInt32(listOfDistancesTemp[destinationTop]);
        }

    }
}