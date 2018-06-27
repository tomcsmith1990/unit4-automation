using Unit4.Automation.Model;
using Unit4.Automation.Interfaces;
using NUnit.Framework;
using Moq;
using System.IO;
using System.Linq;
using Unit4.Automation.Commands.BcrCommand;

namespace Unit4.Automation.Tests
{
    [TestFixture]
    internal class BcrReportRunnerTests
    {
        [Test]
        public void GivenBcrFromReader_ThenThatBcrShouldBePassedToTheMiddleware()
        {
            var bcr = new Bcr(Enumerable.Empty<BcrLine>());

            var reader = new Mock<IBcrReader>();
            reader.Setup(x => x.Read()).Returns(bcr);

            var middleware = new Mock<IBcrMiddleware>();

            var runner = Create(reader: reader.Object, middleware: middleware.Object);

            runner.Run();

            middleware.Verify(x => x.Use(bcr), Times.Once);
        }

        [Test]
        public void GivenBcrFromMiddleware_ThenThatBcrShouldBePassedToTheWriter()
        {
            var bcr = new Bcr(Enumerable.Empty<BcrLine>());

            var middleware = new Mock<IBcrMiddleware>();
            middleware.Setup(x => x.Use(It.IsAny<Bcr>())).Returns(bcr);

            var writer = new Mock<IBcrWriter>();
            var runner = Create(middleware: middleware.Object, writer: writer.Object);

            runner.Run();

            writer.Verify(x => x.Write(It.IsAny<string>(), bcr), Times.Once);
        }

        [Test]
        public void GivenPathFromPathProvider_ThenThatPathShouldBePassedToTheWriter()
        {
            var path = @"C:\this\is\a\path.foo";
            var writer = new Mock<IBcrWriter>();
            var pathProvider = new Mock<IPathProvider>();
            pathProvider.Setup(x => x.NewPath()).Returns(path);

            var runner = Create(writer: writer.Object, pathProvider: pathProvider.Object);

            runner.Run();

            writer.Verify(x => x.Write(path, It.IsAny<Bcr>()), Times.Once);
        }

        private BcrReportRunner Create(ILogging log = null, IBcrReader reader = null, IBcrMiddleware middleware = null, IBcrWriter writer = null, IPathProvider pathProvider = null)
        {
            log = log ?? Mock.Of<ILogging>();
            reader = reader ?? Mock.Of<IBcrReader>();
            middleware = middleware ?? Mock.Of<IBcrMiddleware>();
            writer = writer ?? Mock.Of<IBcrWriter>();
            pathProvider = pathProvider ?? Mock.Of<IPathProvider>();

            return new BcrReportRunner(log, reader, middleware, writer, pathProvider, TextWriter.Null);
        }
    }
}