using System;
using System.IO;
using System.Threading.Tasks;

namespace Unit4
{
    internal class ReportRunner
    {
        public void Run()
        {
            try
            {
                var task = Task.Factory.StartNew(() => RunReport());
                Task.WaitAll(task);
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                Console.WriteLine();
                Console.WriteLine(e.GetType());
                Console.WriteLine(e.StackTrace);
            }
        }

        private void RunReport()
        {
            var credentials = new Credentials();

            var inputFile = Path.Combine(Directory.GetCurrentDirectory(), "Vol Orgs BCR.rerx");
            var outputFile = Path.Combine(Directory.GetCurrentDirectory(), "Vol Orgs BCR.xlsx");

            var engine = new Unit4Engine(credentials);
            engine.RunReport(inputFile, outputFile);
            
            Console.WriteLine("Success");
        }
    }
}