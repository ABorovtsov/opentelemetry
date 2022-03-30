using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace ConsoleApp1
{

    internal class Program
    {
        static void Main(string[] args)
        {
            var outputFile = ConfugureTracing();

            while (true)
            {
                Trace.Assert(1 > 2, "1>2 is not true");
                Trace.WriteLine("ping", "It's me");
                Console.WriteLine(new string('-', 40));

                Console.WriteLine("Press any key to continue ('q' to quit)");
                if (Console.ReadLine() == "q")
                {
                    break;
                }
            }

            Console.WriteLine("Done");
            Console.WriteLine(new string('=', 40));
            Console.WriteLine($"Output file: {outputFile}");
        }

        private static string ConfugureTracing()
        {
            Trace.Listeners.Clear();
            var directoryFullName = new FileInfo(Assembly.GetEntryAssembly().Location).Directory.FullName;
            var outputFile = directoryFullName + @"\trace\log.txt";
            var textWriterTraceListener = new TextWriterTraceListener(outputFile);
            
            Trace.Listeners.Add(textWriterTraceListener);
            Trace.Listeners.Add(new ConsoleTraceListener());

            Trace.AutoFlush = true;
            return outputFile; 
        }
    }
}
