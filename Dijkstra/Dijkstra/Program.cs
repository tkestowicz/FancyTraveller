using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dijkstra
{
    class Program
    {
        static void Main(string[] args)
        {
            int numberOfVertices;
            int numberOfEdges;
            int sourceVertice;
            int destinationVertice;
            int costBetweenVertices;

            List<List<Tuple<int, int>>> listOfNeighboursDistance = new List<List<Tuple<int, int>>>();
            
            Console.WriteLine("Vertices:\n");
            string line = Console.ReadLine();
            numberOfVertices = int.Parse(line);

            Console.WriteLine("Edges:\n");
            line = Console.ReadLine();
            numberOfEdges = int.Parse(line);

            listOfNeighboursDistance.Add(new List<Tuple<int, int>>());
            listOfNeighboursDistance[0].Add(new Tuple<int, int>(0, 0));

            for (int i = 0; i < numberOfEdges ; i++)
            {
                Console.WriteLine("Source:\n");
                line = Console.ReadLine();
                sourceVertice = int.Parse(line);
                Console.WriteLine("Destination:\n");
                line = Console.ReadLine();
                destinationVertice = int.Parse(line);
                Console.WriteLine("Cost:\n");
                line = Console.ReadLine();
                costBetweenVertices = int.Parse(line);

                listOfNeighboursDistance.Add(new List<Tuple<int, int>>());
                listOfNeighboursDistance[sourceVertice].Add(new Tuple<int, int>(destinationVertice, costBetweenVertices));
            }

            Console.WriteLine("From:\n");
            line = Console.ReadLine();
            sourceVertice = int.Parse(line);

            Console.WriteLine("To:\n");
            line = Console.ReadLine();
            destinationVertice = int.Parse(line); 

            DijkstraOperations dop = new DijkstraOperations();
            
            string shortestRoad = dop.DijkstraAlgorithm(sourceVertice, numberOfVertices + 1, listOfNeighboursDistance, destinationVertice);

            Console.WriteLine(shortestRoad);
            Console.ReadKey();
        }
    }
}
