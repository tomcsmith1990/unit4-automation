using NUnit.Framework;
using Unit4.Automation;
using Unit4.Automation.Interfaces;
using System.ComponentModel;
using System.ComponentModel.Composition;

namespace Unit4.Automation.Tests
{
    [TestFixture]
    internal class OptionsProviderTests
    {
        [Test]
        public void GivenAnExportOfOptionsInAssembly_ThenItShouldBeLoaded()
        {
            var provider = new OptionsProvider(GetType().Assembly);

            Assert.That(provider.Types, Is.EqualTo(new[] { typeof(FakeOptions) }));
        }

        [Export(typeof(IOptions))]
        private class FakeOptions : IOptions
        {
        }
    }
}