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

            var cache = new Cache<Foo>(() => { throw new NotImplementedException(); }, mock.Object);

            cache.Fetch();

            Assert.That(obj.WasCalled, Is.False);
        }

        [Test]
        public void GivenCallThrough_ThenTheResultShouldBeSaved()
        {
            var obj = new Foo();
            var mock = new Mock<IFile<Foo>>();
            mock.Setup(x => x.Exists()).Returns(false);

            var cache = new Cache<Foo>(() => obj.Blah(), mock.Object);

            cache.Fetch();

            mock.Verify(x => x.Write(obj), Times.Once);
        }

        [Test]
        public void GivenCleanCache_ThenTheResultShouldBeRead()
        {
            var obj = new Foo();

            var mock = new Mock<IFile<Foo>>();
            mock.Setup(x => x.Exists()).Returns(true);
            mock.Setup(x => x.IsDirty()).Returns(false);
            mock.Setup(x => x.Read()).Returns(obj);

            var cache = new Cache<Foo>(() => { throw new NotImplementedException(); }, mock.Object);

            var actual = cache.Fetch();

            Assert.That(actual, Is.EqualTo(obj));
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