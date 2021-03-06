using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Moq;
using NUnit.Framework;
using Unit4.Automation.Commands.BcrCommand;
using Unit4.Automation.Interfaces;
using Unit4.Automation.Model;

namespace Unit4.Automation.Tests
{
    [TestFixture]
    internal class BcrReportTests
    {
        [Test]
        public void GivenOneTier3_ThenTheReportShouldBeRanForThatTier3()
        {
            var hierarchy = new List<CostCentre>() { new CostCentre { Tier3 = "A" } }.GroupBy(x => x.Tier3, x => x);

            var engineFactory =
                new DummyEngineFactory(
                    new[] { Resql.Bcr(tier3: hierarchy.Single().Single().Tier3) }
                );

            var bcrReport = new BcrReport(engineFactory, new NullLogging());

            bcrReport.RunBcr(hierarchy);

            engineFactory.Mock.Verify(x => x.RunReport(Resql.BcrTier3(hierarchy.Single().Key)), Times.Once);
        }

        [Test]
        public void GivenTier3ThatFails_ThenTheReportShouldBeRanForTheTier4()
        {
            var hierarchy =
                new List<CostCentre>() { new CostCentre { Tier3 = "A", Tier4 = "B" } }.GroupBy(x => x.Tier3, x => x);

            var engineFactory =
                new DummyEngineFactory(
                    returnEmpty: new[] { Resql.Bcr(tier4: hierarchy.Single().Single().Tier4) },
                    throws: new[] { Resql.Bcr(tier3: hierarchy.Single().Key) }
                );

            var bcrReport = new BcrReport(engineFactory, new NullLogging());

            bcrReport.RunBcr(hierarchy);

            engineFactory.Mock.Verify(x => x.RunReport(Resql.BcrTier4(hierarchy.Single().Single().Tier4)), Times.Once);
        }

        [Test]
        public void GivenTier3ThatFails_ThenTheReportShouldBeRanForEachTier4()
        {
            var hierarchy =
                new List<CostCentre>()
                {
                    new CostCentre { Tier3 = "A", Tier4 = "B" },
                    new CostCentre { Tier3 = "A", Tier4 = "C" }
                }.GroupBy(x => x.Tier3, x => x);

            var engineFactory =
                new DummyEngineFactory(
                    returnEmpty: hierarchy.Single().Select(x => Resql.Bcr(tier4: x.Tier4)),
                    throws: new[] { Resql.Bcr(tier3: hierarchy.Single().Key) }
                );

            var bcrReport = new BcrReport(engineFactory, new NullLogging());

            bcrReport.RunBcr(hierarchy);

            hierarchy.Single().ToList().ForEach(
                c => { engineFactory.Mock.Verify(x => x.RunReport(Resql.BcrTier4(c.Tier4)), Times.Once); });
        }

        [Test]
        public void GivenTier4ThatFails_ThenTheReportShouldBeRanForTheCostCentre()
        {
            var hierarchy =
                new List<CostCentre>() { new CostCentre { Tier3 = "A", Tier4 = "B", Code = "C" } }.GroupBy(
                    x => x.Tier3,
                    x => x);

            var engineFactory =
                new DummyEngineFactory(
                    returnEmpty: new[] { Resql.Bcr(costCentre: hierarchy.Single().Single().Code) },
                    throws: new[]
                    {
                        Resql.Bcr(tier3: hierarchy.Single().Key), Resql.Bcr(tier4: hierarchy.Single().Single().Tier4)
                    }
                );

            var bcrReport = new BcrReport(engineFactory, new NullLogging());

            bcrReport.RunBcr(hierarchy);

            engineFactory.Mock.Verify(
                x => x.RunReport(Resql.BcrCostCentre(hierarchy.Single().Single().Code)),
                Times.Once);
        }

        private class DummyEngineFactory : IUnit4EngineFactory
        {
            private readonly Mock<IUnit4Engine> _mock;

            public DummyEngineFactory(IEnumerable<string> returnEmpty, IEnumerable<string> throws = null)
            {
                throws = throws ?? new string[0];

                var mockEngine = new Mock<IUnit4Engine>();
                returnEmpty.ToList().ForEach(x => mockEngine.Setup(y => y.RunReport(x)).Returns(EmptyDataSet()));
                throws.ToList().ForEach(x => mockEngine.Setup(y => y.RunReport(x)).Throws(new Exception()));
                _mock = mockEngine;
            }

            public Mock<IUnit4Engine> Mock => _mock;

            public IUnit4Engine Create() => _mock.Object;

            private DataSet EmptyDataSet()
            {
                var dataset = new DataSet();
                dataset.Tables.Add("foo");
                return dataset;
            }
        }

        private class NullLogging : ILogging
        {
            public string Path => string.Empty;

            public void Start()
            {
            }

            public void Close()
            {
            }

            public void Info(string message)
            {
            }

            public void Error(string message)
            {
            }

            public void Error(Exception exception)
            {
            }
        }
    }
}