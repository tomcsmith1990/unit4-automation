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
            var mockEngine = Mock.Of<IUnit4Engine>();
            var bcrReport = new BcrReport(new DummyEngineFactory(mockEngine), new NullLogging());

            var hierarchy = new List<CostCentre>() { new CostCentre { Tier3 = "A" } }.GroupBy(x => x.Tier3, x => x).Single();

            ReturnEmptyDataSet(Mock.Get(mockEngine), string.Format(Resql.BcrByTier3, hierarchy.Single().Tier4));

            bcrReport.RunBCR(hierarchy);

            Mock.Get(mockEngine).Verify(x => x.RunReport(string.Format(Resql.BcrByTier3, hierarchy.Single().Tier3)), Times.Once);
        }

        [Test]
        public void GivenTier3ThatFails_ThenTheReportShouldBeRanForTheTier4()
        {
            var hierarchy = new List<CostCentre>() { new CostCentre { Tier3 = "A", Tier4 = "B" } }.GroupBy(x => x.Tier3, x => x).Single();

            var mockEngine = Mock.Of<IUnit4Engine>();
            Mock.Get(mockEngine).Setup(x => x.RunReport(string.Format(Resql.BcrByTier3, hierarchy.Single().Tier3))).Throws(new Exception());
            ReturnEmptyDataSet(Mock.Get(mockEngine), string.Format(Resql.BcrByTier4, hierarchy.Single().Tier4));

            var bcrReport = new BcrReport(new DummyEngineFactory(mockEngine), new NullLogging());

            bcrReport.RunBCR(hierarchy);

            Mock.Get(mockEngine).Verify(x => x.RunReport(string.Format(Resql.BcrByTier4, hierarchy.Single().Tier4)), Times.Once);
        }

        [Test]
        public void GivenTier3ThatFails_ThenTheReportShouldBeRanForEachTier4()
        {
            var hierarchy = new List<CostCentre>() { new CostCentre { Tier3 = "A", Tier4 = "B" }, new CostCentre { Tier3 = "A", Tier4 = "C" }  }.GroupBy(x => x.Tier3, x => x).Single();

            var mockEngine = Mock.Of<IUnit4Engine>();
            Mock.Get(mockEngine).Setup(x => x.RunReport(string.Format(Resql.BcrByTier3, hierarchy.Key))).Throws(new Exception());
            
            hierarchy.ToList().ForEach(c => {
                ReturnEmptyDataSet(Mock.Get(mockEngine), string.Format(Resql.BcrByTier4, c.Tier4));
            });
            

            var bcrReport = new BcrReport(new DummyEngineFactory(mockEngine), new NullLogging());

            bcrReport.RunBCR(hierarchy);

            hierarchy.ToList().ForEach(c => {
                Mock.Get(mockEngine).Verify(x => x.RunReport(string.Format(Resql.BcrByTier4, c.Tier4)), Times.Once);
            });            
        }

        private void ReturnEmptyDataSet(Mock<IUnit4Engine> mock, string query)
        {
                mock.Setup(x => x.RunReport(query)).Returns(EmptyDataSet());
        }

        private DataSet EmptyDataSet()
        {
            var dataset = new DataSet();
            dataset.Tables.Add("foo");
            return dataset;
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