using System;
using System.IO;
using Unit4.Automation.Interfaces;
using Unit4.Automation.Model;

namespace Unit4.Automation
{
    internal class PathProvider : IPathProvider
    {
        private readonly string _outputDir;

        public PathProvider(BcrOptions options)
        {
            _outputDir = string.IsNullOrEmpty(options.OutputDirectory) ? Directory.GetCurrentDirectory() : options.OutputDirectory;
        }

        public string NewPath()
        {
            var filename = string.Format("{0}_{1}.xlsx", DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss"), Guid.NewGuid().ToString("N").Substring(0, 4));
            return Path.Combine(_outputDir, filename);
        }
    }
}