using System;
using Taio.Utils;

namespace Taio
{
    class Program
    {
        static void Main(string[] args)
        {
            int.TryParse(Console.ReadLine(), out int n1);
            var graph1 = Util.ReadGraph(n1);
            int.TryParse(Console.ReadLine(), out int n2);
            var graph2 = Util.ReadGraph(n2);
            var result = Approximation.GetDistance(graph1, graph1);
            Console.WriteLine(result);      
        }
    }
}
