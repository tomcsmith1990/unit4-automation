using System.Collections.Generic;
using System.Linq;

namespace Unit4.Automation.Model
{
    internal class Bcr
    {
        private readonly IEnumerable<BcrLine> _lines;

        public Bcr(IEnumerable<BcrLine> lines)
        {
            if (lines == null)
            {
                _lines = Enumerable.Empty<BcrLine>();
            }
            else
            {
                _lines = lines;
            }
        }

        public IEnumerable<BcrLine> Lines => _lines;
    }
}