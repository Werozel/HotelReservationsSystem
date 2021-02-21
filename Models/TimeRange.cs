using System;

namespace Hotels.Models
{
    class TimeRange
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public bool Contains(DateTime dateTime)
        {
            return Start <= dateTime && dateTime <= End;
        }

        public bool Intersects(TimeRange other)
        {
            return this.Contains(other.Start) || this.Contains(other.End);
        }
    }
}
