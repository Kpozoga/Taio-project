using System;
using Taio.Utils;

namespace Taio
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please, enter data: ");
            int.TryParse(Console.ReadLine(), out int n1);
            var graph1 = Util.ReadGraph(n1);
            int.TryParse(Console.ReadLine(), out int n2);
            var graph2 = Util.ReadGraph(n2);
            var result = Approximation.GetDistance(graph1, graph2);
            Console.WriteLine("Approximation: {0}", result.distance);
            int exactDist = ExactAlgorithm.GetExactDistance(graph1, graph2);
            Console.WriteLine("Exact algorithm: {0}", exactDist);
            Console.ReadKey();
        }
    }
}
