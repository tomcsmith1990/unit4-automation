using System;
using System.IO;
using Unit4.Automation.Interfaces;
using Unit4.Automation.Model;

namespace Unit4.Automation
{
    internal class PathProvider : IPathProvider
    {
        public PathProvider(BcrOptions options)
        {

        }

        public string NewPath()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "output", string.Format("{0}.xlsx", Guid.NewGuid().ToString("N")));
        }
    }
}