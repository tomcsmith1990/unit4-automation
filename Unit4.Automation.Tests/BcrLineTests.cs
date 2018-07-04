using NUnit.Framework;
using Unit4.Automation.Model;

namespace Unit4.Automation.Tests
{
    [TestFixture]
    internal class BcrLineTests
    {
        [Test]
        public void GivenNullValue_ThenTheObjectShouldBeGreater()
        {
            var line = new BcrLine();
            var result = line.CompareTo(null) > 0;

            Assert.That(result, Is.True);
        }
    }
}