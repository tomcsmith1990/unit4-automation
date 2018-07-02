using Unit4.Automation.Model;
using Unit4.Automation.Interfaces;
using System.Linq;
using System.Collections.Generic;

namespace Unit4.Automation.Commands.BcrCommand
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
            return new Bcr(bcr.Lines.Where(x => Matches(x.CostCentre)).ToList());
        }

        private bool Matches(CostCentre costCentre)
        {
            return costCentre.Matches(_options);
        }
    }
}