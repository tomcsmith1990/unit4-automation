using Unit4.Automation.Model;
using Unit4.Automation.Interfaces;
using NUnit.Framework;
using Moq;
using System.IO;
using System.Linq;
using Unit4.Automation.Commands.BcrCommand;
using System.Collections.Generic;

namespace Unit4.Automation.Tests
{
    [TestFixture]
    internal class BcrReaderTests
    {
        [Test]
        public void GivenNoCache_ThenItShouldFetchAllTier3Hierarchies()
        {
            var engine = new Mock<IUnit4Engine>();
            var reader = CreateReader(new [] { new CostCentre() { Tier3 = "tier3" } }, new BcrOptions(), Mock.Of<IFile<Bcr>>(), engine.Object);

            reader.Read();

            engine.Verify(x => x.RunReport(Resql.BcrTier3("tier3")), Times.Once);
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
    }
}