using System;
using Unit4.Automation.Interfaces;
using Unit4.Automation.Model;

namespace Unit4.Automation.Commands.ConfigCommand
{
    internal class ConfigRunner : IRunner
    {
        private readonly ConfigOptions _options;
        private readonly ConfigOptionsFile _file;

        public ConfigRunner(ConfigOptions options, ConfigOptionsFile file)
        {
            _options = options;
            _file = file;
        }

        public void Run()
        {
            if (_file.Exists())
            {
                var currentOptions = _file.Load();
                var updatedOptions = Merge(currentOptions, _options);
                _file.Save(updatedOptions);
            }
            else
            {
                _file.Save(_options);
            }
        }

        private ConfigOptions Merge(ConfigOptions current, ConfigOptions newer)
        {
            var client = newer.Client == default(int) ? current.Client : newer.Client;
            var url = newer.Url == default(string) ? current.Url : newer.Url;
            return new ConfigOptions(client, url);
        }
    }
}