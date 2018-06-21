using System;
using System.Diagnostics;
using Log = ReportEngine.Diagnostics.Log;
using System.IO;
using Unit4.Automation.Interfaces;
using ReportEngine.Diagnostics;
using ReportEngine.Data.Sql;

namespace Unit4.Automation.ReportEngine
{
    internal class Logging : ILogging
    {
        private readonly string _logFilePath;

        public string Path { get { return _logFilePath; } }

        public Logging()
        {
            _logFilePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "log", string.Format("{0}.log", Guid.NewGuid().ToString("N")));
        }
        public void Start()
        {
            var logFile = new LogFileListener(_logFilePath, true);
            Log.Level = TraceLevel.Verbose;
            Log.Open(logFile);
        }

        public void Close()
        {
            Log.Close();
        }

        public void Info(string message)
        {
            Log.Info(message);
        }

        public void Error(string message)
        {
            Log.Error(message);
        }

        public void Error(Exception exception)
        {
            if (exception is LineException)
            {
                Log.Error(((LineException)exception).FullLine);
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