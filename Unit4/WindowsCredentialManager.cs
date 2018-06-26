using Unit4.Automation.Interfaces;
using Unit4.Automation.Model;
using CredentialManagement;

namespace Unit4.Automation
{
    internal class WindowsCredentialManager : ICredentialManager
    {
        public ICredentials CredentialsOrDefault
        {
            get
            {
                using (var credential = new Credential() { Target = "Unit4 Automation" })
                {
                    if (credential.Load())
                    {
                        return new Credentials(credential.Username, credential.SecurePassword.Copy());
                    }
                }

                return new Credentials();
            }
        }
    }
}