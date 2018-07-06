using System;
using System.IO;
using CommandLine;
using Unit4.Automation.Interfaces;
using Unit4.Automation.Model;

namespace Unit4.Automation.Commands
{
    internal class CommandParser<TVerb, TVerb2>
        where TVerb : IOptions
        where TVerb2 : IOptions
    {
        private readonly Parser _parser;

        public CommandParser(TextWriter output)
        {
            _parser = new Parser(
                settings =>
                {
                    settings.CaseSensitive = false;
                    settings.HelpWriter = output;
                    settings.MaximumDisplayWidth =
                        Console.WindowWidth > 0 ? Console.WindowWidth : 80; //workaround for tests in Travis
                });
        }

        public IOptions GetOptions(params string[] args)
        {
            return _parser
                .ParseArguments(args, typeof(TVerb), typeof(TVerb2))
                .MapResult<TVerb, TVerb2, IOptions>(
                    options => options,
                    options => options,
                    errors => new NullOptions());
        }
    }
}