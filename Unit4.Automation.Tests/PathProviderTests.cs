using NUnit.Framework;
using Unit4.Automation;
using Unit4.Automation.Model;
using System.IO;

namespace Unit4.Automation.Tests
{
    [TestFixture]
    internal class PathProviderTests
    {
        [Test]
        public void GivenBcrOptions_ThenTheFileShouldBeInTheOutputDirectory()
        {
            var outputPath = @"C:\some\path\to\dir";
            var options = new BcrOptions(null, null, null, null, null, outputPath);
            var provider = new PathProvider(options);

            var actualPath = provider.NewPath();

            Assert.That(Path.GetDirectoryName(actualPath), Is.EqualTo(outputPath));
        }
    }
}