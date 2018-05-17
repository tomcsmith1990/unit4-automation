using System;
using System.Linq;

namespace Unit4.Automation
{
    internal class CommandParser
    {
        public enum Command { Help, Bcr };

        public Command GetCommand(params string[] args)
        {
            if (!args.Any())
            {
                return Command.Help;
            }

            switch (args.First())
            {
                case "bcr": return Command.Bcr;
                default:    return Command.Help;
            }
        }
    }
}