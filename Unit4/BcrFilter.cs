using System;
using Unit4.Automation.Model;
using Unit4.Automation.Interfaces;
using System.Linq;

namespace Unit4.Automation
{
    internal class BcrFilter : IBcrMiddleware
    {
        private readonly BcrOptions _options;

        public BcrFilter(BcrOptions options)
        {
            _options = options;
        }

        public Bcr Use(Bcr bcr)
        {
            if (_options.Tier2 == null || !_options.Tier2.Any())
            {
                return bcr;
            }

            return new Bcr(bcr.Lines.Where(x => x.CostCentre.Matches(_options)).ToList());
        }
    }
}