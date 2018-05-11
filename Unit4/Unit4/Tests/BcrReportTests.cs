using System;
using Unit4;
using NUnit.Framework;
using Unit4.Interfaces;
using System.Data;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace Unit4.Tests
{
    [TestFixture]
    public class BcrReportTests
    {
        [Test]
        public void GivenNoTier4_ThenThereShouldBeNoReport()
        {
            var mockEngine = Mock.Of<IUnit4Engine>();
            var bcrReport = new BcrReport(new DummyEngineFactory(mockEngine), new NullLogging());

            var hierarchy = new List<CostCentre>() { new CostCentre { Tier3 = "A" } }.GroupBy(x => x.Tier3, x => x).Single();
            bcrReport.RunBCR(hierarchy);

            Mock.Get(mockEngine).Verify(x => x.RunReport(string.Format(Resql.BcrByTier3, hierarchy.Single().Tier3)), Times.Once);
        }

        private class DummyEngineFactory : IUnit4EngineFactory
        {
            private readonly IUnit4Engine _engine;

            public DummyEngineFactory(IUnit4Engine engine)
            {
                _engine = engine;
            }

            public IUnit4Engine Create()
            {
                return _engine;
            }
        }

        private class NullLogging : ILogging
        {
            public string Path { get { return string.Empty; } }

            public void Start() {}
            public void Close() {}
            public void Info(string message) {}
            public void Error(string message) {}
            public void Error(Exception exception) {}
        }
    }
}