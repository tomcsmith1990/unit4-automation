using System;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Unit4.Automation.Model;
using Unit4.Automation.Interfaces;

namespace Unit4.Automation
{
    internal class Cache<T> : ICache<T>
    {
        private readonly IFile<T> _file;
        private readonly Func<T> _func;

        public Cache(Func<T> func, string filename)
        {
            _func = func;
            _file = new JsonFile<T>(Path.Combine(Directory.GetCurrentDirectory(), "cache", filename));
        }

        public T Fetch()
        {
            if (_file.Exists())
            {
                return _file.Read();
            }

            T result = _func();

            _file.Write(result);

            return result;
        }
    }
}