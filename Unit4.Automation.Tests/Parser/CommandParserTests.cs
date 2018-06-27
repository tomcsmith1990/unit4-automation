using System;
using NUnit.Framework;
using System.IO;
using Unit4.Automation.Model;
using System.Linq;
using Criteria = Unit4.Automation.Tests.Helpers.A.Criteria;
using Unit4.Automation.Tests.Helpers;
using System.Collections.Generic;
using System.Text;
using Unit4.Automation.Commands;

namespace Unit4.Automation.Tests.Parser
{
    [TestFixture]
    internal class CommandParserTests
    {
        private CommandParser<BcrOptions, ConfigOptions> _parser;

        [SetUp]
        public void Setup()
        {
            _parser = new CommandParser<BcrOptions, ConfigOptions>(TextWriter.Null);
        }

        [Test]
        public void GivenNoArguments_ThenTheCommandShouldBeHelp()
        {
            Assert.That(_parser.GetOptions(), Is.TypeOf(typeof(NullOptions)));
        }

        [Test]
        public void GivenAnUnknownCommand_ThenTheCommandShouldBeHelp()
        {
            Assert.That(_parser.GetOptions("unknown"), Is.TypeOf(typeof(NullOptions)));
        }
    }
}