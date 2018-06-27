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

            var loadedOptions = GetPersistedOptions(options);

            Assert.That(loadedOptions, Is.EqualTo(options));
        }

        [Test]
        public void GivenPriorOptionsAndNewClient_ThenTheUnchangedOptionsShouldBePersisted()
        {
            new ConfigOptions(1234, "http://test.url").Save();

            var loadedOptions = GetPersistedOptions(new ConfigOptions(9999));

            Assert.That(loadedOptions, Is.EqualTo(new ConfigOptions(9999, "http://test.url")));
        }

        [Test]
        public void GivenPriorOptionsAndNewUrl_ThenTheUnchangedOptionsShouldBePersisted()
        {
            new ConfigOptions(1234, "http://test.url").Save();
            
            var loadedOptions = GetPersistedOptions(new ConfigOptions(url: "http://some/other/test.url"));
            Assert.That(loadedOptions, Is.EqualTo(new ConfigOptions(1234, "http://some/other/test.url")));
        }

        [Test]
        public void GivenPriorOptionsAndAllNewOptions_ThenTheNewOptionsShouldBePersisted()
        {
            new ConfigOptions(1234, "http://test.url").Save();

            var loadedOptions = GetPersistedOptions(new ConfigOptions(9999, "http://some/other/test.url"));

            Assert.That(loadedOptions, Is.EqualTo(new ConfigOptions(9999, "http://some/other/test.url")));
        }

        private ConfigOptions GetPersistedOptions(ConfigOptions optionsToSave)
        {
            new ConfigRunner(optionsToSave).Run();
            return ConfigOptions.Load();
        }
    }
}