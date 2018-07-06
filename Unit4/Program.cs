using System;
using Unit4.Automation.Commands;
using Unit4.Automation.Model;

namespace Unit4.Automation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var options = new CommandParser<BcrOptions, ConfigOptions>(Console.Out).GetOptions(args);

            var runner = new ReportRunnerFactory().Create(options);

            runner.Run();
        }
    }
}