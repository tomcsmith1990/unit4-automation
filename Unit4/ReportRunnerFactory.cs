using Unit4.Automation.Interfaces;
using Unit4.Automation.Model;
using Unit4.Automation.ReportEngine;
using System.IO;

namespace Unit4.Automation
{
    internal class ReportRunnerFactory
    {
        public IRunner Create(IOptions options)
        {
            var bcrOptions = options as BcrOptions;
            if (bcrOptions != null)
            {
                var log = new Logging();
                var reader = new BcrReader(log);
                var filter = new BcrFilter(bcrOptions);
                var writer = new Excel();
                var pathProvider = new PathProvider(bcrOptions);
                return new BcrReportRunner(log, reader, filter, writer, pathProvider);
            }

            var configOptions = options as ConfigOptions;
            if (configOptions != null)
            {
                var configPath = Path.Combine(Directory.GetCurrentDirectory(), "config.json");
                var file = new ConfigOptionsFile(configPath);
                return new ConfigRunner(configOptions, file);
            }

            return new NullRunner();
        }
    }
}