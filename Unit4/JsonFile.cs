using System.IO;
using Newtonsoft.Json;
using Unit4.Automation.Interfaces;

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
            Directory.CreateDirectory(Path.GetDirectoryName(_path));
            File.WriteAllText(_path, JsonConvert.SerializeObject(value));
        }

        public T Read()
        {
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(_path));
        }

        public bool Exists() => File.Exists(_path);

        public bool IsDirty() => false;
    }
}