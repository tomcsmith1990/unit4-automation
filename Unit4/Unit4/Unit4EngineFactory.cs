using System;

namespace Unit4
{
    internal class Unit4EngineFactory
    {
        private readonly object _lock = new object();

        public Unit4Engine Create()
        {
            lock (_lock) // probably not necessary?
            {
                var credentials = new Credentials();

                return new Unit4Engine(credentials);
            }
        }
    }
}