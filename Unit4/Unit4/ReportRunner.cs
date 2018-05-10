using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Data;

namespace Unit4
{
    internal class ReportRunner
    {
        public void Run()
        {
            try
            {
                var costCentres = new List<string>() {
                    "30001976",
                    "30002006"
                };

                var tasks = costCentres.Select(x => Task.Factory.StartNew(() => RunBCR(x))).ToArray();

                Task.WaitAll(tasks);

                var mergedData = new DataSet();

                foreach (var t in tasks)
                {
                    mergedData.Merge(t.Result);
                }

                var outputPath = Path.Combine(Directory.GetCurrentDirectory(), "output", string.Format("{0}.xlsx", Guid.NewGuid().ToString("N")));
                new Excel().WriteToExcel(outputPath, mergedData);

                Console.WriteLine(string.Format("Success - {0}", outputPath));
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                Console.WriteLine();
                Console.WriteLine(e.GetType());
                Console.WriteLine(e.StackTrace);
            }
        }

        private DataSet RunBCR(string costCentre)
        {
            return RunReport(string.Format(Resql.BcrByCostCentre, costCentre));
        }

        private DataSet RunReport(string resql)
        {
            var credentials = new Credentials();

            var engine = new Unit4Engine(credentials);
            return engine.RunReport(resql);
        }
    }
}