using System;
using Command = Unit4.Automation.CommandParser.Command;

namespace Unit4.Automation
{
    class Program
    {
        static void Main(string[] args)
        {
            var command = new CommandParser().GetCommand(args);

            switch (command)
            {
                case Command.Bcr:
                    new ReportRunner().Run();
                    break;
                case Command.Help:
                default:
                    break;
            }
        }
    }
}
