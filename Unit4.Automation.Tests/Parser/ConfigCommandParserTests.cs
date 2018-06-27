using NUnit.Framework;
using System.IO;
using Unit4.Automation.Model;
using Unit4.Automation.Commands;

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

        [Test]
        public void GivenTheConfigCommand_ThenTheClientOptionShouldBeRecognised()
        {
            var options = _parser.GetOptions("config", "--client=1234") as ConfigOptions;

            Assert.That(options.Client, Is.EqualTo(1234));
        }

        [Test]
        public void GivenTheConfigCommand_ThenTheUrlOptionShouldBeRecognised()
        {
            var options = _parser.GetOptions("config", "--url=http://foo.bar/Service/x.asmx") as ConfigOptions;

            Assert.That(options.Url, Is.EqualTo("http://foo.bar/Service/x.asmx"));
        }
    }
}