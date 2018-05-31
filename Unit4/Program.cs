using System;
using System.Linq;
using Command = Unit4.Automation.CommandParser.Command;

namespace Unit4.Automation
{
    class Program
    {
        static void Main(string[] args)
        {
            var command = new CommandParser(Console.Out).GetCommand(args);

            switch (command)
            {
                case Command.Bcr:
                    new ReportRunner().Run();
                    break;
                case Command.Help:
                default:
                    Help();
                    break;
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
