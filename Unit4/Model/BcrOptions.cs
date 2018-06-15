using System.Collections.Generic;
using System.Linq;
using Unit4.Automation.Interfaces;
using CommandLine;

namespace Unit4.Automation.Model
{
    [Verb("bcr", HelpText = "Produce a BCR.")]
    internal class BcrOptions : IOptions
    {
        private readonly IEnumerable<string> _tier1;
        private readonly IEnumerable<string> _tier2;
        private readonly IEnumerable<string> _tier3;
        private readonly IEnumerable<string> _tier4;

        public BcrOptions() : this(Enumerable.Empty<string>(), Enumerable.Empty<string>(), Enumerable.Empty<string>(), Enumerable.Empty<string>(), Enumerable.Empty<string>())
        {}

        public BcrOptions(IEnumerable<string> tier1, IEnumerable<string> tier2, IEnumerable<string> tier3, IEnumerable<string> tier4)
            : this(tier1, tier2, tier3, tier4, Enumerable.Empty<string>())
        {
        }

        public BcrOptions(IEnumerable<string> tier1, IEnumerable<string> tier2, IEnumerable<string> tier3, IEnumerable<string> tier4, IEnumerable<string> costCentre)
        {
            _tier1 = tier1;
            _tier2 = tier2;
            _tier3 = tier3;
            _tier4 = tier4;
        }

        [Option(HelpText = "Filter by a tier 1 code.", Separator=',')]
        public IEnumerable<string> Tier1
        {
            get
            {
                return PreventNullAndRemoveEmptyStrings(_tier1);
            }
        }

        [Option(HelpText = "Filter by a tier 2 code.", Separator=',')]
        public IEnumerable<string> Tier2
        {
            get
            {
                return PreventNullAndRemoveEmptyStrings(_tier2);
            }
        }

        [Option(HelpText = "Filter by a tier 3 code.", Separator=',')]
        public IEnumerable<string> Tier3
        {
            get
            {
                return PreventNullAndRemoveEmptyStrings(_tier3);
            }
        }

        [Option(HelpText = "Filter by a tier 4 code.", Separator=',')]
        public IEnumerable<string> Tier4
        {
            get
            {
                return PreventNullAndRemoveEmptyStrings(_tier4);
            }
        }

        public IEnumerable<string> CostCentre
        {
            get
            {
                throw new System.NotSupportedException();
            }
        }

        private IEnumerable<string> PreventNullAndRemoveEmptyStrings(IEnumerable<string> enumerable)
        {
            if (enumerable == null)
            {
                return Enumerable.Empty<string>();
            }

            return enumerable.Where(x => !string.Equals(x, string.Empty));
        }
    }
}