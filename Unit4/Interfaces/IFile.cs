namespace Unit4.Automation.Interfaces
{
    internal interface IFile<T>
    {
        T Read();
        void Write(T value);
        bool Exists();
        bool IsDirty();
    }
}