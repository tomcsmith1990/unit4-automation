using Unit4.Automation.Interfaces;
using CommandLine;
using System;

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
        public int Client => _client;

        [Option(HelpText = "Set the Unit4 SOAP service URL.")]
        public string Url
        {
            get
            {
                if (_url == null)
                {
                    throw new ApplicationException("The Unit4 SOAP service URL is not set in the config file.");
                }
                return _url;
            }
        }

        public override bool Equals(object o)
        {
            var otherConfig = o as ConfigOptions;
            if (otherConfig == null)
            {
                return false;
            }

            return otherConfig.Client == this.Client && string.Equals(otherConfig.Url, this.Url);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int) 2166136261;
                hash = hash * 16777619 + Client.GetHashCode();
                hash = hash * 16777619 + (Url == null ? 0 : Url.GetHashCode());
                return hash;
            }
        }

        public override string ToString() => $"Client = {Client}; Url = {Url}";
    }
}