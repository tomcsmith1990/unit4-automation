using System.Collections.Generic;
using System.Linq;

namespace Unit4.Automation.Model
{
    internal class Bcr
    {
        public Bcr(IEnumerable<BcrLine> lines)
        {
            Lines = lines ?? Enumerable.Empty<BcrLine>();
        }

        public IEnumerable<BcrLine> Lines { get; }
    }
}