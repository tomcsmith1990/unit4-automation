using System;
using NUnit.Framework;

namespace Unit4.Tests
{
    [TestFixture]
    public class SimpleTest
    {
        [Test]
        public void ShouldPass()
        {
            Assert.That(true, Is.True);
        }

        [Test]
        public void ShouldFail()
        {
            Assert.That(true, Is.False);
        }
    }
}