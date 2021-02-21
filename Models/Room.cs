using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public RoomType roomType { get; set; }
        public List<TimeRange> bookedTimes { get; set; }

        public bool Book(TimeRange timeToBook)
        {
            DateTime timeToBookStart = timeToBook.Item1;
            DateTime timeToBookEnd = timeToBook.Item2;
            foreach (TimeRange time in bookedTimes)
            {
                DateTime start = time.Item1;
                DateTime end = time.Item2;
                if ((start <= timeToBookStart && timeToBookStart <= end) || (start <= timeToBookEnd && timeToBookEnd <= end))
                {
                    return false;
                }
            }
            bookedTimes.Add(timeToBook);
            return true;
        }

    }
}
