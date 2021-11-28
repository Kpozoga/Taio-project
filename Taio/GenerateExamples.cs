using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Taio
{
    public class GenerateExamples
    {
        private static Random random=new Random();
        public static bool[,] createRandomGraph(int n, double perc)
        {
            bool[,] graph = new bool[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (random.NextDouble() <= perc)
                        graph[i, j] = true;
                    else
                        graph[i, j] = false;
                }
            }
            return graph;
        }

        public static (bool[,] g1, bool[,] g2) generateRandomGraphs(int n, double perc)
        {
            return (createRandomGraph(n, perc), createRandomGraph(n, perc));
        }
        public static (bool[,] g1, bool[,] g2) generateRandomIsoGraphs(int n, double perc)
        {
            var g1 = createRandomGraph(n, perc);
            bool[,] g2 = new bool[n, n];
            int[] ind = new int[n];
            for (int i = 0; i < n; i++)
                ind[i] = i;
            ind = ind.OrderBy(x => random.Next()).ToArray();
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    g2[i, j] = g1[ind[i], ind[j]];
                }
            }

            return (g1, g2);
        }

        public static void saveToFile(string path, (bool[,] g1, bool[,] g2) graphs)
        {
            int n = graphs.g1.GetLength(0);
            int m = graphs.g2.GetLength(0);
            using (StreamWriter writer = new StreamWriter(path))  
            {
                writer.WriteLine(n);
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if(j>0)
                            writer.Write(' ');
                        if(graphs.g1[i,j])
                            writer.Write(1);
                        else
                            writer.Write(0);
                    }
                    writer.WriteLine();
                }
                writer.WriteLine(m);
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < m; j++)
                    {
                        if(j>0)
                            writer.Write(' ');
                        if(graphs.g2[i,j])
                            writer.Write(1);
                        else
                            writer.Write(0);
                    }
                    writer.WriteLine();
                } 
            }
        }
        static void Main2(string[] args)
        {
            //generate graphs
            for (int i = 1; i < 500; i++)
            {
                //dense graphs (50%)
                if (!File.Exists($"random-dense-{i}.txt"))
                    saveToFile($"random-dense-{i}.txt",generateRandomGraphs(i,0.5));
                //rare graphs (2%)
                if (!File.Exists($"random-rare-{i}.txt"))
                    saveToFile($"random-rare-{i}.txt",generateRandomGraphs(i,0.02));
                //isomorphic graphs
                if (!File.Exists($"random-iso-{i}.txt"))
                    saveToFile($"random-iso-{i}.txt",generateRandomIsoGraphs(i,0.5));
                Console.WriteLine(i);
            }
        }
    }
}