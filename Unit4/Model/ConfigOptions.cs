using System.Collections.Generic;
using System.Linq;
using Unit4.Automation.Interfaces;
using CommandLine;

namespace Unit4.Automation.Model
{
    [Verb("config", HelpText = "Configure the Unit4 connection details.")]
    internal class ConfigOptions : IOptions
    {

    }
}