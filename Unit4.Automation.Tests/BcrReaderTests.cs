using Unit4.Automation.Model;
using Unit4.Automation.Interfaces;
using NUnit.Framework;
using Moq;
using System.IO;
using System.Linq;
using Unit4.Automation.Commands.BcrCommand;
using System.Collections.Generic;
using Unit4.Automation.Tests.Helpers;
using System.Data;

namespace Unit4.Automation.Tests
{
    [TestFixture]
    internal class BcrReaderTests
    {
        [Test]
        public void GivenNoCache_ThenItShouldFetchAllTier3Hierarchies()
        {
            var lines = new BcrLine[] { A.BcrLine().With(A.Criteria.Tier3, "tier3") };
            var engine = new Mock<IUnit4Engine>();
            engine.Setup(x => x.RunReport(Resql.BcrTier3("tier3"))).Returns(lines.AsDataSet());
            var reader = CreateReader(new [] { new CostCentre() { Tier3 = "tier3" } }, Mock.Of<IFile<Bcr>>(), engine.Object);

            reader.Read();

            engine.Verify(x => x.RunReport(Resql.BcrTier3("tier3")), Times.Once);
        }

        [Test]
        public void GivenTier3WithAllCostCentresCached_ThenItShouldNotFetchThatTier3()
        {
            var engine = new Mock<IUnit4Engine>();

            var cache = CreateCache(A.BcrLine().With(A.Criteria.Tier3, "tier3"));

            var reader = CreateReader(new [] { new CostCentre() { Tier3 = "tier3" } }, cache, engine.Object);

            reader.Read();

            engine.Verify(x => x.RunReport(Resql.BcrTier3("tier3")), Times.Never);
        }

        [Test]
        public void GivenTier3WithOneCostCentresCached_ThenItShouldOnlyFetchTheUncachedTier3()
        {
            var lines = new BcrLine[] { A.BcrLine().With(A.Criteria.Tier3, "b").With(A.Criteria.CostCentre, "b") };
            var engine = new Mock<IUnit4Engine>();
            engine.Setup(x => x.RunReport(Resql.BcrTier3("b"))).Returns(lines.AsDataSet());

            var cache = CreateCache(A.BcrLine().With(A.Criteria.Tier3, "a").With(A.Criteria.CostCentre, "a"));

            var reader = CreateReader(new [] { new CostCentre() { Tier3 = "a", Code = "a" }, new CostCentre() { Tier3 = "b", Code = "b" } }, cache, engine.Object);

            var tier3sInBcr = reader.Read().Lines.Select(x => x.CostCentre.Tier3);

            engine.Verify(x => x.RunReport(Resql.BcrTier3("b")), Times.Once);
            engine.Verify(x => x.RunReport(Resql.BcrTier3("a")), Times.Never);

            Assert.That(tier3sInBcr, Is.EquivalentTo(new [] { "a", "b" }));
        }

        [Test]
        public void GivenCachedCostCentreInOneTier3AndUncachedCostCentreInSameTier3_ThenItShouldFetchTheTier3()
        {
            var lines = new BcrLine[] { 
                A.BcrLine().With(A.Criteria.Tier3, "tier3").With(A.Criteria.CostCentre, "a"), 
                A.BcrLine().With(A.Criteria.Tier3, "tier3").With(A.Criteria.CostCentre, "b")
            };
            var engine = new Mock<IUnit4Engine>();
            engine.Setup(x => x.RunReport(Resql.BcrTier3("tier3"))).Returns(lines.AsDataSet());

            var cache = CreateCache(A.BcrLine().With(A.Criteria.Tier3, "tier3").With(A.Criteria.CostCentre, "a"));

            var reader = CreateReader(new [] { new CostCentre() { Tier3 = "tier3", Code = "a" }, new CostCentre() { Tier3 = "tier3", Code = "b" } }, cache, engine.Object);

            var costCentresInBcr = reader.Read().Lines.Select(x => x.CostCentre.Code);

            engine.Verify(x => x.RunReport(Resql.BcrTier3("tier3")), Times.Once);

            Assert.That(costCentresInBcr, Is.EquivalentTo(new [] { "a", "b" }));
        }

        private IFile<Bcr> CreateCache(params BcrLine[] lines)
        {
            var cache = new Mock<IFile<Bcr>>();
            cache.Setup(x => x.Exists()).Returns(true);
            cache.Setup(x => x.IsDirty()).Returns(false);
            cache.Setup(x => x.Read()).Returns(new Bcr(lines));
            return cache.Object;
        }

        private BcrReader CreateReader(IEnumerable<CostCentre> allCostCentres, IFile<Bcr> bcrCache, IUnit4Engine engine)
        {
            var factory = new Mock<IUnit4EngineFactory>();
            factory.Setup(x => x.Create()).Returns(engine);

            var costCentresProvider = new Mock<ICostCentresProvider>();
            var costCentres = new SerializableCostCentreList() { CostCentres = allCostCentres };
            costCentresProvider.Setup(x => x.GetCostCentres()).Returns(costCentres);

            return new BcrReader(
                Mock.Of<ILogging>(),
                new BcrOptions(),
                new ProgramConfig(() => 0, () => string.Empty),
                bcrCache,
                Mock.Of<IFile<SerializableCostCentreList>>(),
                factory.Object,
                costCentresProvider.Object);
        }
    }
}