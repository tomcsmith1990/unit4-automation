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
            var currentOptions = ConfigOptions.Load();

            var updatedOptions = Merge(currentOptions, _options);
            updatedOptions.Save();
        }

        private ConfigOptions Merge(ConfigOptions current, ConfigOptions newer)
        {
            var client = newer.Client == default(int) ? current.Client : newer.Client;
            var url = newer.Url == default(string) ? current.Url : newer.Url;
            return new ConfigOptions(client, url);
        }
    }
}