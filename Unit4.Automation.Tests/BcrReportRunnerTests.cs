using Unit4.Automation.Model;
using Unit4.Automation.Interfaces;
using NUnit.Framework;
using Moq;
using System.IO;
using System.Linq;

namespace Unit4.Automation.Tests
{
    [TestFixture]
    internal class BcrReportRunnerTests
    {
        [Test]
        public void GivenBcrFromReader_ThenThatBcrShouldBePassedToTheMiddleware()
        {
            var log = Mock.Of<ILogging>();
            var writer = Mock.Of<IBcrWriter>();

            var bcr = new Bcr(Enumerable.Empty<BcrLine>());

            var reader = new Mock<IBcrReader>();
            reader.Setup(x => x.Read()).Returns(bcr);

            var middleware = new Mock<IBcrMiddleware>();

            var runner = new BcrReportRunner(log, reader.Object, middleware.Object, writer, Mock.Of<IPathProvider>(), TextWriter.Null);

            runner.Run();

            middleware.Verify(x => x.Use(bcr), Times.Once);
        }

        [Test]
        public void GivenBcrFromMiddleware_ThenThatBcrShouldBePassedToTheWriter()
        {
            var log = Mock.Of<ILogging>();
            var reader = Mock.Of<IBcrReader>();

            var bcr = new Bcr(Enumerable.Empty<BcrLine>());

            var middleware = new Mock<IBcrMiddleware>();
            middleware.Setup(x => x.Use(It.IsAny<Bcr>())).Returns(bcr);

            var writer = new Mock<IBcrWriter>();

            var runner = new BcrReportRunner(log, reader, middleware.Object, writer.Object, Mock.Of<IPathProvider>(), TextWriter.Null);

            runner.Run();

            writer.Verify(x => x.Write(It.IsAny<string>(), bcr), Times.Once);
        }

        [Test]
        public void GivenPathFromPathProvider_ThenThatPathShouldBePassedToTheWriter()
        {
            var log = Mock.Of<ILogging>();
            var reader = Mock.Of<IBcrReader>();

            var bcr = new Bcr(Enumerable.Empty<BcrLine>());

            var middleware = Mock.Of<IBcrMiddleware>();

            var path = @"C:\this\is\a\path.foo";
            var writer = new Mock<IBcrWriter>();
            var pathProvider = new Mock<IPathProvider>();
            pathProvider.Setup(x => x.NewPath()).Returns(path);

            var runner = new BcrReportRunner(log, reader, middleware, writer.Object, pathProvider.Object, TextWriter.Null);

            runner.Run();

            writer.Verify(x => x.Write(path, It.IsAny<Bcr>()), Times.Once);
        }
    }
}