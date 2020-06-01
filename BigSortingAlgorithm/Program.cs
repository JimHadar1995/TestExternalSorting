using System;
using System.Diagnostics;

namespace BigSortingAlgorithm
{
    class Program
    {
        static void Main(string[] args)
        {   
            try
            {
                string filePath = args.Length > 0 ? args[0] : "input.txt";
                var watch = new Stopwatch();
                watch.Start();
                new SortAlgorithm(filePath).Run();
                watch.Stop();
                Console.WriteLine($"Файл отсортирован за {watch.Elapsed.TotalSeconds} секунд");
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }
            Console.ReadKey();
        }
    }
}
