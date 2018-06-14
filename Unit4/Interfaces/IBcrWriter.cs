using Unit4.Automation.Model;

namespace Unit4.Automation.Interfaces
{
    internal interface IBcrWriter
    {
        void Write(string path, Bcr bcr);
    }
}