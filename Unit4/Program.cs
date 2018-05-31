using System;
using System.Linq;
using Unit4.Automation.Model;
using Command = Unit4.Automation.CommandParser.Command;

namespace Unit4.Automation
{
    class Program
    {
        static void Main(string[] args)
        {
            var command = new CommandParser(Console.Out).GetCommand(args);

            if (command.GetType() == typeof(BcrOptions))
            {
                new ReportRunner().Run();
            }
            else
            {
                Help();
            }
        }

        private static void Help()
        {
            var commands = Enum.GetNames(typeof(Command)).Select(x => x.ToLowerInvariant()).ToArray();
            Console.WriteLine(string.Format(@"Unit4 Automation

Available commands:
{0}", string.Join(Environment.NewLine, commands)));
        }
    }
}
