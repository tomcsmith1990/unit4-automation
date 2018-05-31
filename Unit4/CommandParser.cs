using System;
using System.Linq;
using CommandLine;

namespace Unit4.Automation
{
    internal class CommandParser
    {
        public enum Command { Help, Bcr };

        public Command GetCommand(params string[] args)
        {
            var parser = new Parser(settings => settings.CaseSensitive = false);
            return parser
                .ParseArguments(args, typeof(BcrOptions))
                .MapResult<BcrOptions, Command>(options => Command.Bcr, errors => Command.Help);
        }
    }

    [Verb("bcr")]
    internal class BcrOptions
    {

    }
}