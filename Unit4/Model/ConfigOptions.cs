using CommandLine;
using Unit4.Automation.Interfaces;
using System.ComponentModel.Composition;
using Newtonsoft.Json;

namespace Unit4.Automation.Model
{
    [Export(typeof(IOptions))]
    [Verb("config", HelpText = "Configure the Unit4 connection details.")]
    internal class ConfigOptions : IOptions
    {
        public ConfigOptions() : this(0, null)
        {
        }

        [JsonConstructor]
        public ConfigOptions(int client, string url)
        {
            Client = client;
            Url = url;
        }

        [Option(HelpText = "Set the Unit4 client.")]
        public int Client { get; }

        [Option(HelpText = "Set the Unit4 SOAP service URL.")]
        public string Url { get; }

        public override bool Equals(object o)
        {
            var otherConfig = o as ConfigOptions;
            if (otherConfig == null)
            {
                return false;
            }

            return otherConfig.Client == Client && string.Equals(otherConfig.Url, Url);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int) 2166136261;
                hash = hash * 16777619 ^ Client.GetHashCode();
                hash = hash * 16777619 ^ (Url == null ? 0 : Url.GetHashCode());
                return hash;
            }
        }

        public override string ToString() => $"Client = {Client}; Url = {Url}";
    }
}