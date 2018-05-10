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
                var costCentres = GetCostCentres();

                var tasks = costCentres.Select(RunBCRTask).ToArray();

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

        private Task<DataSet> RunBCRTask(string costCentre)
        {
            return Task.Factory.StartNew(() => {
                var bcr = RunBCR(costCentre);
                Console.WriteLine(string.Format("Got BCR for {0}", costCentre));
                return bcr;
            });
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

        private IEnumerable<string> GetCostCentres()
        {
            var data = RunReport(Resql.GetCostCentreList);
            foreach (DataRow row in data.Tables[0].Rows)
            {
                var costCentre = row["dim_value"] as string;
                var costCentreName = row["xdim_value"] as string;
                if (costCentre.StartsWith("3000"))
                {
                    yield return costCentre;
                }
            }
        }
    }
}