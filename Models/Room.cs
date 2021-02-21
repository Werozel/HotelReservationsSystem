using System;
using System.Collections.Generic;

namespace Hotels.Models
{

    using TimeRange = Tuple<DateTime, DateTime>;

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

        public bool Book(TimeRange timeToBook)
        {
            DateTime timeToBookStart = timeToBook.Item1;
            DateTime timeToBookEnd = timeToBook.Item2;
            foreach (TimeRange time in BookedTimes)
            {
                DateTime start = time.Item1;
                DateTime end = time.Item2;
                if ((start <= timeToBookStart && timeToBookStart <= end) || (start <= timeToBookEnd && timeToBookEnd <= end))
                {
                    return false;
                }
            }
            BookedTimes.Add(timeToBook);
            return true;
        }

    }
}
