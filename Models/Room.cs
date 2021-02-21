using System;
using System.Collections.Generic;

namespace Hotels.Models
{

    enum RoomType
    {
        SINGLE = 20,
        DOUBLE = 35,
        DOUBLE_WITH_SOFA = 45,
        JUNIOR_SUITE = 60,
        SUITE = 80,
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
