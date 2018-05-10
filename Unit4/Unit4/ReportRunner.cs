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

                var tier3s = GetTier3List();

                var tasks = tier3s.Select(RunBCRTask).ToArray();

                Task.WaitAll(tasks);

                Console.WriteLine("Merging data");
                
                var lines = tasks.SelectMany(x => x.Result);

                Console.WriteLine("Writing to Excel");
                var outputPath = Path.Combine(Directory.GetCurrentDirectory(), "output", string.Format("{0}.xlsx", Guid.NewGuid().ToString("N")));
                new Excel().WriteToExcel(outputPath, lines);

                stopwatch.Stop();

                Console.WriteLine(string.Format("Success - {0}", outputPath));

                Console.WriteLine(string.Format("Time: {0}ms", stopwatch.ElapsedMilliseconds));
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

        private IEnumerable<string> GetTier3List()
        {
            var tier3List = new List<string>();

            var data = RunReport(Resql.GetCostCentreList);
            foreach (DataRow row in data.Tables[0].Rows)
            {
                var tier3 = row["r0r1dim_value"] as string;
                var tier3Name = row["xr0r1dim_value"] as string;
                var costCentre = row["dim_value"] as string;
                var costCentreName = row["xdim_value"] as string;
                if (costCentre.StartsWith("3000"))
                {
                    tier3List.Add(tier3);
                }
            }
            return tier3List.Distinct().Where(x => x.StartsWith("30T3"));
        }
    }
}