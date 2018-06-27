using System;
using Unit4.Automation.Model;
using Unit4.Automation;
using NUnit.Framework;
using Unit4.Automation.Tests.Helpers;

namespace Unit4.Automation.Tests
{
    internal class ConfigRunnerTests
    {
        private TempFile _tempFile;
        private ConfigOptionsFile _file;

        [SetUp]
        public void Setup()
        {
            _tempFile = new TempFile("json");
            _file = new ConfigOptionsFile(_tempFile.Path);
        }

        [TearDown]
        public void TearDown()
        {
            _tempFile.Dispose();
        }

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
            _file.Save(new ConfigOptions(1234, "http://test.url"));

            var loadedOptions = GetPersistedOptions(new ConfigOptions(9999));

            Assert.That(loadedOptions, Is.EqualTo(new ConfigOptions(9999, "http://test.url")));
        }

        [Test]
        public void GivenPriorOptionsAndNewUrl_ThenTheUnchangedOptionsShouldBePersisted()
        {
            _file.Save(new ConfigOptions(1234, "http://test.url"));
            
            var loadedOptions = GetPersistedOptions(new ConfigOptions(url: "http://some/other/test.url"));
            Assert.That(loadedOptions, Is.EqualTo(new ConfigOptions(1234, "http://some/other/test.url")));
        }

        [Test]
        public void GivenPriorOptionsAndAllNewOptions_ThenTheNewOptionsShouldBePersisted()
        {
            _file.Save(new ConfigOptions(1234, "http://test.url"));

            var loadedOptions = GetPersistedOptions(new ConfigOptions(9999, "http://some/other/test.url"));

            Assert.That(loadedOptions, Is.EqualTo(new ConfigOptions(9999, "http://some/other/test.url")));
        }

        private ConfigOptions GetPersistedOptions(ConfigOptions optionsToSave)
        {
            new ConfigRunner(optionsToSave, _file).Run();
            return _file.Load();
        }
    }
}