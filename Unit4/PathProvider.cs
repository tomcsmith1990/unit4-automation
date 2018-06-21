using System;
using System.IO;

namespace Unit4.Automation
{
    internal class PathProvider
    {
        public string NewPath()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "output", string.Format("{0}.xlsx", Guid.NewGuid().ToString("N")));
        }
    }
}