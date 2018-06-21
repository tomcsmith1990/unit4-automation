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
            return Path.Combine(_outputDir, string.Format("{0}.xlsx", Guid.NewGuid().ToString("N")));
        }
    }
}