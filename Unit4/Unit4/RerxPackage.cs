using System;
using System.IO;

namespace Unit4
{
    internal class RerxPackage
    {
        private readonly string m_InputFile;
        private readonly string m_OutputFile;

        public RerxPackage(string inputFileName, string outputFileName)
        {
            m_InputFile = inputFileName;
            m_OutputFile = outputFileName;
        }

        public string InputPath
        {
            get
            {
                return Path.Combine(Directory.GetCurrentDirectory(), m_InputFile);
            }
        }

        public string OutputPath
        {
            get
            {
                return Path.Combine(Directory.GetCurrentDirectory(), m_OutputFile);
            }
        }
    }
}