using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Unit4
{
    internal class ReportRunner
    {
        public void Run()
        {
            try
            {
                var packages = new List<RerxPackage>() {
                    new RerxPackage("Vol Orgs BCR.rerx", "Vol Orgs BCR.xlsx"),
                    new RerxPackage("Transport BCR.rerx", "Transport BCR.xlsx")
                };

                var tasks = packages.Select(p => Task.Factory.StartNew(() => RunReport(p))).ToArray();

                Task.WaitAll(tasks);
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                Console.WriteLine();
                Console.WriteLine(e.GetType());
                Console.WriteLine(e.StackTrace);
            }
        }

        private void RunReport(RerxPackage rerxPackage)
        {
            var credentials = new Credentials();

            var engine = new Unit4Engine(credentials);
            engine.RunReport(rerxPackage.InputPath, rerxPackage.OutputPath);
            
            Console.WriteLine(string.Format("Success - {0}", rerxPackage.OutputPath));
        }
    }
}