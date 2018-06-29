using System.Security;
using Unit4.Automation.Interfaces;

namespace Unit4.Automation.Model
{
    internal class Credentials : ICredentials
    {
        public Credentials(string username = "", SecureString password = null)
        {
            Username = username;
            Password = password ?? new SecureString();
        }

        public string Username { get; } 

        public SecureString Password { get; }
    }
}