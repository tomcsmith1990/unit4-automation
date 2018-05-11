using System;
using Unit4.Interfaces;

namespace Unit4
{
    internal class Unit4EngineFactory : IUnit4EngineFactory
    {
        private readonly object _lock = new object();

        public IUnit4Engine Create()
        {
            lock (_lock) // probably not necessary?
            {
                var credentials = new Credentials();

                return new Unit4Engine(credentials);
            }
        }
    }
}