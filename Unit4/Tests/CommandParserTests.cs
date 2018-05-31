using System;
using Unit4.Automation;
using NUnit.Framework;
using System.IO;
using Command = Unit4.Automation.CommandParser.Command;

namespace Unit4.Automation.Tests
{
    [TestFixture]
    public class CommandParserTests
    {
        private CommandParser _parser;

        [SetUp]
        public void Setup()
        {
            _parser = new CommandParser(TextWriter.Null);
        }

        [Test]
        public void GivenNoArguments_ThenTheCommandShouldBeHelp()
        {
            Assert.That(_parser.GetCommand(), Is.EqualTo(Command.Help));
        }

        [Test]
        public void GivenAnUnknownCommand_ThenTheCommandShouldBeHelp()
        {
            Assert.That(_parser.GetCommand("unknown"), Is.EqualTo(Command.Help));
        }

        [Test]
        public void GivenTheBcrCommand_ThenTheCommandShouldBeBcr()
        {
            Assert.That(_parser.GetCommand("bcr"), Is.EqualTo(Command.Bcr));
        }

        [Test]
        public void GivenTheBcrCommandInADifferentCase_ThenTheCommandShouldBeBcr()
        {
            Assert.That(_parser.GetCommand("BcR"), Is.EqualTo(Command.Bcr));
        }
    }
}