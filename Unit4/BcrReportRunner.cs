using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Data;
using System.Diagnostics;
using System.Collections.Concurrent;
using Unit4.Automation.Interfaces;
using Unit4.Automation.ReportEngine;
using Unit4.Automation.Model;

namespace Unit4.Automation
{
    internal class BcrReportRunner : IRunner
    {
        private readonly ILogging _log;

        public BcrReportRunner()
        {
            _log = new Logging();
        }

        public void Run()
        {
            try
            {
                _log.Start();

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                long elapsed = 0, current = 0;

                Console.WriteLine("Getting BCRs");

                var bcr = new BcrReader(_log).Read();

                current = stopwatch.ElapsedMilliseconds;
                Console.WriteLine(string.Format("Elapsed: {0}ms", current - elapsed));
                elapsed = current;

                Console.WriteLine("Writing to Excel");

                var outputPath = Path.Combine(Directory.GetCurrentDirectory(), "output", string.Format("{0}.xlsx", Guid.NewGuid().ToString("N")));
                new Excel().WriteToExcel(outputPath, bcr);

                stopwatch.Stop();

                current = stopwatch.ElapsedMilliseconds;
                Console.WriteLine(string.Format("Elapsed: {0}ms", current - elapsed));
                elapsed = current;

                Console.WriteLine(string.Format("Success - {0}", outputPath));

                Console.WriteLine(string.Format("Total time elapsed: {0}ms", elapsed));
            }
            catch (Exception e)
            {    
                _log.Error(e);
                Console.WriteLine(_log.Path);        
            }
            finally
            {
                _log.Close();
            }
        }
    }
}