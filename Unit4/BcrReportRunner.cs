using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Data;
using System.Diagnostics;
using System.Collections.Concurrent;
using Unit4.Automation.Interfaces;
using Unit4.Automation.ReportEngine;
using Unit4.Automation.Model;

namespace Unit4.Automation
{
    internal class BcrReportRunner : IRunner
    {
        private readonly ILogging _log;
        private readonly IBcrReader _reader;
        private readonly IBcrMiddleware _middleware;
        private readonly IBcrWriter _writer;

        public BcrReportRunner(ILogging log, IBcrReader reader, IBcrMiddleware middleware, IBcrWriter writer)
        {
            _log = log;
            _reader = reader;
            _middleware = middleware;
            _writer = writer;
        }

        public void Run()
        {
            try
            {
                _log.Start();

                var outputPath = Path.Combine(Directory.GetCurrentDirectory(), "output", string.Format("{0}.xlsx", Guid.NewGuid().ToString("N")));

                using (var progress = new Progress())
                {
                    progress.Update("Getting BCRs");

                    var bcr = _reader.Read();

                    progress.Complete();

                    progress.Update("Writing to Excel");

                    _writer.Write(outputPath, bcr);

                    progress.Complete();
                }

                Console.WriteLine(string.Format("Success - {0}", outputPath));
            }
            catch (Exception e)
            {    
                _log.Error(e);
                Console.WriteLine(_log.Path);        
            }
            finally
            {
                _log.Close();
            }
        }

        internal class Progress : IDisposable
        {
            private readonly Stopwatch _stopwatch;
            private long _elapsed = 0, _current = 0;

            public Progress()
            {
                _stopwatch = new Stopwatch();
                _stopwatch.Start();
            }

            public void Update(string message)
            {
                Console.WriteLine(message);
            }

            public void Complete()
            {
                _current = _stopwatch.ElapsedMilliseconds;
                Console.WriteLine(string.Format("Elapsed: {0}ms", _current - _elapsed));
                _elapsed = _current;
            }

            public void Dispose()
            {
                Console.WriteLine(string.Format("Total time elapsed: {0}ms", _elapsed));
            }
        }
    }
}