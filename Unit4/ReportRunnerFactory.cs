using System;
using Unit4.Automation.Interfaces;
using Unit4.Automation.Model;

namespace Unit4.Automation
{
    internal class ReportRunnerFactory
    {
        public IRunner Create(IOptions options)
        {
            var bcrOptions = options as BcrOptions;
            if (bcrOptions != null)
            {
                return new BcrReportRunner();
            }

            return new NullRunner();
        }
    }
}