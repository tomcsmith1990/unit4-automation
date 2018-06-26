using Unit4.Automation.Interfaces;

namespace Unit4.Automation.ReportEngine
{
    internal class Unit4EngineFactory : IUnit4EngineFactory
    {
        private readonly object _lock = new object();

        public IUnit4Engine Create()
        {
            lock (_lock) // probably not necessary?
            {
                return new Unit4Engine();
            }
        }
    }
}