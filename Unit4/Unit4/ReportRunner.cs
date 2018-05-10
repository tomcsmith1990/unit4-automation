using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Data;
using System.Diagnostics;
using Log = ReportEngine.Diagnostics.Log;

namespace Unit4
{
    internal class ReportRunner
    {
        string logFilePath;
        public void Run()
        {
            try
            {
                logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "log", string.Format("{0}.log", Guid.NewGuid().ToString("N")));
                var logFile = new ReportEngine.Diagnostics.LogFileListener(logFilePath, true);
                ReportEngine.Diagnostics.Log.Level = TraceLevel.Verbose;

                Console.WriteLine(logFilePath);

                ReportEngine.Diagnostics.Log.Open(logFile);

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
                WriteException(e);

                var aggregateException = e as AggregateException;
                if (aggregateException != null)
                {
                    foreach (var exception in aggregateException.InnerExceptions)
                    {
                        WriteException(exception);
                    }
                }

                Console.WriteLine(logFilePath);          
            }
        }

        private void WriteException(Exception exception)
        {
            if (exception is ReportEngine.Data.Sql.LineException)
            {
                Log.Error(((ReportEngine.Data.Sql.LineException)exception).FullLine);
            }

            Log.Error(exception.Message);
            Log.Error(exception.GetType().ToString());
            Log.Error(exception.StackTrace);
        }

        private Task<IEnumerable<BCRLine>> RunBCRTask(string tier3)
        {
            return Task.Factory.StartNew(() => {
                try
                {
                    var bcr = RunBCR(tier3);
                    Console.WriteLine(string.Format("Got BCR for {0}", tier3));
                    return ParseBCR(bcr);
                }
                catch (Exception e)
                {
                    Console.WriteLine(string.Format("Error on {0}", tier3));
                    WriteException(e);
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

        private IEnumerable<BCRLine> ParseBCR(DataSet data)
        {
            foreach (DataRow row in data.Tables[0].Rows)
            {
                yield return new BCRLine() {
                    Tier1 = row["r0r0r0r3dim2"] as string,
                    Tier2 = row["r0r0r3dim2"] as string,
                    Tier3 = row["r0r3dim2"] as string,
                    Tier4 = row["r3dim2"] as string,
                    CostCentre = row["dim2"] as string,
                    Account = row["dim1"] as string,

                    Tier1Name = row["xr0r0r0r3dim2"] as string,
                    Tier2Name = row["xr0r0r3dim2"] as string,
                    Tier3Name = row["xr0r3dim2"] as string,
                    Tier4Name = row["xr3dim2"] as string,
                    CostCentreName = row["xdim2"] as string,
                    AccountName = row["xdim1"] as string,

                    Budget = (double) row["plb_amount"] ,
                    Profile = (double) row["f0_budget_to_da13"] ,
                    Actuals = (double) row["f1_total_exp_to16"] ,
                    Variance = (double) row["f3_variance_to_15"] ,
                    Forecast = (double) row["plf_amount"] ,
                    OutturnVariance = (double) row["f2_outturn_vari18"] 
                };
            }
        }
    }

    internal class BCRLine
    {
        public string Tier1 { get; set; }
        public string Tier2 { get; set; }
        public string Tier3 { get; set; }
        public string Tier4 { get; set; }
        public string CostCentre { get; set; }
        public string Account { get; set; }

        public string Tier1Name { get; set; }
        public string Tier2Name { get; set; }
        public string Tier3Name { get; set; }
        public string Tier4Name { get; set; }
        public string CostCentreName { get; set; }
        public string AccountName { get; set; }

        public double Budget { get; set; }
        public double Profile { get; set; }
        public double Actuals { get; set; }
        public double Variance { get; set; }
        public double Forecast { get; set; }
        public double OutturnVariance { get; set; }
    }
}