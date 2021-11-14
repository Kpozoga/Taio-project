using System;
using Taio.Utils;

namespace Taio
{
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
