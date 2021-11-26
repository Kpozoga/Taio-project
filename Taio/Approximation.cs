using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http.Headers;
using Taio.Utils;

namespace Taio
{
    public static class Approximation
    {
        private static Vertex[] CalculateDegrees(bool[,] graph)
        {
            int n = graph.GetLength(0);
            var degrees = new Vertex[n];
            for (int i = 0; i < n; ++i)
            {
                degrees[i] = new Vertex(i);
                for (int j = 0; j < n; j++)
                {
                    //if (j == i) continue;
                    if (graph[i, j])
                        degrees[i].outDeg++;
                    if (graph[j, i])
                        degrees[i].inDeg++;
                }
            }
            return degrees;
        }
        private static Vertex CalculateInternalDegrees(bool[,] graph, int[] inVertices, int index)
        {
            int n = graph.GetLength(0);
            Vertex degree = new Vertex(index);
            foreach (int i in inVertices)
            {
                if (graph[i, index])
                    degree.inDeg++;
                if (graph[index, i])
                    degree.outDeg++;
            }
            return degree;
        }

        private static int[] GetExternal(bool[,] graph, List<int> inVertices)
        {
            bool[] tmpTable = new bool [graph.GetLength(0)];
            for (int i = 0; i < tmpTable.Length; i++)
                tmpTable[i] = false;
            foreach (int i in inVertices)
            {
                tmpTable[i] = true;
            }
            List<int> ret = new List<int>();
            for (int i = 0; i < tmpTable.Length; i++)
            {
                if(!tmpTable[i])
                    ret.Add(i);
            }
            return ret.ToArray();
        }

        private static Vertex[] GetNeighbours(bool[,] graph, List<int> inVertices)
        {
            var tmpTable = new Vertex[graph.GetLength(0)];
            var outVertices = GetExternal(graph, inVertices);
            for (int i = 0; i < tmpTable.Length; i++)
                tmpTable[i] = new Vertex(i);
            foreach (int i in inVertices)
            {
                foreach (var j in outVertices)
                {
                    if(graph[i, j])
                        tmpTable[j].inDeg++;
                    if(graph[j, i])
                        tmpTable[j].outDeg++;
                }
            }
            List<Vertex> ret = new List<Vertex>();
            foreach(var v in tmpTable)
            {
                if(v.inDeg+v.outDeg>0)
                    ret.Add(v);
            }
            return ret.ToArray();
        }

        private static int FindMaxIndex(List<Vertex> vertices)
        {
            int ret = 0;
            for (int i = 1; i < vertices.Count; i++)
            {
                if (vertices[i].GetValue() > vertices[ret].GetValue() ||
                    (vertices[i].GetValue() == vertices[ret].GetValue() && vertices[i].inDeg + vertices[i].outDeg >
                        vertices[ret].inDeg + vertices[ret].outDeg))
                    ret = i;
            }
            return ret;
        }

        public static (int distance, int[] nodes) GetDistance(bool[,] graph1, bool[,] graph2)
        {
            int n1 = graph1.GetLength(0);
            int n2 = graph2.GetLength(0);
            //Phase 1: calculating vertex degree
            var degrees1 = new List<Vertex>(CalculateDegrees(graph1));
            var degrees2 = new List<Vertex>(CalculateDegrees(graph2));
            //Phase 2: sort arrays by total degree
            int[] equivalence = Enumerable.Repeat(-1, n1).ToArray();
            List<int> inVertices1 = new List<int>(); //from graph 1
            List<int> inVertices2 = new List<int>(); //from graph 2
            //set max
            /*int max = SetMax(ref equivalence, ref degrees1, ref degrees2);
            inVertices1.Add(max);
            inVertices2.Add(equivalence[max]);*/
            while(degrees1.Count>0 && degrees2.Count>0)
            {
                //Console.WriteLine("{0} {1}",degrees1.Count, degrees2.Count);
                Vertex[] tmp1 = GetNeighbours(graph1, inVertices1);
                Vertex[] tmp2 = GetNeighbours(graph2, inVertices2);
                int best1, best2;
                if (tmp1.Length == 0 || tmp2.Length == 0)
                {
                    //get max indexes
                    best1 = degrees1[FindMaxIndex(degrees1)].index;
                    best2 = degrees1[FindMaxIndex(degrees2)].index;
                }
                else
                {
                    //get most similar neighbours
                    Vertex mb1 = tmp1[0];
                    Vertex mb2 = tmp2[0];
                    foreach (Vertex v1 in tmp1)
                    {
                        foreach (Vertex v2 in tmp2)
                        {
                            int dist = (int)Math.Pow(v1.inDeg - v2.inDeg, 2) + (int)Math.Pow(v1.outDeg - v2.outDeg, 2);
                            int bestDist = (int)Math.Pow(mb1.inDeg - mb2.inDeg, 2) + (int)Math.Pow(mb1.outDeg - mb2.outDeg, 2);
                            if (dist < bestDist || (dist==bestDist && v1.inDeg+v1.outDeg>=v2.inDeg+v2.outDeg))
                            {
                                mb1 = v1;
                                mb2 = v2;
                            }
                        }
                    }
                    best1 = mb1.index;
                    best2 = mb2.index;
                }
                equivalence[best1] = best2;
                degrees1.RemoveAll(x => x.index==best1);
                degrees2.RemoveAll(x => x.index==best2);
                inVertices1.Add(best1);
                inVertices2.Add(best2);
            }
            return (Util.GetDistance(graph1,graph2,equivalence), equivalence);
        }

        public static void CalculateApproximation(bool[,] graph1, bool[,] graph2)
        {
            Console.WriteLine("---Approximation algorithm---");
            var result = Approximation.GetDistance(graph1, graph2);
            Util.WriteGraph(graph1, graph2, "graph1", "graph2", result.nodes);
            Console.WriteLine("Distance between the graphs above: {0}", result.distance);
            Console.WriteLine();
            Console.WriteLine();
        }
    }   
}