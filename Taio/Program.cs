using System;
using Taio.Utils;

namespace Taio
{
    class Program
    {
        static void Main(string[] args)
        {
            bool[,] graph1=null, graph2=null;
            bool pr = true;
            if (args!=null && args.Length > 0)
            {
                pr = false;
                try
                {
                    (graph1, graph2) = Util.ReadFromFile(args[0]);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception in reading file "+args[0]+". Get data from console");
                    pr = true;
                }
                if(!pr)
                    Console.WriteLine(args[0]+"File loaded successfully");
            }
            else
            {
                Console.WriteLine("No file provided in args. Get data from console.");
            }
            if(pr)
            {
                Console.WriteLine("Please, enter data: ");
                int.TryParse(Console.ReadLine(), out int n1);
                graph1 = Util.ReadGraph(n1);
                int.TryParse(Console.ReadLine(), out int n2);
                graph2 = Util.ReadGraph(n2);
            }
            var result = Approximation.GetDistance(graph1, graph2);
            Console.WriteLine("Approximation: {0}", result.distance);
            Util.WriteGraph(graph1, graph2, result.nodes);
            int exactDist = ExactAlgorithm.GetExactDistance(graph1, graph2);
            Console.WriteLine("Exact algorithm: {0}", exactDist);
        }
    }
}
