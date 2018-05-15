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
        public void GivenOneTier3_ThenTheReportShouldBeRanForThatTier3()
        {
            var hierarchy = new List<CostCentre>() { new CostCentre { Tier3 = "A" } }.GroupBy(x => x.Tier3, x => x).Single();

            var engineFactory = 
                new DummyEngineFactory(
                    new string[] { Resql.Bcr(tier3: hierarchy.Single().Tier3) }
                );

            var bcrReport = new BcrReport(engineFactory, new NullLogging());

            bcrReport.RunBCR(hierarchy);

            engineFactory.Mock.Verify(x => x.RunReport(Resql.BcrTier3(hierarchy.Key)), Times.Once);
        }

        [Test]
        public void GivenTier3ThatFails_ThenTheReportShouldBeRanForTheTier4()
        {
            var hierarchy = new List<CostCentre>() { new CostCentre { Tier3 = "A", Tier4 = "B" } }.GroupBy(x => x.Tier3, x => x).Single();

            var engineFactory =
                new DummyEngineFactory(
                    returnEmpty: new string[] { Resql.Bcr(tier4: hierarchy.Single().Tier4) },
                    throws: new string[] { Resql.Bcr(tier3: hierarchy.Key) }
                );

            var bcrReport = new BcrReport(engineFactory, new NullLogging());

            bcrReport.RunBCR(hierarchy);

            engineFactory.Mock.Verify(x => x.RunReport(Resql.BcrTier4(hierarchy.Single().Tier4)), Times.Once);
        }

        [Test]
        public void GivenTier3ThatFails_ThenTheReportShouldBeRanForEachTier4()
        {
            var hierarchy = new List<CostCentre>() { new CostCentre { Tier3 = "A", Tier4 = "B" }, new CostCentre { Tier3 = "A", Tier4 = "C" }  }.GroupBy(x => x.Tier3, x => x).Single();

            var engineFactory =
                new DummyEngineFactory(
                    returnEmpty: hierarchy.Select(x => Resql.Bcr(tier4: x.Tier4)),
                    throws: new string[] { Resql.Bcr(tier3: hierarchy.Key) }
                );

            var bcrReport = new BcrReport(engineFactory, new NullLogging());

            bcrReport.RunBCR(hierarchy);

            hierarchy.ToList().ForEach(c => {
                engineFactory.Mock.Verify(x => x.RunReport(Resql.BcrTier4(c.Tier4)), Times.Once);
            });            
        }

        private class DummyEngineFactory : IUnit4EngineFactory
        {
            private readonly Mock<IUnit4Engine> _mock;

            public Mock<IUnit4Engine> Mock { get { return _mock; } }

            public DummyEngineFactory(IEnumerable<string> returnEmpty, IEnumerable<string> throws = null)
            {
                throws = throws ?? new string[0];

                var mockEngine = new Mock<IUnit4Engine>();
                returnEmpty.ToList().ForEach(x => mockEngine.Setup(y => y.RunReport(x)).Returns(EmptyDataSet()));
                throws.ToList().ForEach(x => mockEngine.Setup(y => y.RunReport(x)).Throws(new Exception()));
                _mock = mockEngine;
            }

            private DataSet EmptyDataSet()
            {
                var dataset = new DataSet();
                dataset.Tables.Add("foo");
                return dataset;
            }

            public IUnit4Engine Create()
            {
                return _mock.Object;
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