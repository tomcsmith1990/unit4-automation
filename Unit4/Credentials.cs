using System;
using Unit4.Automation.Interfaces;

namespace Unit4.Automation
{
    internal class Credentials : ICredentials
    {
        public string Username
        { 
            get 
            { 
                return ""; 
            }
        }

        public string Password
        { 
            get 
            { 
                return ""; 
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