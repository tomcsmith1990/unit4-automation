using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Data;
using System.Diagnostics;

namespace Unit4
{
    internal class ReportRunner
    {
        private readonly Logging _log = new Logging();

        public void Run()
        {
            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                long elapsed = 0, current = 0;

                Console.WriteLine("Getting cost centre hierarchy");

                var tier3s = new CostCentreHierarchy().GetTier3List();

                current = stopwatch.ElapsedMilliseconds;
                Console.WriteLine(string.Format("Elapsed: {0}ms", current - elapsed));
                elapsed = current;

                Console.WriteLine("Getting BCRs");

                var tasks = tier3s.Select(RunBCRTask).ToArray();

                Task.WaitAll(tasks);

                current = stopwatch.ElapsedMilliseconds;
                Console.WriteLine(string.Format("Elapsed: {0}ms", current - elapsed));
                elapsed = current;
                
                Console.WriteLine("Combining rows");

                var lines = tasks.SelectMany(x => x.Result);

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
            }
            catch (Exception e)
            {    
                _log.Error(e);                
                Console.WriteLine(_log.Path);          
            }
        }

        private Task<IEnumerable<BCRLine>> RunBCRTask(string tier3)
        {
            return Task.Factory.StartNew(() => {
                try
                {
                    var bcr = RunBCR(tier3);
                    Console.WriteLine(string.Format("Got BCR for {0}", tier3));
                    return BCRLine.FromDataSet(bcr);
                }
                catch (Exception e)
                {
                    Console.WriteLine(string.Format("Error on {0}", tier3));
                    _log.Error(e);
                    return new List<BCRLine>();
                }
            });
        }

        private DataSet RunBCR(string tier3)
        {
            return RunReport(string.Format(Resql.BcrByTier3, tier3));
        }

        private DataSet RunReport(string resql)
        {
            var credentials = new Credentials();

            var engine = new Unit4Engine(credentials);
            return engine.RunReport(resql);
        }
    }
}