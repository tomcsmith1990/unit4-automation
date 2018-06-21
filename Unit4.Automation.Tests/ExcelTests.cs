using System;
using System.IO;
using NUnit.Framework;
using Unit4.Automation.Model;
using System.Linq;

namespace Unit4.Automation.Tests
{
    [TestFixture]
    internal class ExcelTests
    {
        [Test]
        [Category("RequiresExcelInstall")]
        public void GivenNoRows_ItShouldNotThrow()
        {
            using (var tempFile = new TempFile())
            {
                var excel = new Excel();

                Assert.That(() => excel.Write(tempFile.Path, new Bcr(Enumerable.Empty<BcrLine>())), Throws.Nothing);
            }
        }

        private class TempFile : IDisposable
        {
            private readonly string _path;

            public string Path { get { return _path; } }
            
            public TempFile()
            {
                var tempDirectory = System.IO.Path.GetTempPath();

                var fileName = string.Format("{0}.xlsx", Guid.NewGuid().ToString("N"));

                _path = System.IO.Path.Combine(tempDirectory, fileName);
            }

            public void Dispose()
            {
                try
                {
                    File.Delete(_path);
                }
                catch (Exception)
                {
                }
            }
        }
    }
}