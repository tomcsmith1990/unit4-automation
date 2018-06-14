namespace Unit4.Automation.Interfaces
{
    internal interface ICredentials
    {
        string Username { get; }

        string Password { get; }

        string Client { get; }

        string SoapService { get; }
    }
}