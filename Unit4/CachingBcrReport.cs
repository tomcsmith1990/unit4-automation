using System;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Unit4.Automation.Model;

namespace Unit4.Automation
{
    internal class CachingBcrReport
    {
        private readonly BcrReport _report;

        public CachingBcrReport(BcrReport report)
        {
            _report = report;
        }

        public Bcr RunBCR(IEnumerable<IGrouping<string, CostCentre>> hierarchy)
        {
            var outputPath = Path.Combine(Directory.GetCurrentDirectory(), "cache", "bcr.json");

            if (File.Exists(outputPath))
            {
                return JsonConvert.DeserializeObject<Bcr>(File.ReadAllText(outputPath));
            }

            var bcr = _report.RunBCR(hierarchy);


            File.WriteAllText(outputPath, JsonConvert.SerializeObject(bcr));

            return bcr;
        }
    }
}