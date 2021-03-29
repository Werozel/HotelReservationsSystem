using Hotels.Models.Requests;
using Hotels.Models.Rooms;
using System.Collections.Generic;

namespace Hotels.Models
{

    class Hotel
    {
        public List<Room> Rooms { get; }
        public double Profit { get; private set; } = 0;

        public Hotel(List<Room> rooms)
        {
            this.Rooms = rooms;
        }

        public Room Book(Request request)
        {
            RoomType roomType = request.HasDiscount ? request.DiscountRoomType : request.RoomType;
            IList<Room> roomsFilteredByType = GetAllRoomsByRoomType(roomType);

            foreach (Room room in roomsFilteredByType)
            {
                if (room.Book(request.TimeRange))
                {
                    this.Profit +=  room.Price * (request.HasDiscount ? 0.7 : 1);
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
