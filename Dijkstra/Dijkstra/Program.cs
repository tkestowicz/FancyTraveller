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

            //List<List<Tuple<int, int>>> listOfNeighboursDistance = new List<List<Tuple<int, int>>>();
            List<List<Vertex>> listOfNeighboursDistance = new List<List<Vertex>>();

            Console.WriteLine("Vertices:\n");
            string line = Console.ReadLine();
            numberOfVertices = int.Parse(line);

            Console.WriteLine("Edges:\n");
            line = Console.ReadLine();
            numberOfEdges = int.Parse(line);

            listOfNeighboursDistance.Add(new List<Vertex>());
            Vertex zeroVertex = new Vertex() { SourceCity = "Washington", DestinationCity = 0, Distance = 0 };
            listOfNeighboursDistance[0].Add(zeroVertex);

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

                listOfNeighboursDistance.Add(new List<Vertex>());
                Vertex nextVertex = new Vertex() { SourceCity = "aaa", DestinationCity = destinationVertice, Distance = costBetweenVertices};
                listOfNeighboursDistance[sourceVertice].Add(nextVertex);
            }

            Console.WriteLine("From:\n");
            line = Console.ReadLine();
            sourceVertice = int.Parse(line);

            Console.WriteLine("To:\n");
            line = Console.ReadLine();
            destinationVertice = int.Parse(line); 

            DijkstraOperations dop = new DijkstraOperations();

            string shortestRoad = dop.DijkstraAlgorithm(sourceVertice, destinationVertice, numberOfVertices + 1, listOfNeighboursDistance);

            Console.WriteLine(shortestRoad);
            Console.ReadKey();
        }
    }
}
