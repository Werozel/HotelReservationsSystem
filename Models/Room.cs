using System;
using System.Collections.Generic;

namespace Hotels.Models
{

    public enum RoomType
    {
        SINGLE,
        DOUBLE,
        DOUBLE_WITH_SOFA,
        JUNIOR_SUITE,
        SUITE,
    }

    class Room
    {
        public RoomType RoomType { get; set; }
        public List<TimeRange> BookedTimes { get; set; }

        public Room(RoomType roomType)
        {
            this.RoomType = roomType;
            this.BookedTimes = new List<TimeRange>();
        }

        public bool Book(TimeRange timeToBook)
        {
            foreach (TimeRange time in BookedTimes)
            {
                if (timeToBook.Intersects(time))
                {
                    return false;
                }
            }
            BookedTimes.Add(timeToBook);
            return true;
        }

    }
}
