using System.Security;

namespace Unit4.ReportEngine
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