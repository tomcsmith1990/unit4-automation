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
            var newBcr = new Bcr();
            newBcr.Lines = _options.Tier2 == null ? bcr.Lines.ToList() : bcr.Lines.Where(x => x.CostCentre.Tier2.Equals(_options.Tier2)).ToList();
            return newBcr;
        }
    }
}