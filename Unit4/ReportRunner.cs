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

namespace Unit4.Automation
{
    internal class ReportRunner
    {
        private readonly ILogging _log = new Logging();
        private readonly BcrLineBuilder _builder = new BcrLineBuilder();
        private readonly CostCentreHierarchy _hierarchy = new CostCentreHierarchy(new CostCentreList());

        public void Run()
        {
            try
            {
                _log.Start();

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                long elapsed = 0, current = 0;

                Console.WriteLine("Getting cost centre hierarchy");

                var tier3Hierarchy = _hierarchy.GetHierarchyByTier3();

                current = stopwatch.ElapsedMilliseconds;
                Console.WriteLine(string.Format("Elapsed: {0}ms", current - elapsed));
                elapsed = current;

                Console.WriteLine("Getting BCRs");

                var factory = new Unit4EngineFactory();
                var bcrReport = new BcrReport(factory, _log);

                var lines = bcrReport.RunBCR(tier3Hierarchy);

                current = stopwatch.ElapsedMilliseconds;
                Console.WriteLine(string.Format("Elapsed: {0}ms", current - elapsed));
                elapsed = current;

                Console.WriteLine("Writing to Excel");

                var outputPath = Path.Combine(Directory.GetCurrentDirectory(), "output", string.Format("{0}.xlsx", Guid.NewGuid().ToString("N")));
                new Excel().WriteToExcel(outputPath, lines);

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