using Unit4.Automation.Interfaces;
using Unit4.Automation.Model;

namespace Unit4.Automation
{
    internal class WindowsCredentialManager : ICredentialManager
    {
        public ICredentials CredentialsOrDefault
        {
            get
            {
                return new Credentials();
            }
        }
    }
}