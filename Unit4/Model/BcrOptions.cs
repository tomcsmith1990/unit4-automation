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
        private readonly IEnumerable<string> _tier2;

        public BcrOptions() : this(null)
        {}

        public BcrOptions(IEnumerable<string> tier2)
        {
            _tier2 = tier2;
        }

        [Option(HelpText = "Filter by a tier 2 code.", Separator=',')]
        public IEnumerable<string> Tier2 { get { return _tier2; } }
    }
}