﻿using System;

namespace Hotels.Models
{
    class TimeRange
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public TimeRange(DateTime start, DateTime end)
        {
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
            return this.Contains(other.Start) || this.Contains(other.End);
        }
    }
}
