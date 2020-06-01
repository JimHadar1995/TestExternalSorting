namespace BigSortingAlgorithm.Internal
{
    internal interface IRecordSource
    {
        bool HasMoreRecords { get; }
        bool MoveToNextRunData();
        Line Read();
    }
}
