using Unit4.Automation.Interfaces;
using Unit4.Automation.Model;

namespace Unit4.Automation.ReportEngine
{
    internal class Unit4EngineFactory : IUnit4EngineFactory
    {
        private readonly object _lock = new object();
        private readonly ProgramConfig _config;

        public Unit4EngineFactory(ProgramConfig config)
        {
            _config = config;
        }

        public IUnit4Engine Create()
        {
            lock (_lock) // probably not necessary?
            {
                return new Unit4Engine(_config);
            }
        }
    }
}