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

            return line.Contains("//{{AFX_DATA(") ||
                   line.Equals("//}}AFX_DATA") ||
                   line.Contains("//{{AFX_DATA_INIT") ||
                   line.Equals("//}}AFX_DATA_INIT") ||
                   line.Contains("//{{AFX_DATA_MAP") ||
                   line.Equals("//}}AFX_DATA_MAP") ||
                   line.Contains("//{{AFX_MSG") ||
                   line.Equals("//}}AFX_MSG") ||
                   line.Contains("//{{AFX_MSG_MAP") ||
                   line.Equals("//}}AFX_MSG_MAP") ||
                   line.Contains("//{{AFX_VIRTUAL") ||
                   line.Equals("//}}AFX_VIRTUAL") ||
                   line.Contains("//{{AFX_EVENTSINK_MAP") ||
                   line.Equals("//}}AFX_EVENTSINK_MAP") ||
                   line.Contains("//{{AFX_DISPATCH") ||
                   line.Equals("//}}AFX_DISPATCH") ||
                   line.Contains("//{{AFX_DISPATCH_MAP") ||
                   line.Equals("//}}AFX_DISPATCH_MAP") ||
                   line.Contains("//{{AFX_FIELD_MAP") ||
                   line.Equals("//}}AFX_FIELD_MAP") ||
                   line.Contains("//{{AFX_FIELD_INIT") ||
                   line.Equals("//}}AFX_FIELD_INIT") ||
                   line.Contains("//{{AFX_EVENT_MAP") ||
                   line.Equals("//}}AFX_EVENT_MAP") ||
                   line.Equals("// Construction") ||
                   line.Equals("// Dialog Data") ||
                   line.Equals("// Overrides") ||
                   line.Equals("// Implementation") ||
                   line.Equals("// ClassWizard generated virtual function overrides") ||
                   line.Equals("// NOTE: the ClassWizard will add message map macros here") ||
                   line.Equals("// NOTE: the ClassWizard will add data members here") ||
                   line.Equals("// NOTE: the ClassWizard will add member initialization here") ||
                   line.Equals("// NOTE - ClassWizard will add and remove event map entries") ||
                   line.Equals("// Generated message map functions") ||
                   line.Equals("// NOTE - the ClassWizard will add and remove member functions here.") ||
                   line.Equals("//    DO NOT EDIT what you see in these blocks of generated code !") ||
                   line.Equals("// NOTE - the ClassWizard will add and remove mapping macros here.") ||
                   line.Equals("//    DO NOT EDIT what you see in these blocks of generated code!") ||
                   line.Equals("//{{AFX_INSERT_LOCATION}}") ||
                   line.Equals(
                       "// Microsoft Developer Studio will insert additional declarations immediately before the previous line.") ||
                   line.Equals("// Microsoft Visual C++ will insert additional declarations immediately before the previous line.") ||
                   line.Equals("// Generated message map functions") ||
                   line.Equals("// NOTE: the ClassWizard will add member functions here") ||
                   line.Equals("// ClassWizard generate virtual function overrides") ||
                   line.Equals("// NOTE: the ClassWizard will add DDX and DDV calls here");
        }
    }
}
