using System;
using NUnit.Framework;
using Unit4.Automation.Model;
using Newtonsoft.Json;

namespace Unit4.Automation.Tests
{
    [TestFixture]
    public class SerializableRoundTripTests
    {
        [Test]
        public void CanSerializeThenDeserializeType()
        {
            var obj = new SerializableCostCentreList() { CostCentres = new CostCentre[0] };

            var json = JsonConvert.SerializeObject(obj);

            Assert.That(() => JsonConvert.DeserializeObject<SerializableCostCentreList>(json), Throws.Nothing);
        }
    }
}