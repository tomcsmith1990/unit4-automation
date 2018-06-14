using System;
using Unit4.Automation.Interfaces;

namespace Unit4.Automation
{
    internal class Cache<T> : ICache<T>
    {
        private readonly IFile<T> _file;
        private readonly Func<T> _func;

        public Cache(Func<T> func, IFile<T> file)
        {
            _func = func;
            _file = file;
        }

        public T Fetch()
        {
            if (_file.Exists() && !_file.IsDirty())
            {
                return _file.Read();
            }

            T result = _func();

            _file.Write(result);

            return result;
        }
    }
}