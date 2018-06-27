using System;
using System.IO;

namespace Unit4.Automation.Tests.Helpers
{
    internal class TempFile : IDisposable
    {
        private readonly string _path;

        public string Path { get { return _path; } }
        
        public TempFile(string extension)
        {
            var tempDirectory = System.IO.Path.GetTempPath();

            var fileName = string.Format("{0}.{1}", Guid.NewGuid().ToString("N"), extension);

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