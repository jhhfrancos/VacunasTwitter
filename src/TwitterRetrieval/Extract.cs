using System;
using System.IO;
using System.Linq;

namespace TwitterRetrieval
{
    public static class Extract
    {

        public static string[] GetDocuments()
        {
            try
            {
                // Set a variable to Twitter4j path.
                string docPath =
                ".//twitter4j/data";

                var files = from file in Directory.EnumerateFiles(docPath, "*", SearchOption.AllDirectories)
                            from line in File.ReadLines(file)
                            select new
                            {
                                File = file,
                                Line = line
                            };
                string[] result = new string[files.Count()];
                int i = 0;
                foreach (var f in files)
                {
                    //Quitar el ID
                    string line = f.Line.Remove(0,20);
                    result[i++] = line;
                }
                return result;
            }
            catch (UnauthorizedAccessException uAEx)
            {
                Console.WriteLine(uAEx.Message);
            }
            catch (PathTooLongException pathEx)
            {
                Console.WriteLine(pathEx.Message);
            }
            return new string[]{ };
        }

        public static string[] GetDocumentsBase()
        {
            try
            {
                // Set a variable to Twitter4j path.
                string docPath =
                ".//twitter4j/listAll.txt";

                var files = from line in File.ReadLines(docPath)
                            select new
                            {
                                Line = line
                            };
                string[] result = new string[files.Count()];
                int i = 0;
                foreach (var f in files)
                {
                    //Quitar el ID
                    string line = f.Line.Remove(0, 20);
                    result[i++] = line;
                }
                return result;
            }
            catch (UnauthorizedAccessException uAEx)
            {
                Console.WriteLine(uAEx.Message);
            }
            catch (PathTooLongException pathEx)
            {
                Console.WriteLine(pathEx.Message);
            }
            return new string[] { };
        }
    }
}
