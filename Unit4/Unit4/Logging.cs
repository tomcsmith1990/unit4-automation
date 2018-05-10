using System;
using System.Diagnostics;
using Log = ReportEngine.Diagnostics.Log;
using System.IO;

namespace Unit4
{
    internal class Logging
    {
        private readonly string m_LogFilePath;

        public string Path { get { return m_LogFilePath; } }

        public Logging()
        {
            m_LogFilePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "log", string.Format("{0}.log", Guid.NewGuid().ToString("N")));
        }
        public void Start()
        {
            var logFile = new ReportEngine.Diagnostics.LogFileListener(m_LogFilePath, true);
            Log.Level = TraceLevel.Verbose;
            Log.Open(logFile);
        }

        public void Error(Exception exception)
        {
            if (exception is ReportEngine.Data.Sql.LineException)
            {
                Log.Error(((ReportEngine.Data.Sql.LineException)exception).FullLine);
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