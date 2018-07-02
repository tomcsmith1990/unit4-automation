using System;
using System.IO;
using System.Diagnostics;
using Unit4.Automation.Interfaces;
using Unit4.Automation.Model;
using Unit4.Automation.ReportEngine;

namespace Unit4.Automation.Commands.BcrCommand
{
    internal class BcrReportRunner : IRunner
    {
        private readonly ILogging _log;
        private readonly IBcrReader _reader;
        private readonly IBcrMiddleware _middleware;
        private readonly IBcrWriter _writer;
        private readonly TextWriter _progress;
        private readonly IPathProvider _pathProvider;

        public BcrReportRunner(ILogging log, IBcrReader reader, IBcrMiddleware middleware, IBcrWriter writer, IPathProvider pathProvider)
            : this(log, reader, middleware, writer, pathProvider, Console.Out)
        {
        }

        public BcrReportRunner(ILogging log, IBcrReader reader, IBcrMiddleware middleware, IBcrWriter writer, IPathProvider pathProvider, TextWriter progress)
        {
            _log = log;
            _reader = reader;
            _middleware = middleware;
            _writer = writer;
            _progress = progress;
            _pathProvider = pathProvider;
        } 

        public void Run()
        {
            try
            {
                _log.Start();

                var outputPath = _pathProvider.NewPath();

                using (var progress = new Progress(_progress))
                {
                    progress.Update("Getting BCRs");

                    var bcr = _reader.Read();

                    progress.Complete();

                    var finalBcr = _middleware.Use(bcr);

                    progress.Update("Writing to Excel");

                    _writer.Write(outputPath, finalBcr);

                    progress.Complete();
                }

                _progress.WriteLine("Success - {0}", outputPath);
            }
            catch (Exception e)
            {    
                _log.Error(e);
                _progress.WriteLine(e.Message);
                _progress.WriteLine(_log.Path);        
            }
            finally
            {
                _log.Close();
            }
        }

        public static BcrReportRunner Create(BcrOptions options, ProgramConfig config)
        { 
            var log = new Logging();
            var reader = 
                new BcrReader(
                    log, 
                    options, 
                    config, 
                    new JsonFile<Bcr>(Path.Combine(Directory.GetCurrentDirectory(), "cache", "bcr.json")),
                    new JsonFile<SerializableCostCentreList>(Path.Combine(Directory.GetCurrentDirectory(), "cache", "costCentres.json")));
            var filter = new BcrFilter(options);
            var writer = new Excel();
            var pathProvider = new PathProvider(options);
            return new BcrReportRunner(log, reader, filter, writer, pathProvider);
        }

        private class Progress : IDisposable
        {
            private readonly Stopwatch _stopwatch;
            private long _elapsed, _current;

            private readonly TextWriter _output;

            public Progress(TextWriter output)
            {
                _output = output;
                _stopwatch = new Stopwatch();
                _stopwatch.Start();
            }

            public void Update(string message)
            {
                _output.WriteLine(message);
            }

            public void Complete()
            {
                _current = _stopwatch.ElapsedMilliseconds;
                _output.WriteLine("Elapsed: {0}ms", _current - _elapsed);
                _elapsed = _current;
            }

            public void Dispose()
            {
                _output.WriteLine("Total time elapsed: {0}ms", _elapsed);
            }
        }
    }
}