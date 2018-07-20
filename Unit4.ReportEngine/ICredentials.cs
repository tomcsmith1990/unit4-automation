using System.Security;

namespace Unit4.ReportEngine
{
    internal interface ICredentials
    {
        string Username { get; }

        SecureString Password { get; }
    }
}