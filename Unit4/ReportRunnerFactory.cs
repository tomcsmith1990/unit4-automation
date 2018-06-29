using Unit4.Automation.Interfaces;
using Unit4.Automation.Model;
using Unit4.Automation.ReportEngine;
using System.IO;
using Unit4.Automation.Commands.BcrCommand;
using Unit4.Automation.Commands.ConfigCommand;

namespace Unit4.Automation
{
    internal class ReportRunnerFactory
    {
        private readonly ConfigOptionsFile _file;

        public ReportRunnerFactory()
        {
            var configPath = Path.Combine(Directory.GetCurrentDirectory(), "config.json");
            _file = new ConfigOptionsFile(configPath);
        }

        public IRunner Create(IOptions options)
        { 
            var bcrOptions = options as BcrOptions;
            if (bcrOptions != null)
            {
                return BcrReportRunner.Create(bcrOptions, GetConfig());
            }

            var configOptions = options as ConfigOptions;
            if (configOptions != null)
            {
                return new ConfigRunner(configOptions, _file);
            }

            return new NullRunner();
        }

        private ProgramConfig GetConfig()
        {
            var config = _file.Exists() ? _file.Load() : new ConfigOptions();
            return new ProgramConfig(config.Client, config.Url);
        }
    }
}