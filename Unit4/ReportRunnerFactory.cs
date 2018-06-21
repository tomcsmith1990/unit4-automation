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
                var filter = new BcrFilter(bcrOptions);
                var writer = new Excel();
                var pathProvider = new PathProvider(bcrOptions);
                return new BcrReportRunner(log, reader, filter, writer, pathProvider);
            }

            return new NullRunner();
        }
    }
}