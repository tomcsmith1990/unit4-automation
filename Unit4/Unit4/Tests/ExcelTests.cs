using System;
using Unit4;
using System.IO;
using NUnit.Framework;
using System.Collections.Generic;

namespace Unit4.Tests
{
    [TestFixture]
    public class ExcelTests
    {
        [Test]
        public void GivenNoRows_ItShouldNotThrow()
        {
            using (var tempFile = new TempFile())
            {
                var excel = new Excel();

                Assert.That(() => excel.WriteToExcel(tempFile.Path, new List<BCRLine>()), Throws.Nothing);
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