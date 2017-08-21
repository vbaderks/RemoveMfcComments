using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoveMfcComments
{
    static class Program
    {
        private static void Main(string[] args)
        {
            ReadAndClean("input.h", "output.h");
        }

        private static void ReadAndClean(string inputPath, string outputPath)
        {
            using (var inputFile = new StreamReader(inputPath, Encoding.GetEncoding("Windows-1252"), true))
            using (var outputFile = new StreamWriter(outputPath, false, Encoding.UTF8))
            {
                string line;
                while ((line = inputFile.ReadLine()) != null)
                {
                    if (IsLegacyMfcClassWizardComment(line))
                        continue;
                    outputFile.WriteLine(line);
                }
            }
        }

        private static bool IsLegacyMfcClassWizardComment(string line)
        {
            return line.Contains("//{{AFX_DATA_INIT") ||
                   line.Contains("//}}AFX_DATA_INIT");
        }
    }
}
