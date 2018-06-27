using System;
using Unit4.Automation.Model;
using Unit4.Automation;
using NUnit.Framework;

namespace Unit4.Automation.Tests
{
    internal class ConfigRunnerTests
    {
        [Test]
        public void GivenNoPriorConfigOptions_ThenTheNewOptionsShouldBeSaved()
        {
            var options = new ConfigOptions(1234, "http://test.url");
            var runner = new ConfigRunner(options);

            runner.Run();

            var loadedOptions = ConfigOptions.Load();

            Assert.That(loadedOptions, Is.EqualTo(options));
        }

        [Test]
        public void GivenPriorOptionsAndNewClient_ThenTheUnchangedOptionsShouldBePersisted()
        {
            var priorOptions = new ConfigOptions(1234, "http://test.url");
            priorOptions.Save();

            var options = new ConfigOptions(9999);

            var runner = new ConfigRunner(options);

            runner.Run();

            var loadedOptions = ConfigOptions.Load();

            Assert.That(loadedOptions, Is.EqualTo(new ConfigOptions(9999, "http://test.url")));
        }
    }
}