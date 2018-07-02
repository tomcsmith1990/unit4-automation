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
            var engine = new Mock<IUnit4Engine>();
            engine.Setup(x => x.RunReport(Resql.BcrTier3("tier3"))).Returns(CreateDataSet(new CostCentre() { Tier3 = "tier3" }));
            var reader = CreateReader(new [] { new CostCentre() { Tier3 = "tier3" } }, new BcrOptions(), Mock.Of<IFile<Bcr>>(), engine.Object);

            reader.Read();

            engine.Verify(x => x.RunReport(Resql.BcrTier3("tier3")), Times.Once);
        }

        [Test]
        public void GivenTier3WithAllCostCentresCached_ThenItShouldNotFetchThatTier3()
        {
            var engine = new Mock<IUnit4Engine>();

            var bcrCache = new Mock<IFile<Bcr>>();
            bcrCache.Setup(x => x.Exists()).Returns(true);
            bcrCache.Setup(x => x.IsDirty()).Returns(false);
            bcrCache.Setup(x => x.Read()).Returns(new Bcr(new BcrLine[] { A.BcrLine().With(A.Criteria.Tier3, "tier3").Build() }));

            var reader = CreateReader(new [] { new CostCentre() { Tier3 = "tier3" } }, new BcrOptions(), bcrCache.Object, engine.Object);

            reader.Read();

            engine.Verify(x => x.RunReport(Resql.BcrTier3("tier3")), Times.Never);
        }

        [Test]
        public void GivenTier3WithOneCostCentresCached_ThenItShouldOnlyFetchTheUncachedTier3()
        {
            var engine = new Mock<IUnit4Engine>();
            engine.Setup(x => x.RunReport(Resql.BcrTier3("b"))).Returns(CreateDataSet(new CostCentre() { Tier3 = "b", Code = "b" }));

            var bcrCache = new Mock<IFile<Bcr>>();
            bcrCache.Setup(x => x.Exists()).Returns(true);
            bcrCache.Setup(x => x.IsDirty()).Returns(false);
            bcrCache.Setup(x => x.Read()).Returns(new Bcr(new BcrLine[] { A.BcrLine().With(A.Criteria.Tier3, "a").With(A.Criteria.CostCentre, "a").Build() }));

            var reader = CreateReader(new [] { new CostCentre() { Tier3 = "a", Code = "a" }, new CostCentre() { Tier3 = "b", Code = "b" } }, new BcrOptions(), bcrCache.Object, engine.Object);

            var tier3sInBcr = reader.Read().Lines.Select(x => x.CostCentre.Tier3);

            engine.Verify(x => x.RunReport(Resql.BcrTier3("b")), Times.Once);
            engine.Verify(x => x.RunReport(Resql.BcrTier3("a")), Times.Never);

            Assert.That(tier3sInBcr, Is.EquivalentTo(new [] { "a", "b" }));
        }

        [Test]
        public void GivenCachedCostCentreInOneTier3AndUncachedCostCentreInSameTier3_ThenItShouldFetchTheTier3()
        {
            var engine = new Mock<IUnit4Engine>();
            engine.Setup(x => x.RunReport(Resql.BcrTier3("tier3"))).Returns(CreateDataSet(new CostCentre() { Tier3 = "tier3", Code = "a" }, new CostCentre() { Tier3 = "tier3", Code = "b" }));

            var bcrCache = new Mock<IFile<Bcr>>();
            bcrCache.Setup(x => x.Exists()).Returns(true);
            bcrCache.Setup(x => x.IsDirty()).Returns(false);
            bcrCache.Setup(x => x.Read()).Returns(new Bcr(new BcrLine[] { A.BcrLine().With(A.Criteria.Tier3, "tier3").With(A.Criteria.CostCentre, "a").Build() }));

            var reader = CreateReader(new [] { new CostCentre() { Tier3 = "tier3", Code = "a" }, new CostCentre() { Tier3 = "tier3", Code = "b" } }, new BcrOptions(), bcrCache.Object, engine.Object);

            var costCentresInBcr = reader.Read().Lines.Select(x => x.CostCentre.Code);

            engine.Verify(x => x.RunReport(Resql.BcrTier3("tier3")), Times.Once);

            Assert.That(costCentresInBcr, Is.EquivalentTo(new [] { "a", "b" }));
        }

        private BcrReader CreateReader(IEnumerable<CostCentre> allCostCentres, BcrOptions options, IFile<Bcr> bcrCache, IUnit4Engine engine)
        {
            var factory = new Mock<IUnit4EngineFactory>();
            factory.Setup(x => x.Create()).Returns(engine);

            var costCentresProvider = new Mock<ICostCentresProvider>();
            var costCentres = new SerializableCostCentreList() { CostCentres = allCostCentres };
            costCentresProvider.Setup(x => x.GetCostCentres()).Returns(costCentres);

            return new BcrReader(
                Mock.Of<ILogging>(),
                options,
                new ProgramConfig(() => 0, () => string.Empty),
                bcrCache,
                Mock.Of<IFile<SerializableCostCentreList>>(),
                factory.Object,
                costCentresProvider.Object);
        }

        private DataSet CreateDataSet(params CostCentre[] costCentres)
        {
            var dataset = new DataSet();
            var table = dataset.Tables.Add("foo");
            table.Columns.Add("r0r0r0r3dim2", typeof(string));
            table.Columns.Add("r0r0r3dim2", typeof(string));
            table.Columns.Add("r0r3dim2", typeof(string));
            table.Columns.Add("r3dim2", typeof(string));
            table.Columns.Add("dim2", typeof(string));
            table.Columns.Add("xr0r0r0r3dim2", typeof(string));
            table.Columns.Add("xr0r0r3dim2", typeof(string));
            table.Columns.Add("xr0r3dim2", typeof(string));
            table.Columns.Add("xr3dim2", typeof(string));
            table.Columns.Add("xdim2", typeof(string));
            table.Columns.Add("dim1", typeof(string));
            table.Columns.Add("xdim1", typeof(string));
            table.Columns.Add("plb_amount", typeof(double));
            table.Columns.Add("f0_budget_to_da13", typeof(double));
            table.Columns.Add("f1_total_exp_to16", typeof(double));
            table.Columns.Add("f3_variance_to_15", typeof(double));
            table.Columns.Add("plf_amount", typeof(double));
            table.Columns.Add("f2_outturn_vari18", typeof(double));

            foreach (var code in costCentres)
            {
                var row = table.NewRow();
                table.Rows.Add(string.Empty, string.Empty, code.Tier3, string.Empty, code.Code, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 0, 0, 0, 0, 0, 0);
            }
            return dataset;
        }
    }
}