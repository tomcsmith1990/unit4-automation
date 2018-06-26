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

        public string Username
        { 
            get 
            { 
                return _username; 
            }
        }

        public SecureString Password
        {
            get
            {
                return _password;
            }
        }

        public string Client
        { 
            get 
            { 
                return ""; 
            }
        }

        public string SoapService
        { 
            get 
            { 
                return ""; 
            }
        }
    }
}