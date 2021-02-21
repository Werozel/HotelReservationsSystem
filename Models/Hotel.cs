using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotels.Models
{

    using TimeRange = Tuple<DateTime, DateTime>;

    class Hotel
    {
        public List<Room> Rooms { get; set; }

        public bool Book(TimeRange timeToBook)
        {
            foreach (Room room in Rooms)
            {
                if (room.Book(timeToBook))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
