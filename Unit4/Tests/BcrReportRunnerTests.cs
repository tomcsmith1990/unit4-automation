using System;
using Unit4.Automation.Model;
using Unit4.Automation.Interfaces;
using NUnit.Framework;
using Moq;

namespace Unit4.Automation.Tests
{
    [TestFixture]
    public class BcrReportRunnerTests
    {
        [Test]
        public void GivenBcrFromReader_ThenThatBcrShouldBePassedToTheMiddleware()
        {
            var log = Mock.Of<ILogging>();
            var writer = Mock.Of<IBcrWriter>();

            var bcr = new Bcr();

            var reader = new Mock<IBcrReader>();
            reader.Setup(x => x.Read()).Returns(bcr);

            var middleware = new Mock<IBcrMiddleware>();

            var runner = new BcrReportRunner(log, reader.Object, middleware.Object, writer);

            runner.Run();

            middleware.Verify(x => x.Use(bcr), Times.Once);
        }
    }
}