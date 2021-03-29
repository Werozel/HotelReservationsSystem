using System;

namespace Hotels.Models
{
    class TimeRange
    {
        public static string CELL_FORMAT_STRING = "dd.MM.yy";
        public static string FULL_FORMAT_STRING = "hh:mm, dd.MM.yy";

        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public TimeRange(DateTime start, DateTime end)
        {
            if (end < start)
            {
                throw new Exception("TimeRange: End can't be before Start");
            }
            this.Start = start;
            this.End = end;
        }

        public TimeRange(int length)
        {
            this.Start = DateTime.Now;
            this.End = DateTime.Now;
            this.End.AddDays(length);
        }

        public bool Contains(DateTime dateTime)
        {
            return Start <= dateTime && dateTime <= End;
        }

        public bool Intersects(TimeRange other)
        {
            return !(this.Start >= other.End || this.End <= other.Start);
        }

        public string ToCellString()
        {
            return Start.ToString(CELL_FORMAT_STRING) + " - " + End.ToString(CELL_FORMAT_STRING);
        }
    }
}
