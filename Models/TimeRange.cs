using System;

namespace Hotels.Models
{
    internal class TimeRange
    {
        private const string CellFormatString = "dd.MM.yy";

        public DateTime Start { get; }
        public DateTime End { get; }

        public TimeRange(DateTime start, DateTime end)
        {
            if (end < start)
            {
                throw new Exception("TimeRange: End can't be before Start");
            }
            Start = start;
            End = end;
        }

        public bool Contains(DateTime dateTime)
        {
            return Start <= dateTime && dateTime <= End;
        }

        public bool Intersects(TimeRange other)
        {
            return !(Start >= other.End || End <= other.Start);
        }

        public string ToCellString()
        {
            return Start.ToString(CellFormatString) + " - " + End.ToString(CellFormatString);
        }
    }
}
