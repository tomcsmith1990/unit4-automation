using System;
using Unit4.Automation.Interfaces;
using Unit4.Automation.Model;
using Unit4.Automation.ReportEngine;

namespace Unit4.Automation
{
    internal class ReportRunnerFactory
    {
        public IRunner Create(IOptions options)
        {
            var bcrOptions = options as BcrOptions;
            if (bcrOptions != null)
            {
                var log = new Logging();
                var reader = new BcrReader(log);
                var writer = new Excel();
                return new BcrReportRunner(log, reader, new NullMiddleware(), writer);
            }

            return new NullRunner();
        }
    }

    internal class NullMiddleware : IBcrMiddleware
    {
        public Bcr Use(Bcr bcr)
        {
            return bcr;
        }
    }
}