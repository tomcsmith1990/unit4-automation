using System;
using System.IO;
using Unit4.Automation.Interfaces;

namespace Unit4.Automation
{
    internal class PathProvider : IPathProvider
    {
        public string NewPath()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "output", string.Format("{0}.xlsx", Guid.NewGuid().ToString("N")));
        }
    }
}