using System;
using Unit4.Automation.Interfaces;
using Unit4.Automation.Model;

namespace Unit4.Automation
{
    internal class ConfigRunner : IRunner
    {
        private readonly ConfigOptions _options;

        public ConfigRunner(ConfigOptions options)
        {
            _options = options;
        }

        public void Run()
        {
            _options.Save();
        }
    }
}