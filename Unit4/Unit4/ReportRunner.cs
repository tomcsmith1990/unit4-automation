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
        private readonly BCRLineBuilder _builder = new BCRLineBuilder();
        private readonly CostCentreHierarchy _hierarchy = new CostCentreHierarchy();

        public void Run()
        {
            try
            {
                _log.Start();

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                long elapsed = 0, current = 0;

                Console.WriteLine("Getting cost centre hierarchy");

                var costCentreByTier3 = _hierarchy.GetHierarchyByTier3();

                current = stopwatch.ElapsedMilliseconds;
                Console.WriteLine(string.Format("Elapsed: {0}ms", current - elapsed));
                elapsed = current;

                Console.WriteLine("Getting BCRs");

                var bag = new ConcurrentBag<BCRLine>();

                Parallel.ForEach(costCentreByTier3, new ParallelOptions { MaxDegreeOfParallelism = 3 }, t =>
                {
                    var bcrLines = RunBCR(t);
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

        private IEnumerable<BCRLine> RunBCR(IGrouping<string, string> hierarchy)
        {
            try
            {
                var bcr = RunBCRTier3(hierarchy.Key);
                _log.Info(string.Format("Got BCR for {0}", hierarchy.Key));

                return _builder.Build(bcr);
            }
            catch (Exception e)
            {
                _log.Error(string.Format("Error getting BCR for {0}, falling back to tier 4", hierarchy.Key));
                _log.Error(e);
                return hierarchy.Distinct().SelectMany(x => RunBCRTier4(x));
            }
        }

        private DataSet RunBCRTier3(string tier3)
        {
            return RunReport(string.Format(Resql.BcrByTier3, tier3));
        }

        private IEnumerable<BCRLine> RunBCRTier4(string tier4)
        {
            try
            {
                var bcr = RunReport(string.Format(Resql.BcrByTier4, tier4));
                _log.Info(string.Format("Got BCR for {0}", tier4));

                return _builder.Build(bcr);
            }
            catch (Exception e)
            {
                _log.Error(string.Format("Error getting BCR for {0}", tier4));
                _log.Error(e);
                return Enumerable.Empty<BCRLine>();
            }
        }

        private DataSet RunReport(string resql)
        {
            var credentials = new Credentials();

            var engine = new Unit4Engine(credentials);
            return engine.RunReport(resql);
        }
    }
}