using NUnit.Framework;
using Unit4.Automation.Model;
using System;

namespace Unit4.Automation.Tests
{
    [TestFixture]
    internal class ConfigOptionsTests
    {
        [Test]
        public void GivenNullUrl_ThenItShouldThrow()
        {
            var options = new ConfigOptions();

            Assert.Throws<ApplicationException>(() => { var x = options.Url; });
        }
    }
}