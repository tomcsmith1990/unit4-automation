using System;
using System.Collections.Generic;
using System.Linq;

namespace Unit4.Automation.Model
{
    internal class Bcr
    {
        private IEnumerable<BcrLine> _lines;

        public IEnumerable<BcrLine> Lines
        {
            get
            {
                return _lines == null ? Enumerable.Empty<BcrLine>() : _lines;
            }
            set
            {
                _lines = value;
            }
        }
    }
}