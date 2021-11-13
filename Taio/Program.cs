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
            return CountEdges(graph1) + CountEdges(graph2) - 2 * CountEdges(GetSubgraph(graph1, graph2)) + Math.Abs(graph1.GetLength(0) - graph2.GetLength(0));
        }
        public static bool[,] ReadGraph(int n)
        {
            var graph = new bool[n,n];
            for(int i=0;i<n;i++)
            {
                var line = Console.ReadLine();
                var splitted = line.Split(' ');
                for(int j=0;j<n;j++)
                {
                    int.TryParse(splitted[j], out int num);                   
                    graph[i, j] = Convert.ToBoolean(num);
                }
            }
            return graph;
        }
        public static bool[,] GetSubgraph(bool[,] g1, bool[,] g2)
        {
            int nMin, nMax;
            if(g1.GetLength(0) > g2.GetLength(0))
            {
                nMax = g1.GetLength(0);
                nMin = g2.GetLength(0);
            }
            else
            {
                nMax = g2.GetLength(0);
                nMin = g1.GetLength(0);
            }
            var subgraph = new bool[nMax, nMax];
            for(int i=0;i<nMin;i++)               
                for(int j=0;j<nMin;j++)
                    subgraph[i,j] = g1[i,j] && g2[i,j];
            return subgraph;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            int.TryParse(Console.ReadLine(), out int n1);
            var graph1 = Util.ReadGraph(n1);
            int.TryParse(Console.ReadLine(), out int n2);
            var graph2 = Util.ReadGraph(n2);
            var result = Approximation.GetDistance(graph1, graph1);
            Console.WriteLine(result);
        }
    }
}
