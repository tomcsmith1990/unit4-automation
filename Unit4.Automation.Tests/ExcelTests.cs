using NUnit.Framework;
using Unit4.Automation.Model;
using System.Linq;
using Unit4.Automation.Tests.Helpers;

namespace Unit4.Automation.Tests
{
    [TestFixture]
    internal class ExcelTests
    {
        [Test]
        [Category("RequiresExcelInstall")]
        public void GivenNoRows_ItShouldNotThrow()
        {
            using (var tempFile = new TempFile("xslx"))
            {
                var excel = new Excel();

                Assert.That(() => excel.Write(tempFile.Path, new Bcr(Enumerable.Empty<BcrLine>())), Throws.Nothing);
            }
        }
    }
}