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
    internal class BcrReaderTests
    {
        [Test]
        public void GivenNoCache_ThenItShouldFetchAllTier3Hierarchies()
        {
            var engine = new Mock<IUnit4Engine>();

            var factory = new Mock<IUnit4EngineFactory>();
            factory.Setup(x => x.Create()).Returns(engine.Object);

            var costCentresProvider = new Mock<ICostCentresProvider>();
            var costCentres = new SerializableCostCentreList() { CostCentres = new [] { new CostCentre() { Tier3 = "tier3" } } };
            costCentresProvider.Setup(x => x.GetCostCentres()).Returns(costCentres);

            var reader = 
                new BcrReader(
                    Mock.Of<ILogging>(),
                    new BcrOptions(),
                    new ProgramConfig(() => 0, () => string.Empty),
                    Mock.Of<IFile<Bcr>>(),
                    Mock.Of<IFile<SerializableCostCentreList>>(),
                    factory.Object,
                    costCentresProvider.Object);

            reader.Read();

            engine.Verify(x => x.RunReport(Resql.BcrTier3("tier3")), Times.Once);
        }
    }
}