using System.Security;
using Unit4.Automation.Interfaces;

namespace Unit4.Automation.Model
{
    internal class Credentials : ICredentials
    {
        private readonly string _username;
        private readonly SecureString _password;

        public Credentials(string username = "", SecureString password = null)
        {
            _username = username;
            _password = password ?? new SecureString();
        }

        public string Username => _username; 

        public SecureString Password => _password;
    }
}