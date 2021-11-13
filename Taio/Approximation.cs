using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net.Http.Headers;

namespace Taio
{
    struct Vertex
    {
        public int index;
        public int inDeg;
        public int outDeg;

        public Vertex(int index, int inDeg, int outDeg)
        {
            this.index = index;
            this.inDeg = inDeg;
            this.outDeg = outDeg;
        }
        public Vertex(int index)
        {
            this.index = index;
            this.inDeg = 0;
            this.outDeg = 0;
        }
        public int GetValue()
        {
            return this.outDeg - this.inDeg;
        }
    }
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

        private static int findMaxIndex(List<Vertex> vertices)
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
            int max1 = findMaxIndex(degrees1);
            int max2 = findMaxIndex(degrees2);
            int[] equivalence = Enumerable.Repeat(-1, n1).ToArray();
            equivalence[degrees1[max1].index] = degrees2[max2].index;
            degrees1.RemoveAt(max1);
            degrees2.RemoveAt(max2);
            while(degrees1.Count>0 && degrees2.Count>0)
            {
                max1 = findMaxIndex(degrees1);
                max2 = findMaxIndex(degrees2);
                equivalence[degrees1[max1].index] = degrees2[max2].index;
                degrees1.RemoveAt(max1);
                degrees2.RemoveAt(max2);
            }
            return (Util.GetDistance(graph1,graph2,equivalence), equivalence);
        }
    }
}