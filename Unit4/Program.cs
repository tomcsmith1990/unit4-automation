using System;
using Unit4.Automation.Model;
using Unit4.Automation.Commands;

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
