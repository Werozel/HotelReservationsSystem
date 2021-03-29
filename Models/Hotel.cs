using System.Collections.Generic;

namespace Hotels.Models
{

    class Hotel
    {
        public List<Room> Rooms { get; }
        public int Profit { get; private set; } = 0;

        public Hotel(List<Room> rooms)
        {
            this.Rooms = rooms;
        }

        public Room Book(Request request)
        {
            IList<Room> roomsFilteredByType = GetAllRoomsByRoomType(request.RoomType);

            foreach (Room room in roomsFilteredByType)
            {
                if (room.Book(request.TimeRange))
                {
                    this.Profit += room.Price;
                    return room;
                }
            }
            return null;
        }

        public IList<Room> GetAllRoomsByRoomType(RoomType roomType)
        {
            return Rooms.FindAll(room => room.RoomType == roomType);
        }

    }
}
