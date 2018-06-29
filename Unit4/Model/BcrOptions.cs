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
        private readonly IEnumerable<string> _costCentre;
        private readonly string _outputDirectory;

        public BcrOptions() : this(Enumerable.Empty<string>(), Enumerable.Empty<string>(), Enumerable.Empty<string>(), Enumerable.Empty<string>(), Enumerable.Empty<string>(), null)
        {}

        // https://github.com/commandlineparser/commandline/issues/188
        // Constructor parameter order must be the same order as properties are defined in class.
        // It also looks like the parameter name must match the property name, not the option name.
        public BcrOptions(IEnumerable<string> tier1, IEnumerable<string> tier2, IEnumerable<string> tier3, IEnumerable<string> tier4, IEnumerable<string> costCentre, string outputDirectory)
        {
            _tier1 = tier1;
            _tier2 = tier2;
            _tier3 = tier3;
            _tier4 = tier4;
            _costCentre = costCentre;
            _outputDirectory = outputDirectory;
        }

        [Option(HelpText = "Filter by a tier 1 code.", Separator=',')]
        public IEnumerable<string> Tier1 => PreventNullAndRemoveEmptyStrings(_tier1);

        [Option(HelpText = "Filter by a tier 2 code.", Separator=',')]
        public IEnumerable<string> Tier2 => PreventNullAndRemoveEmptyStrings(_tier2);

        [Option(HelpText = "Filter by a tier 3 code.", Separator=',')]
        public IEnumerable<string> Tier3 => PreventNullAndRemoveEmptyStrings(_tier3);

        [Option(HelpText = "Filter by a tier 4 code.", Separator=',')]
        public IEnumerable<string> Tier4 => PreventNullAndRemoveEmptyStrings(_tier4);

        [Option(HelpText = "Filter by a cost centre.", Separator=',')]
        public IEnumerable<string> CostCentre => PreventNullAndRemoveEmptyStrings(_costCentre);

        [Option("output", HelpText = "The directory to save the bcr in. The directory must already exist.")]
        public string OutputDirectory => _outputDirectory;

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