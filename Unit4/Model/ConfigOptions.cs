using System.Collections.Generic;
using System.Linq;
using Unit4.Automation.Interfaces;
using CommandLine;

namespace Unit4.Automation.Model
{
    [Verb("config", HelpText = "Configure the Unit4 connection details.")]
    internal class ConfigOptions : IOptions
    {
        private readonly int _client;

        public ConfigOptions(int client = 0)
        {
            _client = client;
        }

        [Option(HelpText = "Set the Unit4 client.")]
        public int Client
        {
            get
            {
                return _client;
            }
        }

        public string Url
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }
    }
}