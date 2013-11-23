﻿using System;
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

        public int FindShortestRoute(int sourceTop, int destinationTop, int allVertices, List<List<Vertex>> vertices)
        {

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
                    for (int i = 0; i < vertices[verticePickedFromQueue].Count; ++i)
                    {

                        verticeNeighbour = vertices[verticePickedFromQueue][i].DestinationCity.CityId;
                        toNeighbourCost = vertices[verticePickedFromQueue][i].Distance;

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