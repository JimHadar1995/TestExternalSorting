using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;

namespace BigSortingAlgorithm.FileGenerator
{
    public sealed class BigSortingFileGenerator
    {
        private readonly string[] _strArray = new[]
            {
                "Something something something",
                "Cherry is the best",
                "Banana is yellow",
                "Apple",
                "Blueberry is better than the best. It's a fact. Nobody can argue with that",
                "Strawberry is very delicious",
                "Pineapple is sweet and tasty",
                "What you see is what you get"
            };
        public string FileName { get;}
        public long FileSize { get; }
        public BigSortingFileGenerator(string fileName, long fileSize)
        {
            FileName = fileName;
            FileSize = fileSize;
        }

        public void Generate()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;;

            var randomInt = new Random();

            long totalBytes = 0;
            using (var writer = new StreamWriter(new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.None, 8192)))
            {
                while (totalBytes < FileSize)
                {
                    var str = _strArray[randomInt.Next(_strArray.Length)];
                    var number = randomInt.Next(1000000);
                    var fileStr = $"{number}. {str}";
                    var newLine = Environment.NewLine;
                    writer.Write($"{number}. {str}{newLine}");
                    totalBytes += fileStr.Length + newLine.Length;
                }
            }
        }
    }
}
