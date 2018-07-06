using System;
using Unit4.Automation.Commands;
using Unit4.Automation.Model;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Collections.Generic;
using Unit4.Automation.Interfaces;
using System.Linq;

namespace Unit4.Automation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var provider = new OptionsProvider();
            var options = new CommandParser(Console.Out, provider.Types.ToArray()).GetOptions(args);

            var runner = new ReportRunnerFactory().Create(options);

            runner.Run();
        }
    }
}