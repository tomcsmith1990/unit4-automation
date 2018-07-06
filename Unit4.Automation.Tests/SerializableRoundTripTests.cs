using System.Linq;
using Newtonsoft.Json;
using NUnit.Framework;
using Unit4.Automation.Model;

namespace Unit4.Automation.Tests
{
    [TestFixture]
    internal class SerializableRoundTripTests
    {
        [Test]
        public void CanSerializeThenDeserializeType()
        {
            var obj = new SerializableCostCentreList() { CostCentres = new CostCentre[0] };

            var json = JsonConvert.SerializeObject(obj);

            Assert.That(() => JsonConvert.DeserializeObject<SerializableCostCentreList>(json), Throws.Nothing);
        }

        [Test]
        public void CanSerializeThenDeserializeTypeBcr()
        {
            var obj = new Bcr(Enumerable.Empty<BcrLine>());

            var json = JsonConvert.SerializeObject(obj);

            Assert.That(() => JsonConvert.DeserializeObject<Bcr>(json), Throws.Nothing);
        }
    }
}