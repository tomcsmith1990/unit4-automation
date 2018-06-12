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
            if (_options.Tier2 == null)
            {
                return bcr;
            }

            return new Bcr(bcr.Lines.Where(x => x.CostCentre.Tier2.Equals(_options.Tier2.First())).ToList());
        }
    }
}