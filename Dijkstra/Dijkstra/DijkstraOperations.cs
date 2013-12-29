using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dijkstra
{
    public class DijkstraOperations
    {
        private int verticeNeighbour;
        private int toNeighbourCost;
        private int verticePickedFromQueue;
        private List<double> listOfDistances = new List<double>();
        private Dictionary<double, double> listOfDistancesTemp = new Dictionary<double, double>();
        private Queue<int> allVerticesQueue = new Queue<int>();
                
        public string DijkstraAlgorithm(int sourceTop, int destinationTop, int allVertices, IEnumerable<IEnumerable<Vertex>> listOfNeighboursDistance1)
        {
            List<List<Vertex>> listOfNeighboursDistance = listOfNeighboursDistance1 as List<List<Vertex>>;
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
                    for (int i = 0; i < listOfNeighboursDistance[verticePickedFromQueue].Count; ++i)
                    {
                        verticeNeighbour = listOfNeighboursDistance[verticePickedFromQueue][i].DestinationCity;
                        toNeighbourCost = listOfNeighboursDistance[verticePickedFromQueue][i].Distance;

                        if (listOfDistancesTemp[verticeNeighbour] > listOfDistancesTemp[verticePickedFromQueue] + toNeighbourCost)
                        {
                            listOfDistancesTemp[verticeNeighbour] = listOfDistancesTemp[verticePickedFromQueue] + toNeighbourCost;
                            allVerticesQueue.Enqueue(verticeNeighbour);
                        }
                    }
                }

                allVerticesQueue.Dequeue();

            } while (allVerticesQueue.Count > 0);

            return "Shortest road from " + sourceTop + " to " + destinationTop + " is: " + listOfDistancesTemp[destinationTop].ToString() + " km";
        }
    }
}
