using System;

namespace Taio
{
    class Util
    {
        private static int CountEdges(bool[,] graph)
        {
            int ret = 0;
            for (int i = 0; i < graph.GetLength(0); i++)
            {
                for (int j = 0; j < graph.GetLength(1); j++)
                {
                    if (graph[i, j])
                        ret++;
                }
            }
            return ret;
        }
        public static int GetDistance(bool[,] graph1, bool[,] graph2, int[] equivalence)
        {
            return CountEdges(graph1) + CountEdges(graph2) - Math.Abs(graph1.GetLength(0) - graph2.GetLength(0));
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var graph1 = new bool[3, 3];
            graph1[1, 0] = true;
            graph1[1, 2] = true;
            var result = Approximation.GetDistance(graph1, graph1);
        }
    }
}
