using System;
using System.Linq;
using Unit4.Automation.Model;

namespace Unit4.Automation
{
    class Program
    {
        static void Main(string[] args)
        {
            var command = new CommandParser<BcrOptions>(Console.Out).GetCommand(args);

            var runner = new ReportRunnerFactory().Create(command);

            runner.Run();
        }
    }
}
