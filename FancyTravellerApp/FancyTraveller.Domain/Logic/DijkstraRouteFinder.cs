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
        private Queue<int> allVerticesQueue = new Queue<int>();

        public int FindShortestRoute(int sourceTop, int destinationTop, int allVertices, IEnumerable<IEnumerable<Vertex>> vertices)
        {
            List<List<Vertex>> neighbourDistances = vertices as List<List<Vertex>>;
            for (int i = 0; i < allVertices; ++i)
            {
                listOfDistances.Add(i);
                listOfDistances[i] = double.PositiveInfinity;
                allVerticesQueue.Enqueue(i);
            }

            listOfDistances[sourceTop] = 0;

            do
            {
                verticePickedFromQueue = allVerticesQueue.Peek();

                if (verticePickedFromQueue != 0)
                {
                    for (int i = 0; i < neighbourDistances[verticePickedFromQueue].Count; ++i)
                    {

                        verticeNeighbour = neighbourDistances[verticePickedFromQueue][i].DestinationCity.Id;
                        toNeighbourCost = neighbourDistances[verticePickedFromQueue][i].Distance;

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

        //private string FindShortestPath(int sourceTop, int allVertices, List<List<Tuple<int, int>>> listOfNeighboursDistance, int destinationTop)
        //{
        //for (int i = 0; i < allVertices; ++i)
        //{
        //    listOfDistances.Add(i);
        //    listOfDistances[i] = double.PositiveInfinity;
        //    allVerticesQueue.Enqueue(i);
        //}

        //listOfDistances[sourceTop] = 0;

        //do
        //{
        //    verticePickedFromQueue = allVerticesQueue.Peek();

        //    if (verticePickedFromQueue != 0)
        //    {
        //        for (int i = 0; i < listOfNeighboursDistance[verticePickedFromQueue].Count; ++i)
        //        {

        //            verticeNeighbour = listOfNeighboursDistance[verticePickedFromQueue][i].Item1;
        //            toNeighbourCost = listOfNeighboursDistance[verticePickedFromQueue][i].Item2;

        //            if (listOfDistances[verticeNeighbour] > listOfDistances[verticePickedFromQueue] + toNeighbourCost)
        //            {
        //                listOfDistances[verticeNeighbour] = listOfDistances[verticePickedFromQueue] + toNeighbourCost;
        //                allVerticesQueue.Enqueue(verticeNeighbour);
        //            }
        //        }
        //    }

        //    allVerticesQueue.Dequeue();

        //} while (allVerticesQueue.Count > 0);

        //return "Shortest road from " + sourceTop + " to " + destinationTop + " is: " + listOfDistances[destinationTop].ToString() + " km";
        //}

    }
}