using System.IO;
using Unit4.Automation.Interfaces;
using Newtonsoft.Json;

namespace Unit4.Automation
{
    internal class JsonFile<T> : IFile<T>
    {
        private readonly string _path;

        public JsonFile(string path)
        {
            _path = path;
        }

        public void Write(T value)
        {
            File.WriteAllText(_path, JsonConvert.SerializeObject(value));
        }

        public T Read()
        {
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(_path));
        }

        public bool Exists()
        {
            return File.Exists(_path);
        }

        public bool IsDirty()
        {
            return false;
        }
    }
}