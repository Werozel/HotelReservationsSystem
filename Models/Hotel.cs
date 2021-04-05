using Hotels.Models.Requests;
using Hotels.Models.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;

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
            var rnd = new Random();
            RoomType roomType = request.HasDiscount ? request.DiscountRoomType : request.RoomType;
            List<Room> roomsFilteredByType = GetAllRoomsByRoomType(roomType).OrderBy(a => rnd.Next()).ToList();

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

        public List<Room> GetAllRoomsByRoomType(RoomType roomType)
        {
            return Rooms.FindAll(room => room.RoomType == roomType);
        }

    }
}
