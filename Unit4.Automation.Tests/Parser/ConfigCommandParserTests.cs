using System;
using NUnit.Framework;
using System.IO;
using Unit4.Automation.Model;
using System.Linq;
using Criteria = Unit4.Automation.Tests.Helpers.A.Criteria;
using Unit4.Automation.Tests.Helpers;
using System.Collections.Generic;
using System.Text;

namespace Unit4.Automation.Tests.Parser
{
    [TestFixture]
    internal class ConfigCommandParserTests
    {
        private CommandParser<BcrOptions, ConfigOptions> _parser;

        [SetUp]
        public void Setup()
        {
            _parser = new CommandParser<BcrOptions, ConfigOptions>(TextWriter.Null);
        }

        [Test]
        public void GivenTheConfigCommandInAnyCase_ThenTheCommandShouldBeConfig(
            [Values("config", "CONFIG", "cOnFiG", "Config")] string command)
        {
            Assert.That(_parser.GetOptions(command), Is.TypeOf(typeof(ConfigOptions)));
        }
    }
}