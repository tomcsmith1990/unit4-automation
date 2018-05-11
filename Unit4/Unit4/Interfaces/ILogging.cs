using System;

namespace Unit4.Interfaces
{
    internal interface ILogging
    {
        string Path { get; }

        void Start();
        void Close();

        void Info(string message);
        void Error(string message);
        void Error(Exception exception);
    }
}