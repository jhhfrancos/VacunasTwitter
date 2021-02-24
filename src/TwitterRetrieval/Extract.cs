using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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

        public static bool ExecuteBash(this string cmd) //, ILogger logger
        {
            //var source = new TaskCompletionSource<int>();
            var escapedArgs = cmd.Replace("\"", "\\\"");
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "Bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = false,
                    RedirectStandardError = false,
                    UseShellExecute = true,
                    CreateNoWindow = true                    
                },
                EnableRaisingEvents = true
            };
            
            process.Exited += (sender, args) =>
            {
                //var outline = process.StandardOutput.ReadToEnd();
                //var error = process.StandardError.ReadToEnd();
                //Console.WriteLine(error);
                //Console.WriteLine(args);
                //logger.LogWarning(process.StandardError.ReadToEnd());
                //logger.LogInformation(process.StandardOutput.ReadToEnd());
                //if (process.ExitCode == 0)
                //{
                //    source.SetResult(0);
                //}
                //else
                //{
                //    source.SetException(new Exception($"Command `{cmd}` failed with exit code `{process.ExitCode}`"));
                //}
                foreach (var p in Process.GetProcessesByName("Bash"))
                {
                    p.Kill();
                }
                process.Dispose();
                process.Close();
            };

            try
            {
                process.Start();
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //logger.LogError(e, "Command {} failed", cmd);
                //source.SetException(e);
            }
            double timeExcec = 0;
            do
            {
                Console.WriteLine("Process running? " + process.Responding);
                timeExcec = (DateTime.Now - process.StartTime).TotalMinutes;

            } while (timeExcec <= 10); // Correr por 10 minutos
            process.Kill();
            return true;
        }
    }
}
