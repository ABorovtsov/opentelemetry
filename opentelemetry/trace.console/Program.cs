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
            // The config below gives such output in the Dbg View tool

                //[28596] ----DEBUG ASSERTION FAILED ----
                //[28596]---- Assert Short Message----
                //[28596] 1 > 2 is not true
                //[28596]---- Assert Long Message----
                //[28596]
                //[28596]    at ConsoleApp1.Program.Main(String[] args) in C: \Users\aborovtsov\source\repos\opentelemetry\opentelemetry\trace.console\Program.cs:line 17
                //[28596]
                //[28596] It's me: ping 


            var directoryFullName = new FileInfo(Assembly.GetEntryAssembly().Location).Directory.FullName;
            var outputFile = directoryFullName + @"\trace\log.txt";
            var textWriterTraceListener = new TextWriterTraceListener(outputFile);
            
            Trace.Listeners.Clear();
            Trace.Listeners.Add(textWriterTraceListener);
            Trace.Listeners.Add(new ConsoleTraceListener());
            Trace.Listeners.Add(new DefaultTraceListener() {AssertUiEnabled = false}); // it uses "Interop.Kernel32.OutputDebugString"
            Trace.AutoFlush = true;

            return outputFile; 
        }
    }
}
