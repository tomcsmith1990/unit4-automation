using System;
using ReportEngine;
using ReportEngine.Base.Data.Provider;
using ReportEngine.Interfaces;
using ReportEngine.Base.Security;
using ReportEngine.Base.Interfaces;
using ReportEngine.Provider.WebService;
using System.Data;
using ReportEngine.IO;
using ReportEngine.Data;
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

                var agressoAuthenticator = new AgressoAuthenticator();
                agressoAuthenticator.Password = SecureStringHelper.ToSecureString(credentials.Password);
                
                var authenticators = new BaseAuthenticator[1]
                {
                    agressoAuthenticator
                };
                var connector = new WebProviderConnector() 
                { 
                    Name = "WebService",
                    Authenticators = authenticators,
                    Authenticator = agressoAuthenticator,
                    Datasource = credentials.SoapService
                };
                
                agressoAuthenticator.Name = credentials.Username;
                agressoAuthenticator.Client = credentials.Client;

                var engine = new Engine(new WebProvider(connector), ClientType.External);
                engine.InProcess = true;
                using (engine)
                {
                    DataSet dataset = null;
                    engine.RunReportEx(inputFile, outputFile, false);
                    Console.WriteLine("Success");
                }
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
