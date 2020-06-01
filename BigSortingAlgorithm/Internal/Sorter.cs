using System;

namespace BigSortingAlgorithm.Internal
{
    internal class Sorter : IRecordSource, IDisposable
    {
        private static readonly int BatchSize = 100000;
        private readonly FileReader _reader;
        private BinaryHeap<Line> _runRecords;

        public Sorter(FileReader reader)
        {
            _reader = reader;
        }

        public void Dispose()
        {
            _reader?.Dispose();
        }

        public bool MoveToNextRunData()
        {
            return ReadNextRunData();
        }

        public bool HasMoreRecords => !_reader.EndOfFile;

        public Line Read()
        {
            if (_runRecords == null)
            {
                ReadNextRunData();
            }
            if (_runRecords.Count > 0)
            {
                return _runRecords.RemoveRoot();
            }
            return null;
        }

        private const int Mbyte = 1024 * 1024;

        private const long GByte = Mbyte * 1024;

        private bool ReadNextRunData()
        {
            _runRecords = new BinaryHeap<Line>(Mbyte);

            _reader.ResumeReading();

            bool breakFromWhile = false;

            while (!breakFromWhile)
            {
                for (var i = 0; i < BatchSize; i++)
                {
                    var record = _reader.Read();

                    if (record == null)
                    {
                        breakFromWhile = true;
                        break;
                    }

                    _runRecords.Insert(record);
                }


                if (SystemInfo.GetOccupiedMemoryPercent() >= 0.8 &&
                    SystemInfo.GetFreeMemoryLeft() < GByte)
                {
                    _reader.PauseReading();
                }
            }

            if (_runRecords.Count < 1)
            {
                return false;
            }
            return true;
        }
    }
}
