using Unit4.Automation.Interfaces;

namespace Unit4.Automation.ReportEngine
{
    internal class Unit4EngineFactory : IUnit4EngineFactory
    {
        private readonly ProgramConfig _config;
        private readonly object _lock = new object();

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