using NUnit.Framework;
using Unit4.Automation;
using Unit4.Automation.Interfaces;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System;
using Unit4.Automation.Model;

namespace Unit4.Automation.Tests
{
    [TestFixture]
    internal class OptionsProviderTests
    {
        [SetUp]
        public void Setup()
        {
            _options = null;
        }

        [Test]
        public void GivenAnExportOfOptionsInAssembly_ThenItShouldBeLoaded()
        {
            var provider = new OptionsProvider(GetType().Assembly);

            Assert.That(provider.Types, Is.EqualTo(new[] { typeof(FakeOptions) }));
        }

        [TestCase(typeof(BcrOptions))]
        [TestCase(typeof(ConfigOptions))]
        public void GivenAnImportOfOptions_ThenWeShouldLoadTheType(Type type)
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new TypeCatalog(new[] { type }));
            var container = new CompositionContainer(catalog);

            container.ComposeParts(this);

            Assert.That(_options, Is.TypeOf(type));
        }

        [Import(typeof(IOptions))] private IOptions _options;

        [Export(typeof(IOptions))]
        private class FakeOptions : IOptions
        {
        }
    }
}