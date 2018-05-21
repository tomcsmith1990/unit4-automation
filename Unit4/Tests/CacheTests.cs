using System;
using NUnit.Framework;
using Unit4.Automation;
using Unit4.Automation.Interfaces;
using Moq;

namespace Unit4.Automation.Tests
{
    [TestFixture]
    public class CacheTests
    {
        [Test]
        public void GivenDirtyFile_ThenItShouldRunTheFunction()
        {
            var obj = new Foo();
            var mock = new Mock<IFile<Foo>>();
            mock.Setup(x => x.Exists()).Returns(true);
            mock.Setup(x => x.IsDirty()).Returns(true);

            var cache = new Cache<Foo>(() => obj.Blah(), mock.Object);

            cache.Fetch();

            Assert.That(obj.WasCalled, Is.True);
        }

        [Test]
        public void GivenNonDirtyFile_ThenItShouldNotRunTheFunction()
        {
            var obj = new Foo();
            var mock = new Mock<IFile<Foo>>();
            mock.Setup(x => x.Exists()).Returns(true);
            mock.Setup(x => x.IsDirty()).Returns(false);

            var cache = new Cache<Foo>(() => obj.Blah(), mock.Object);

            cache.Fetch();

            Assert.That(obj.WasCalled, Is.False);
        }

        public class Foo
        {
            public Foo Blah()
            {
                WasCalled = true;
                return this;
            }

            public bool WasCalled { get; private set; }
        }
    }
}