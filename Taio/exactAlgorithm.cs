using System;
using System.Collections.Generic;
using System.Text;
using Taio.Utils;

namespace Taio
{
    public static class ExactAlgorithm
    {
        public static void CalculateExactAlgorithm(bool[,] graph1, bool[,] graph2)
        {
            Console.WriteLine("---Exact algorithm---");
            int exactDist = ExactAlgorithm.GetExactDistance(graph1, graph2);
            Console.WriteLine("Distance between the graphs above: {0}", exactDist);
            Console.WriteLine();
        }
        public static int GetExactDistance(bool[,] graph1, bool[,] graph2)
        {
            int dist = int.MaxValue;
            int n = Math.Min(graph1.GetLength(0), graph2.GetLength(0));
            int m = Math.Max(graph1.GetLength(0), graph2.GetLength(0));
            bool[,] graphBase = new bool[n, n];
            bool[,] graphPermuted = new bool[m, m];
            // wykonujemy głębokie kopie, zawsze będziemy permutować większą macierz
            if (graph1.GetLength(0) > graph2.GetLength(0))
            {
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                        graphBase[i, j] = graph2[i, j];
                for (int i = 0; i < m; i++)
                    for (int j = 0; j < m; j++)
                        graphPermuted[i, j] = graph1[i, j];
            }
            else
            {
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                        graphBase[i, j] = graph1[i, j];
                for (int i = 0; i < m; i++)
                    for (int j = 0; j < m; j++)
                        graphPermuted[i, j] = graph2[i, j];
            }
            bool[,] closestGraph = Permute(m, graphBase, ref graphPermuted, new bool[m,m], ref dist);

            
            Util.PrintGraphs(graphBase, closestGraph, "Base graph", "graph permuted");

            return (dist);
        }

        public static bool[,] Permute(int k, bool[,] graphBase, ref bool[,] graphPermuted, bool[,] closestGraph, ref int dist)
        {
            int n = graphPermuted.GetLength(0);
            //http://algorytmika.wikidot.com/exponential-permut stąd pochodzi algorytm permutacji
            if (k == 1) 
            {
                int tmpDist = Util.GetDistance(graphBase, graphPermuted);
                if (tmpDist < dist)
                {
                    dist = tmpDist;
                    for (int i = 0; i < n; i++)
                        for (int j = 0; j < n; j++)
                            closestGraph[i, j] = graphPermuted[i, j];    
                }
            }
            else
            {
                for (int i=0; i < k ; i++)
                {
                    Swap(ref graphPermuted, i, k - 1);
                    Permute(k - 1, graphBase, ref graphPermuted, closestGraph, ref dist);
                    Swap(ref graphPermuted, i, k - 1);
                }
            }
            return closestGraph;
        }

        public static void Swap(ref bool[,] graph, int i, int j) // zamiana i-tego i j-tego wierzchołka
        {
            int n = graph.GetLength(0);
            bool[,] graphCopy = new bool[n, n];
            for (int a = 0; a < n; a++)
                for (int b = 0; b < n; b++)
                    graphCopy[a, b] = graph[a, b];

            for (int k = 0; k < n; k++)
            {
                graph[i, k] = graphCopy[j, k];
                graph[j, k] = graphCopy[i, k];
                graph[k, i] = graphCopy[k, j];
                graph[k, j] = graphCopy[k, i];
            }
            bool tmp = graph[i, i];
            graph[i, i] = graph[j, i];
            graph[j, i] = tmp;
            tmp = graph[i, j];
            graph[i, j] = graph[j, j];
            graph[j, j] = tmp;
        }

        public static bool AskUserWhetherCalculateBigGraph(bool[,] graph1, bool[,] graph2)
        {
            if (Math.Max(graph1.GetLength(0), graph2.GetLength(0)) > 10)
            {
                ConsoleKey response;
                Console.WriteLine("Computations of the exact algorithm may take a long time due to the size of input graphs.");
                do
                {
                    Console.Write($"Do you want to continue it's computations? [y/n] ");
                    response = Console.ReadKey(false).Key;
                    if (response != ConsoleKey.Enter)
                    {
                        Console.WriteLine();
                    }
                } while (response != ConsoleKey.Y && response != ConsoleKey.N);

                return (response == ConsoleKey.Y);
            }
            return true;
        }
    }
}
