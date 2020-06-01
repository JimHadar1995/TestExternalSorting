using System;

namespace BigSortingAlgorithm.Internal
{
    /// <summary>
    /// Строка
    /// </summary>
    internal sealed class Line : IComparable<Line>
    {
        public Line(uint number, string text)
        {
            Number = number;
            Text = text;
        }

        public uint Number { get; }
        public string Text { get; }

        public int CompareTo(Line other)
        {
            var compareResult = string.Compare(Text, other.Text, StringComparison.InvariantCulture);
            if (compareResult == 0)
            {
                return Number.CompareTo(other.Number);
            }
            return compareResult;
        }

        public override string ToString()
        {
            return $"{Number}. {Text}";
        }
    }
}
