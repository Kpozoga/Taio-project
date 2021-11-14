using System;
using System.Collections.Generic;
using System.Text;
using Taio.Utils;

namespace Taio
{
    public static class ExactAlgorithm
    {
        public static int GetExactDistance(bool[,] graph1, bool[,] graph2)
        {
            int dist = 0;
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
            Permute(m, graphBase, ref graphPermuted, ref dist);
            return dist;
        }

        public static void Permute(int k, bool[,] graphBase, ref bool[,] graphPermuted, ref int dist)
        {
            //http://algorytmika.wikidot.com/exponential-permut stąd pochodzi algorytm permutacji
            if (k == 1)
            {
                int tmpDist = Util.GetDistance(graphBase, graphPermuted);
                if (tmpDist < dist)
                    dist = tmpDist;
            }
            else
            {
                for (int i=0; i < k ; i++)
                {
                    Swap(ref graphPermuted, i, k - 1);
                    Permute(k - 1, graphBase, ref graphPermuted, ref dist);
                    Swap(ref graphPermuted, i, k - 1);
                }
            }
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
                graph[i, k] = graphCopy[k, j];
                graph[k, i] = graphCopy[j, k];
            }
        }

    }
}
