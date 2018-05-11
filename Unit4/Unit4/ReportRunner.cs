using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Data;
using System.Diagnostics;
using System.Collections.Concurrent;

namespace Unit4
{
    internal class ReportRunner
    {
        private readonly Logging _log = new Logging();

        public void Run()
        {
            try
            {
                _log.Start();

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                long elapsed = 0, current = 0;

                Console.WriteLine("Getting cost centre hierarchy");

                var tier3s = new CostCentreHierarchy().GetCostCentres().Select(x => x.Tier3).Distinct();

                current = stopwatch.ElapsedMilliseconds;
                Console.WriteLine(string.Format("Elapsed: {0}ms", current - elapsed));
                elapsed = current;

                Console.WriteLine("Getting BCRs");

                var bag = new ConcurrentBag<BCRLine>();

                Parallel.ForEach(tier3s, new ParallelOptions { MaxDegreeOfParallelism = 3 }, t =>
                {
                    var bcrLines = RunBCRTask(t);
                    foreach (var line in bcrLines)
                    {
                        bag.Add(line);
                    }
                });

                current = stopwatch.ElapsedMilliseconds;
                Console.WriteLine(string.Format("Elapsed: {0}ms", current - elapsed));
                elapsed = current;

                Console.WriteLine("Writing to Excel");

                var outputPath = Path.Combine(Directory.GetCurrentDirectory(), "output", string.Format("{0}.xlsx", Guid.NewGuid().ToString("N")));
                new Excel().WriteToExcel(outputPath, bag);

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

        private IEnumerable<BCRLine> RunBCRTask(string tier3)
        {
            try
            {
                var bcr = RunBCR(tier3);
                _log.Info(string.Format("Got BCR for {0}", tier3));

                return BCRLine.FromDataSet(bcr);
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("Error on {0}", tier3));
                _log.Error(e);
                return new List<BCRLine>();
            }
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