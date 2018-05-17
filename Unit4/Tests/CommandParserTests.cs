using System;
using Unit4.Automation;
using NUnit.Framework;
using Command = Unit4.Automation.CommandParser.Command;

namespace Unit4.Automation.Tests
{
    [TestFixture]
    public class CommandParserTests
    {
        [Test]
        public void GivenNoArguments_ThenTheCommandShouldBeHelp()
        {
            var commandParser = new CommandParser();

            Assert.That(commandParser.GetCommand(), Is.EqualTo(Command.Help));
        }

        [Test]
        public void GivenAnUnknownCommand_ThenTheCommandShouldBeHelp()
        {
            var commandParser = new CommandParser();

            Assert.That(commandParser.GetCommand("unknown"), Is.EqualTo(Command.Help));
        }

        [Test]
        public void GivenTheBcrCommand_ThenTheCommandShouldBeBcr()
        {
            var commandParser = new CommandParser();

            Assert.That(commandParser.GetCommand("bcr"), Is.EqualTo(Command.Bcr));
        }

        [Test]
        public void GivenTheBcrCommandInADifferentCase_ThenTheCommandShouldBeBcr()
        {
            var commandParser = new CommandParser();

            Assert.That(commandParser.GetCommand("BcR"), Is.EqualTo(Command.Bcr));
        }
    }
}