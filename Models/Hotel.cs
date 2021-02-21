using System.Collections.Generic;

namespace Hotels.Models
{

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

        public bool Book(Request request)
        {
            return this.Book(request.TimeRange);
        }

    }
}
