namespace Unit4.Automation
{
    internal class ProgramConfig
    {
        public ProgramConfig(int client, string url)
        {
            Client = client;
            Url = url;
        }

        public int Client { get; }

        public string Url { get; }
    }
}