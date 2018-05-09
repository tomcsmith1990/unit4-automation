using System;
using System.IO;

namespace Unit4
{
    internal class RerxPackage
    {
        public string InputPath
        {
            get
            {
                return Path.Combine(Directory.GetCurrentDirectory(), "Vol Orgs BCR.rerx");
            }
        }

        public string OutputPath
        {
            get
            {
                return Path.Combine(Directory.GetCurrentDirectory(), "Vol Orgs BCR.xlsx");
            }
        }
    }
}