using System;
using Unit4.Automation;
using System.IO;
using NUnit.Framework;
using System.Collections.Generic;
using Unit4.Automation.Model;

namespace Unit4.Automation.Tests
{
    [TestFixture]
    public class ExcelTests
    {
        [Test]
        [Category("RequiresExcelInstall")]
        public void GivenNoRows_ItShouldNotThrow()
        {
            using (var tempFile = new TempFile())
            {
                var excel = new Excel();

                Assert.That(() => excel.WriteToExcel(tempFile.Path, new Bcr() { Lines = new List<BcrLine>() }), Throws.Nothing);
            }
        }

        private class TempFile : IDisposable
        {
            private readonly string m_Path;

            public string Path { get { return m_Path; } }
            
            public TempFile()
            {
                var tempDirectory = System.IO.Path.GetTempPath();

                var fileName = string.Format("{0}.xlsx", Guid.NewGuid().ToString("N"));

                m_Path = System.IO.Path.Combine(tempDirectory, fileName);
            }

            public void Dispose()
            {
                try
                {
                    File.Delete(m_Path);
                }
                catch (Exception)
                {
                }
            }
        }
    }
}