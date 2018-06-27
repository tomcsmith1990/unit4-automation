using System.Security;

namespace Unit4.Automation.Interfaces
{
    internal interface ICredentials
    {
        string Username { get; }

        SecureString Password { get; }
    }
}