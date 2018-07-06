using System;
using System.IO;
using CommandLine;
using Unit4.Automation.Interfaces;
using Unit4.Automation.Model;
using System.Linq;

namespace Unit4.Automation.Commands
{
    internal class CommandParser
    {
        private readonly Parser _parser;
        private readonly Type[] _types;

        public CommandParser(TextWriter output, params Type[] types)
        {
            _parser = new Parser(
                settings =>
                {
                    settings.CaseSensitive = false;
                    settings.HelpWriter = output;
                    settings.MaximumDisplayWidth =
                        Console.WindowWidth > 0 ? Console.WindowWidth : 80; //workaround for tests in Travis
                });

            _types = types;
        }

        public IOptions GetOptions(params string[] args)
        {
            if (!_types.Any())
            {
                throw new System.Exception();
            }

            var result = _parser.ParseArguments(args, _types);

            if (result is NotParsed<object>)
            {
                return new NullOptions();
            }

            if (result is Parsed<object>)
            {
                return (result as Parsed<object>).Value as IOptions;
            }

            throw new System.Exception();
        }
    }
}