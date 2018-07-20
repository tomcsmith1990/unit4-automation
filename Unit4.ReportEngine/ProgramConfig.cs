using System;

namespace Unit4.Automation
{
    public class ProgramConfig
    {
        private readonly Func<int> _client;
        private readonly Func<string> _url;

        public ProgramConfig(Func<int> client, Func<string> url)
        {
            _client = client;
            _url = url;
        }

        public int Client => _client();

        public string Url => _url();
    }
}