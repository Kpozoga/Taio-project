using System;
using System.Linq;

namespace Taio.Utils
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
            return CountEdges(graph1) + CountEdges(graph2) - 2 * CountEdges(GetSubgraph(graph1, graph2, equivalence)) + Math.Abs(graph1.GetLength(0) - graph2.GetLength(0));
        }
        public static int GetDistance(bool[,] graph1, bool[,] graph2)
        {
            return CountEdges(graph1) + CountEdges(graph2) - 2 * CountEdges(GetSubgraph(graph1, graph2)) + Math.Abs(graph1.GetLength(0) - graph2.GetLength(0));
        }
        public static bool[,] GetSubgraph(bool[,] g1, bool[,] g2, int[] eq)
        {
            int nMin, nMax;
            if (g1.GetLength(0) > g2.GetLength(0))
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
            for (int i = 0; i < nMin; i++)
                for (int j = 0; j < nMin; j++)
                    if (eq[i] != -1 && eq[j] != -1)
                        subgraph[i, j] = g1[i, j] && g2[eq[i], eq[j]];
            return subgraph;
        }
        public static bool[,] GetSubgraph(bool[,] g1, bool[,] g2)
        {
            int nMin, nMax;
            if (g1.GetLength(0) > g2.GetLength(0))
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
            for (int i = 0; i < nMin; i++)
                for (int j = 0; j < nMin; j++)
                    subgraph[i, j] = g1[i, j] && g2[i, j];
            return subgraph;
        }
        public static bool[,] ReadGraph(int n)
        {
            var graph = new bool[n, n];
            for (int i = 0; i < n; i++)
            {
                var line = Console.ReadLine();
                var splitted = line.Split(' ');
                for (int j = 0; j < n; j++)
                {
                    int.TryParse(splitted[j], out int num);
                    graph[i, j] = Convert.ToBoolean(num);
                }
            }
            return graph;
        }

        public static (bool[,] graph1, bool[,] graph2) ReadFromFile(string path)
        {
            string[] lines = System.IO.File.ReadAllLines(path);
            int n1 = int.Parse(lines[0]);
            var graph1 = new bool[n1, n1];
            for (int i = 0; i < n1; i++)
            {
                var line = lines[i+1];
                var splitted = line.Split(' ');
                for (int j = 0; j < n1; j++)
                {
                    int.TryParse(splitted[j], out int num);
                    graph1[i, j] = Convert.ToBoolean(num);
                }
            }
            int n2 = int.Parse(lines[n1 + 1]);
            var graph2 = new bool[n1, n1];
            for (int i = 0; i < n2; i++)
            {
                var line = lines[n1+i+2];
                var splitted = line.Split(' ');
                for (int j = 0; j < n2; j++)
                {
                    int.TryParse(splitted[j], out int num);
                    graph2[i, j] = Convert.ToBoolean(num);
                }
            }
            return (graph1, graph2);
        }

        static public void WriteGraph(bool[,] graph1, bool[,] graph2, int[] equivalence)
        {
            const int consoleWidth = 40;
            int n1 = graph1.GetLength(0);
            int n2 = graph2.GetLength(0);
            if (Math.Max(n1, n2) > consoleWidth) //not writing graph because it is too large
                return;
            (int i1, int i2)[] table = new (int,int)[Math.Max(n1,n2)];
            bool[] tmpGraph2 = new bool[n2];
            int p = 0, k = equivalence.Length - 1;
            for (int i = 0; i < equivalence.Length; i++)
            {
                if (equivalence[i] != -1)
                {
                    table[p] = (i, equivalence[i]);
                    tmpGraph2[equivalence[i]] = true;
                    p++;
                }
                else
                {
                    table[k] = (i, -1);
                    k--;
                }
            }

            if (n2 > n1)
            {
                p = n1;
                for (int i = 0; i < tmpGraph2.Length; i++)
                {
                    if (!tmpGraph2[i])
                    {
                        table[p] = (-1, i);
                        p++;
                    }
                }
            }
            if (n1 + n2 + 1 < consoleWidth)
            {
                //write two graphs next to each other
                Console.Write(n1);
                for (int i = (int)Math.Ceiling(Math.Log10(n1 + 1)); i < n1 + 1; i++)
                {
                    Console.Write(' ');
                }
                Console.WriteLine(n2);
                for (int i = 0; i < Math.Min(n1, n2); i++)
                {
                    for(int j=0;j<n1;j++)
                        Console.Write(graph1[table[i].i1, table[j].i1] ? 1 : 0);
                    Console.Write(" ");
                    for(int j=0;j<n2;j++)
                        Console.Write(graph2[table[i].i2, table[j].i2] ? 1 : 0);
                    Console.WriteLine();
                }
                for (int i = Math.Min(n1,n2); i < n1; i++)
                {
                    for(int j=0;j<n1;j++)
                        Console.Write(graph1[table[i].i1, table[j].i1] ? 1 : 0);
                    Console.WriteLine();
                }
                for (int i = Math.Min(n1,n2); i < n2; i++)
                {
                    for(int j=0;j<=n1;j++)
                        Console.Write(" ");
                    for(int j=0;j<n2;j++)
                        Console.Write(graph2[table[i].i2, table[j].i2] ? 1 : 0);
                    Console.WriteLine();
                }
            }
            else
            {
                //write second graph after first one
                Console.WriteLine(n1);
                for (int i = 0; i < n1; i++)
                {
                    for (int j = 0; j < n1; j++)
                        Console.Write(graph1[table[i].i1, table[j].i1] ? 1 : 0);
                    Console.WriteLine();
                }
                Console.WriteLine(n2);
                for (int i = 0; i < n2; i++)
                {
                    for (int j = 0; j < n2; j++)
                        Console.Write(graph2[table[i].i2, table[j].i2] ? 1 : 0);
                    Console.WriteLine();
                }
            }
            Console.WriteLine();
        }
    }
}
