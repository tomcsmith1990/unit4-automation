using System;
using System.Linq;
using System.IO;
using CommandLine;
using Unit4.Automation.Interfaces;
using Unit4.Automation.Model;

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

        public IOptions GetCommand(params string[] args)
        {
            return _parser
                .ParseArguments(args, typeof(BcrOptions))
                .MapResult<BcrOptions, IOptions>(options => options, errors => new NullOptions());
        }
    }
}