using System.IO;
using NUnit.Framework;
using Unit4.Automation.Commands;
using Unit4.Automation.Model;

namespace Unit4.Automation.Tests.Parser
{
    [TestFixture]
    internal class ConfigCommandParserTests
    {
        [SetUp]
        public void Setup()
        {
            _parser = new CommandParser<BcrOptions, ConfigOptions>(TextWriter.Null);
        }

        private CommandParser<BcrOptions, ConfigOptions> _parser;

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

        [Test]
        public void GivenTheConfigCommandInAnyCase_ThenTheCommandShouldBeConfig(
            [Values("config", "CONFIG", "cOnFiG", "Config")]
            string command)
        {
            Assert.That(_parser.GetOptions(command), Is.TypeOf(typeof(ConfigOptions)));
        }
    }
}