using Unit4.Automation.Interfaces;
using Unit4.Automation.Model;
using Unit4.Automation.ReportEngine;
using System.IO;
using Unit4.Automation.Commands.BcrCommand;

namespace Unit4.Automation
{
    internal class ReportRunnerFactory
    {
        public IRunner Create(IOptions options)
        {
            var configPath = Path.Combine(Directory.GetCurrentDirectory(), "config.json");
            var file = new ConfigOptionsFile(configPath);
            
            var bcrOptions = options as BcrOptions;
            if (bcrOptions != null)
            {
                var log = new Logging();
                var config = file.Exists() ? file.Load() : new ConfigOptions();
                var reader = new BcrReader(log, config);
                var filter = new BcrFilter(bcrOptions);
                var writer = new Excel();
                var pathProvider = new PathProvider(bcrOptions);
                return new BcrReportRunner(log, reader, filter, writer, pathProvider);
            }

            var configOptions = options as ConfigOptions;
            if (configOptions != null)
            {
                return new ConfigRunner(configOptions, file);
            }

            return new NullRunner();
        }
    }
}