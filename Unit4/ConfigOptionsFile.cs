using Unit4.Automation.Model;
using Unit4.Automation.Interfaces;

namespace Unit4.Automation
{
    internal class ConfigOptionsFile
    {
        private readonly IFile<ConfigOptions> _file;
        
        public ConfigOptionsFile(string path)
        {
            _file = new JsonFile<ConfigOptions>(path);
        }

        public void Save(ConfigOptions options) => _file.Write(options);

        public ConfigOptions Load()
        {
            if (Exists())
            {
                return _file.Read();
            }

            throw new System.IO.FileNotFoundException("Could not load config from file");
        }

        public bool Exists() => _file.Exists();
    }
}