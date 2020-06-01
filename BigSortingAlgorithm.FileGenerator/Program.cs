using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

namespace BigSortingAlgorithm.FileGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var sw = new Stopwatch();
            sw.Start();
            string fileName = args.Length >= 1 ? args[0] : "input.txt";
            long fileSize = args.Length >= 2 ? long.Parse(args[1]) : 10000;
            new BigSortingFileGenerator(fileName, fileSize).Generate();
            sw.Stop();
            Console.WriteLine($"Затраченное время: {sw.Elapsed.TotalSeconds} секунд");
        }
    }
}
