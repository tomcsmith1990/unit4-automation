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
        private readonly string _url;

        public ConfigOptions(int client = 0, string url = null)
        {
            _client = client;
            _url = url;
        }

        [Option(HelpText = "Set the Unit4 client.")]
        public int Client
        {
            get
            {
                return _client;
            }
        }

        [Option(HelpText = "Set the Unit4 SOAP service URL.")]
        public string Url
        {
            get
            {
                return _url;
            }
        }
    }
}