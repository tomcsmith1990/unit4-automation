using System.Security;

namespace Unit4.Automation.Model
{
    internal class WindowsCredentials
    {
        private readonly string _username;
        private readonly SecureString _password;

        public WindowsCredentials(string username, SecureString password)
        {
            _username = username;
            _password = password;
        }

        public string Username { get { return _username; } }

        public SecureString Password { get { return _password; } }
    }
}