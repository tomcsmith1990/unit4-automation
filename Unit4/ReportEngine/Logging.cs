using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using ReportEngine.Data.Sql;
using ReportEngine.Diagnostics;
using Unit4.Automation.Interfaces;

namespace Unit4.Automation.ReportEngine
{
    internal class Logging : ILogging
    {
        public Logging()
        {
            var assemblyDirectory = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Path = System.IO.Path.Combine(
                assemblyDirectory,
                "log",
                string.Format("{0}.log", Guid.NewGuid().ToString("N")));
        }

        public string Path { get; }

        public void Start()
        {
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(Path));
            var logFile = new LogFileListener(Path, true);
            Log.Level = TraceLevel.Verbose;
            Log.Open(logFile);
        }

        public void Close() => Log.Close();

        public void Info(string message) => Log.Info(message);

        public void Error(string message) => Log.Error(message);

        public void Error(Exception exception)
        {
            var lineException = exception as LineException;
            if (lineException != null)
            {
                Log.Error(lineException.FullLine);
            }

            Log.Error(exception.Message);
            Log.Error(exception.GetType().ToString());
            Log.Error(exception.StackTrace);

            var aggregateException = exception as AggregateException;
            if (aggregateException != null)
            {
                foreach (var e in aggregateException.InnerExceptions)
                {
                    Error(e);
                }
            }
        }
    }
}