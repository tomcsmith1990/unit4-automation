using System;
using System.IO;

namespace Unit4
{
    class Program
    {
        static void Main(string[] args)
        {
            try {
                var credentials = new Credentials();

                var inputFile = Path.Combine(Directory.GetCurrentDirectory(), "Vol Orgs BCR.rerx");
                var outputFile = Path.Combine(Directory.GetCurrentDirectory(), "Vol Orgs BCR.xlsx");

                var engine = new Unit4Engine(credentials);
                engine.RunReport(inputFile, outputFile);
                
                Console.WriteLine("Success");
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                Console.WriteLine();
                Console.WriteLine(e.GetType());
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
