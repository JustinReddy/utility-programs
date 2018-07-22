using System;
using System.IO;

namespace FileMerger
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Files Merger version 1.0");
                Console.WriteLine("Author: Justin Reddy");
                Console.WriteLine("Release Date: 30 Jan 2017");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;

                String sourceFolderName = String.Empty;
                String targetFile = String.Empty;
                if (args != null && args.Length == 2)
                {
                    Console.WriteLine("Source Folder = {0}", args[0]);
                    sourceFolderName = args[0];
                    Console.WriteLine("Target File = {0}", args[1]);
                    targetFile = args[1];
                }
                else
                {
                    Console.Write("Enter path to source folder:");
                    sourceFolderName = Console.ReadLine();
                    Console.Write("Enter Target filename:");
                    targetFile = Console.ReadLine();
                }

                if (!Directory.Exists(sourceFolderName))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Folder does NOT exist - {0}", sourceFolderName);
                }
                else
                {
                    var files = Directory.GetFiles(sourceFolderName);
                    targetFile = Path.GetFullPath(sourceFolderName) + (sourceFolderName.EndsWith("\\")? null : "\\") + Path.GetFileName(targetFile);
                    Console.WriteLine("Found {0} files to merge", files.GetLength(0));
                    foreach (var file in files)
                    {
                        Console.WriteLine(file);
                    }


                    Console.WriteLine("Starting to Merge Files...");

                    foreach (var file in files)
                    {
                        using (var sourceFile = new StreamReader(file))
                        {
                            var destinationFile = new StreamWriter(targetFile, true);
                            try
                            {
                                string line;
                                while ((line = sourceFile.ReadLine()) != null)
                                {
                                    destinationFile.WriteLine(line);
                                }
                            }
                            finally
                            {
                                Console.WriteLine("Merging File done...Cleaning up resources.");
                                destinationFile.Dispose();
                            }
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
