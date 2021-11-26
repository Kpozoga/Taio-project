using System;
using Taio.Utils;

namespace Taio
{
    class Program
    {
        static void Main(string[] args)
        {
            bool[,] graph1 = null, graph2 = null;
            bool pr = true;
            if (args != null && args.Length > 0)
            {
                pr = false;
                try
                {
                    (graph1, graph2) = Util.ReadFromFile(args[0]);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception in reading file " + args[0] + ". Get data from console");
                    pr = true;
                }
                if (!pr)
                    Console.WriteLine(args[0] + " File loaded successfully");
            }
            else
            {
                Console.WriteLine("No file provided in args. Get data from console.");
            }
            bool pr2 = true;
            int n1 = 0;
            if (pr)
            {
                Console.WriteLine("Please, enter data: ");
                string line = Console.ReadLine();
                if (!int.TryParse(line, out n1))
                {
                    pr2 = false;
                    try
                    {
                        (graph1, graph2) = Util.ReadFromFile(line);

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception in reading file " + line + ". Please enter a matrix:");
                        pr2 = true;
                    }
                    if (!pr2)
                        Console.WriteLine(line + " File loaded successfully");
                }

            }
            if (pr && pr2)
            {

                graph1 = Util.ReadGraph(n1);
                int.TryParse(Console.ReadLine(), out int n2);
                graph2 = Util.ReadGraph(n2);
            }

            Util.PrintGraphs(graph1, graph2, "graph 1", "graph 2");

            var result = Approximation.GetDistance(graph1, graph2);
            Console.WriteLine("Approximation: {0}", result.distance);
            Util.WriteGraph(graph1, graph2, result.nodes);

            ExactAlgorithm.calculateExactAlgorithm(graph1, graph2);

            Console.WriteLine("Please, press any key to continue...");
            try
            {
                Console.ReadKey();
            }
            catch (Exception) { }
        }
    }
}
