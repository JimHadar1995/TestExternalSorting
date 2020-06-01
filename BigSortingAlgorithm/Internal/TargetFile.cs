using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BigSortingAlgorithm.Internal
{
    internal sealed class TargetFile
    {
        public TargetFile(string targetFilePath)
        {
            TargetFilePath = targetFilePath;
        }

        public int RunsCount { get; set; }
        public string TargetFilePath { get; set; }

        public void MakeFinal()
        {
            const string outputFileName = "output.txt";
            if (File.Exists(outputFileName))
            {
                File.Delete(outputFileName);
            }

            File.Move(TargetFilePath, outputFileName);
        }
    }
}
