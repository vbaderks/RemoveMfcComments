using System.IO;
using System.Text;

namespace RemoveMfcComments
{
    internal static class Program
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
                   line.Equals("// Construction") ||
                   line.Equals("// Dialog Data") ||
                   line.Equals("// Overrides") ||
                   line.Equals("// Implementation") ||
                   line.Equals("// ClassWizard generated virtual function overrides") ||
                   line.Equals("// NOTE: the ClassWizard will add message map macros here") ||
                   line.Equals("// NOTE: the ClassWizard will add data members here");
        }
    }
}
