using System;
using Unit4.Automation.Interfaces;
using Unit4.Automation.Model;
using CredentialManagement;

namespace Unit4.Automation
{
    internal class WindowsCredentialManager : ICredentialManager
    {
        private const string _credentialName = "Unit4 Automation";

        public ICredentials Credentials
        {
            get
            {
                using (var credential = new Credential() { Target = _credentialName })
                {
                    if (credential.Load())
                    {
                        return new Credentials(credential.Username, credential.SecurePassword.Copy());
                    }
                }

                throw new MissingCredentialsException();
            }
        }

        private class MissingCredentialsException : Exception
        {
            public MissingCredentialsException()
                : this(string.Format("The {0} credential does not exist in Credential Manager. Please create it.", _credentialName))
            {
            }

            public MissingCredentialsException(string message) : base(message)
            {
            }

            public MissingCredentialsException(string message, Exception inner) : base(message, inner)
            {
            }
        }
    }
}