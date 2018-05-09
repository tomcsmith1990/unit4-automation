using System;
using System.Threading.Tasks;

namespace Unit4
{
    internal class ReportRunner
    {
        public void Run()
        {
            try
            {
                var task = Task.Factory.StartNew(() => RunReport(new RerxPackage()));
                Task.WaitAll(task);
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
            
            Console.WriteLine("Success");
        }
    }
}