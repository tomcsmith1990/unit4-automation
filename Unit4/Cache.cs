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
        private readonly Func<T> _func;
        private readonly string _filename;

        public Cache(Func<T> func, string filename)
        {
            _func = func;
            _filename = filename;
        }

        public T Fetch()
        {
            var outputPath = Path.Combine(Directory.GetCurrentDirectory(), "cache", _filename);

            if (File.Exists(outputPath))
            {
                return JsonConvert.DeserializeObject<T>(File.ReadAllText(outputPath));
            }

            T result = _func();

            File.WriteAllText(outputPath, JsonConvert.SerializeObject(result));

            return result;
        }
    }
}