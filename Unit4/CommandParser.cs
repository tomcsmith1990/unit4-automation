using System;
using System.Linq;
using System.IO;
using CommandLine;

namespace Unit4.Automation
{
    internal class CommandParser
    {
        public enum Command { Help, Bcr };
        private readonly Parser _parser;

        public CommandParser(TextWriter output)
        {
            _parser = new Parser(settings => {
                settings.CaseSensitive = false;
                settings.HelpWriter = output;
            });
        }

        public Command GetCommand(params string[] args)
        {
            return _parser
                .ParseArguments(args, typeof(BcrOptions))
                .MapResult<BcrOptions, Command>(options => Command.Bcr, errors => Command.Help);
        }
    }

    [Verb("bcr", HelpText = "Produce a BCR.")]
    internal class BcrOptions
    {

    }
}