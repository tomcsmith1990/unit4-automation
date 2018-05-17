using System;
using Unit4.Automation;
using NUnit.Framework;

namespace Unit4.Automation.Tests
{
    [TestFixture]
    public class CommandParserTests
    {
        [Test]
        public void GivenNoArguments_ThenTheCommandShouldBeHelp()
        {
            var commandParser = new CommandParser();

            Assert.That(commandParser.GetCommand(), Is.EqualTo(CommandParser.Command.Help));
        }
    }
}