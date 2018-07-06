using System.IO;
using NUnit.Framework;
using Unit4.Automation.Commands;
using Unit4.Automation.Model;

namespace Unit4.Automation.Tests.Parser
{
    [TestFixture]
    internal class CommandParserTests
    {
        [SetUp]
        public void Setup()
        {
            _parser = new CommandParser<BcrOptions, ConfigOptions>(TextWriter.Null);
        }

        private CommandParser<BcrOptions, ConfigOptions> _parser;

        [Test]
        public void GivenAnUnknownCommand_ThenTheCommandShouldBeHelp()
        {
            Assert.That(_parser.GetOptions("unknown"), Is.TypeOf(typeof(NullOptions)));
        }

        [Test]
        public void GivenNoArguments_ThenTheCommandShouldBeHelp()
        {
            Assert.That(_parser.GetOptions(), Is.TypeOf(typeof(NullOptions)));
        }
    }
}