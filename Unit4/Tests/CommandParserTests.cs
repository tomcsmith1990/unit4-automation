using System;
using Unit4.Automation;
using NUnit.Framework;
using System.IO;
using Unit4.Automation.Model;

namespace Unit4.Automation.Tests
{
    [TestFixture]
    public class CommandParserTests
    {
        private CommandParser<BcrOptions> _parser;

        [SetUp]
        public void Setup()
        {
            _parser = new CommandParser<BcrOptions>(TextWriter.Null);
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

        [Test]
        public void GivenTheBcrCommand_ThenTheCommandShouldBeBcr()
        {
            Assert.That(_parser.GetOptions("bcr"), Is.TypeOf(typeof(BcrOptions)));
        }

        [Test]
        public void GivenTheBcrCommandInADifferentCase_ThenTheCommandShouldBeBcr()
        {
            Assert.That(_parser.GetOptions("BcR"), Is.TypeOf(typeof(BcrOptions)));
        }

        [Test]
        public void GivenTheBcrCommand_ThenTheTier2OptionShouldBeRecognised()
        {
            var options = _parser.GetOptions("bcr", "--tier2=00T2000");
            var bcrOptions = options as BcrOptions;
            
            Assert.That(bcrOptions.Tier2, Is.EqualTo("00T2000"));
        }
    }
}