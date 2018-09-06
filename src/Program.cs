// Copyright (C) Victor Derks. See LICENSE.md for the details of the software license.

using System;
using System.IO;
using System.Text;

namespace RemoveMfcComments
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: RemoveMfcComments <directory> <pattern>");
                return;
            }

            try
            {
                const int DirectoryIndex = 0;
                const int PatternIndex = 1;

                var filenames = Directory.GetFiles(args[DirectoryIndex], args[PatternIndex], SearchOption.AllDirectories);
                Console.WriteLine("Found {0} files with pattern {1} in directory {2} ", filenames.Length, args[PatternIndex], args[DirectoryIndex]);
                var updateCount = 0;

                foreach (string inputFilename in filenames)
                {
                    string outputFilename = inputFilename + ".stripped";
                    bool linesStripped = ReadAndClean(inputFilename, outputFilename);
                    if (linesStripped)
                    {
                        File.Delete(inputFilename);
                        File.Move(outputFilename, inputFilename);
                        updateCount++;
                    }
                    else
                    {
                        File.Delete(outputFilename);
                    }
                }

                Console.WriteLine("Processed {0} files", updateCount);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static bool ReadAndClean(string inputPath, string outputPath)
        {
            var lineStripped = false;

            using (var inputFile = new StreamReader(inputPath, Encoding.GetEncoding("Windows-1252"), true))
            using (var outputFile = new StreamWriter(outputPath, false, Encoding.UTF8))
            {
                string line;
                while ((line = inputFile.ReadLine()) != null)
                {
                    if (IsLegacyMfcClassWizardComment(line))
                    {
                        lineStripped = true;
                        continue;
                    }

                    outputFile.WriteLine(line);
                }
            }

            return lineStripped;
        }

        private static bool IsLegacyMfcClassWizardComment(string line)
        {
            line = line.Trim();

            return line.Contains("//{{AFX_DATA(", StringComparison.InvariantCulture) ||
                   line.Equals("//}}AFX_DATA", StringComparison.InvariantCulture) ||
                   line.Contains("//{{AFX_DATA_INIT", StringComparison.InvariantCulture) ||
                   line.Equals("//}}AFX_DATA_INIT", StringComparison.InvariantCulture) ||
                   line.Contains("//{{AFX_DATA_MAP", StringComparison.InvariantCulture) ||
                   line.Equals("//}}AFX_DATA_MAP", StringComparison.InvariantCulture) ||
                   line.Contains("//{{AFX_MSG", StringComparison.InvariantCulture) ||
                   line.Equals("//}}AFX_MSG", StringComparison.InvariantCulture) ||
                   line.Contains("//{{AFX_MSG_MAP", StringComparison.InvariantCulture) ||
                   line.Equals("//}}AFX_MSG_MAP", StringComparison.InvariantCulture) ||
                   line.Contains("//{{AFX_VIRTUAL", StringComparison.InvariantCulture) ||
                   line.Equals("//}}AFX_VIRTUAL", StringComparison.InvariantCulture) ||
                   line.Contains("//{{AFX_EVENTSINK_MAP", StringComparison.InvariantCulture) ||
                   line.Equals("//}}AFX_EVENTSINK_MAP", StringComparison.InvariantCulture) ||
                   line.Contains("//{{AFX_DISPATCH", StringComparison.InvariantCulture) ||
                   line.Equals("//}}AFX_DISPATCH", StringComparison.InvariantCulture) ||
                   line.Contains("//{{AFX_DISPATCH_MAP", StringComparison.InvariantCulture) ||
                   line.Equals("//}}AFX_DISPATCH_MAP", StringComparison.InvariantCulture) ||
                   line.Contains("//{{AFX_FIELD_MAP", StringComparison.InvariantCulture) ||
                   line.Equals("//}}AFX_FIELD_MAP", StringComparison.InvariantCulture) ||
                   line.Contains("//{{AFX_FIELD_INIT", StringComparison.InvariantCulture) ||
                   line.Equals("//}}AFX_FIELD_INIT", StringComparison.InvariantCulture) ||
                   line.Contains("//{{AFX_EVENT_MAP", StringComparison.InvariantCulture) ||
                   line.Equals("//}}AFX_EVENT_MAP", StringComparison.InvariantCulture) ||
                   line.Equals("// Construction", StringComparison.InvariantCulture) ||
                   line.Equals("// Dialog Data", StringComparison.InvariantCulture) ||
                   line.Equals("// Overrides", StringComparison.InvariantCulture) ||
                   line.Equals("// Implementation", StringComparison.InvariantCulture) ||
                   line.Equals("// ClassWizard generated virtual function overrides", StringComparison.InvariantCulture) ||
                   line.Equals("// NOTE: the ClassWizard will add message map macros here", StringComparison.InvariantCulture) ||
                   line.Equals("// NOTE: the ClassWizard will add data members here", StringComparison.InvariantCulture) ||
                   line.Equals("// NOTE: the ClassWizard will add member initialization here", StringComparison.InvariantCulture) ||
                   line.Equals("// NOTE - ClassWizard will add and remove event map entries", StringComparison.InvariantCulture) ||
                   line.Equals("// Generated message map functions", StringComparison.InvariantCulture) ||
                   line.Equals("// NOTE - the ClassWizard will add and remove member functions here.", StringComparison.InvariantCulture) ||
                   line.Equals("//    DO NOT EDIT what you see in these blocks of generated code !", StringComparison.InvariantCulture) ||
                   line.Equals("// NOTE - the ClassWizard will add and remove mapping macros here.", StringComparison.InvariantCulture) ||
                   line.Equals("//    DO NOT EDIT what you see in these blocks of generated code!", StringComparison.InvariantCulture) ||
                   line.Equals("//{{AFX_INSERT_LOCATION}}", StringComparison.InvariantCulture) ||
                   line.Equals(
                       "// Microsoft Developer Studio will insert additional declarations immediately before the previous line.", StringComparison.InvariantCulture) ||
                   line.Equals("// Microsoft Visual C++ will insert additional declarations immediately before the previous line.", StringComparison.InvariantCulture) ||
                   line.Equals("// Generated message map functions", StringComparison.InvariantCulture) ||
                   line.Equals("// NOTE: the ClassWizard will add member functions here", StringComparison.InvariantCulture) ||
                   line.Equals("// ClassWizard generate virtual function overrides", StringComparison.InvariantCulture) ||
                   line.Equals("// NOTE: the ClassWizard will add DDX and DDV calls here", StringComparison.InvariantCulture);
        }
    }
}
