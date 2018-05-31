using System;
using System.Collections.Generic;
using System.Linq;
using Unit4.Automation.Interfaces;
using CommandLine;

namespace Unit4.Automation.Model
{
    [Verb("bcr", HelpText = "Produce a BCR.")]
    internal class BcrOptions : IOptions
    {
        public BcrOptions() : this(null)
        {}

        public BcrOptions(string tier2)
        {

        }

        [Option(HelpText = "Filter by a tier 2 code.")]
        public string Tier2 { get { throw new NotImplementedException(); } }
    }
}