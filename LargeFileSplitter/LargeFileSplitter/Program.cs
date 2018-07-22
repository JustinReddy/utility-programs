using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Linq.Expressions;

namespace LargeFileSplitter
{
    /// <summary>
    /// http://caioproiete.net/en/split-text-file-into-several-files-using-csharp/
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Large File Splitter version 1.0");
                Console.WriteLine("Author: Justin Reddy");
                Console.WriteLine("Release Date: 27 Jan 2017");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;

                String sourceFileName = String.Empty;
                int linesPerFile = 400000;
                if (args != null && args.Length == 2)
                {
                    Console.WriteLine("Source File = {0}", args[0]);
                    sourceFileName = args[0];
                    Console.WriteLine("Lines per File = {0}", args[1]);
                    sourceFileName = args[1];
                }
                else
                {
                    Console.Write("Enter path to source file:");
                    sourceFileName = Console.ReadLine();
                    Console.Write("Enter Lines per File:");
                    int.TryParse(Console.ReadLine(), out linesPerFile);
                }

                if (!File.Exists(sourceFileName))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("File does NOT exist - {0}", sourceFileName);
                }
                else
                {
                    string destinationFileName = Path.GetDirectoryName(sourceFileName) +  @"\" + Path.GetFileNameWithoutExtension(sourceFileName) +  @"-Part-{0}" + Path.GetExtension(sourceFileName);
                    Console.WriteLine("Destinationation File Name Format = {0} ", destinationFileName);
                    Console.WriteLine("Starting to Split File...");
                    using (var sourceFile = new StreamReader(sourceFileName))
                    {
                        var fileCounter = 0;
                        var destinationFile = new StreamWriter(string.Format(destinationFileName, fileCounter + 1));

                        try
                        {
                            var lineCounter = 0;

                            string line;
                            while ((line = sourceFile.ReadLine()) != null)
                            {
                                // Did we reach the maximum number of lines for the file?
                                if (lineCounter >= linesPerFile)
                                {
                                    // Yep... Time to close this one and 
                                    // switch to another file
                                    lineCounter = 0;
                                    fileCounter++;

                                    destinationFile.Dispose();
                                    destinationFile = new StreamWriter(string.Format(destinationFileName, fileCounter + 1));
                                    Console.WriteLine("Writing File: " + string.Format(destinationFileName, fileCounter + 1));
                                }

                                destinationFile.WriteLine(line);
                                lineCounter++;
                            }
                        }
                        finally
                        {
                            Console.WriteLine("Split File done...Cleaning up resources.");
                            destinationFile.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("General Error Occured");
                Console.WriteLine(ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine(ex.InnerException.Message);
                }
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Hit Enter to exit..");
            Console.ReadKey();
        }

    }
}
