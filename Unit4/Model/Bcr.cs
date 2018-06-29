using System.Collections.Generic;
using System.Linq;

namespace Unit4.Automation.Model
{
    internal class Bcr
    {
        public Bcr(IEnumerable<BcrLine> lines)
        {
            if (lines == null)
            {
                Lines = Enumerable.Empty<BcrLine>();
            }
            else
            {
                Lines = lines;
            }
        }

        public IEnumerable<BcrLine> Lines { get; }
    }
}