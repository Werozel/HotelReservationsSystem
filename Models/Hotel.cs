using System.Collections.Generic;

namespace Hotels.Models
{

    class Hotel
    {
        public List<Room> Rooms { get; set; }
        public int Profit { get; private set; } = 0;

        public Hotel(List<Room> rooms)
        {
            this.Rooms = rooms;
        }

        public Room Book(Request request)
        {
            IList<Room> roomsFilteredByType = Rooms.FindAll(room => room.RoomType == request.RoomType);

            foreach (Room room in Rooms)
            {
                if (room.Book(request.TimeRange))
                {
                    this.Profit += room.Price;
                    return room;
                }
            }
            return null;
        }

    }
}
