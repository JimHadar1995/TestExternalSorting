using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BigSortingAlgorithm.Internal
{
    internal sealed class FileReader : IDisposable
    {
        private const int BatchSize = 100000;
        private readonly ManualResetEvent _moveToNextRunEvent;
        private readonly ManualResetEvent _pauseReadingEvent;
        private readonly StreamReader _reader;
        private volatile bool _pauseReading;
        private readonly CancellationTokenSource _cts;
        private Task _task;

        public FileReader(string filePath)
        {
            var fstream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 1 << 18, FileOptions.SequentialScan);
            TotalSizeInBytes = fstream.Length;
            _reader = new StreamReader(fstream);
            _pauseReadingEvent = new ManualResetEvent(true);
            _moveToNextRunEvent = new ManualResetEvent(true);
            Queue = new BlockingCollection<Line>(BatchSize * 2);
            _cts = new CancellationTokenSource();
            StartReading(_cts.Token);
        }

        public long TotalSizeInBytes { get; private set; }

        public bool EndOfFile
        {
            get { return Queue.IsAddingCompleted; }
        }

        private BlockingCollection<Line> Queue { get; }

        public void Dispose()
        {
            _reader?.Dispose();
            _pauseReadingEvent?.Dispose();
            _cts.Cancel();
        }

        public bool MoveToNextRun()
        {
            if (!EndOfFile)
            {
                _moveToNextRunEvent.Set();
                return true;
            }
            return false;
        }

        private void StartReading(CancellationToken token)
        {
            if (_task != null)
            {
                return;
            }

            _task = Task.Factory.StartNew(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    WaitHandle.WaitAll(new WaitHandle[] { _moveToNextRunEvent, _pauseReadingEvent });
                    for (var i = 0; i < BatchSize; i++)
                    {
                        var line = _reader.ReadLine();

                        if (line == "===")
                        {
                            _moveToNextRunEvent.Reset();
                            break;
                        }

                        if (string.IsNullOrEmpty(line))
                        {
                            if (_reader.EndOfStream)
                            {
                                break;
                            }
                            continue;
                        }

                        var span = line.AsSpan();
                        var separatorPos = span.IndexOf('.');

                        Queue.Add(new Line(
                            uint.Parse(span[..separatorPos]),
                            span[(separatorPos + 1)..].Trim().ToString()));
                    }

                    if (_reader.EndOfStream)
                    {
                        break;
                    }
                }

                if (_reader.EndOfStream)
                    Queue.CompleteAdding();
            }, token);
        }

        public void ResumeReading()
        {
            _pauseReadingEvent.Set();
            _pauseReading = false;
        }

        public void PauseReading()
        {
            _pauseReadingEvent.Reset();
            _pauseReading = true;
        }

        public Line Read()
        {
            if (!Queue.IsCompleted)
            {
                try
                {
                    var spinWait = new SpinWait();
                    Line record = null;
                    while (!Queue.IsCompleted && !Queue.TryTake(out record))
                    {
                        if (_pauseReading || !_moveToNextRunEvent.WaitOne(0))
                        {
                            return null;
                        }
                        spinWait.SpinOnce();
                    }

                    return record;
                }
                catch (InvalidOperationException)
                {
                    return null;
                }
            }
            return null;
        }
    }
}
