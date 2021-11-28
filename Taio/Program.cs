using System;
using System.IO;
using System.Timers;
using Taio.Utils;

namespace Taio
{
    class Program
    {
        //only for tests
        public static StreamWriter sw;
        //end
        static void Main(string[] args)
        {
            //only for tests
            FileStream aFile = new FileStream("tests.txt", FileMode.Append, FileAccess.Write);
            sw = new StreamWriter(aFile);
            //end
            bool[,] graph1 = null, graph2 = null;
            bool pr = true;
            bool computeExact = true;
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
            if (args != null && args.Length > 1)
                if (args[1] == "approx-only")
                    computeExact = false;

            bool pr2 = true;
            int n1 = 0;
            if (pr)
            {
                Console.WriteLine("Please, enter path to the file: ");
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

            Console.WriteLine();
            Console.WriteLine("---Graphs extracted from the file---");
            Util.PrintGraphs(graph1, graph2, "graph 1", "graph 2");
            Console.WriteLine();
            
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            Approximation.CalculateApproximation(graph1, graph2);
            watch.Stop();
            Console.WriteLine($"Approximation algorithm execution time: {watch.ElapsedMilliseconds} ms");
            //only for tests
            sw.WriteLine($" {watch.ElapsedMilliseconds}");
            //end
            if (computeExact)
                if (ExactAlgorithm.AskUserWhetherCalculateBigGraph(graph1, graph2))
                {
                    watch.Restart();
                    ExactAlgorithm.CalculateExactAlgorithm(graph1, graph2);
                    watch.Stop();
                    Console.WriteLine($"Exact algorithm execution time: {watch.ElapsedMilliseconds} ms");
                }

            Console.WriteLine("Please, press any key to continue...");
            //only for tests
            sw.Close();
            aFile.Close();
            return;
            //end
            try
            {
                Console.ReadKey();
            }
            catch (Exception) { }
        }
    }
}
