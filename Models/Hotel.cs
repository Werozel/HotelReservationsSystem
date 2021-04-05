using Hotels.Models.Requests;
using Hotels.Models.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hotels.Models
{
    internal class Hotel
    {
        public List<Room> Rooms { get; }
        public double Profit { get; private set; }

        public Hotel(List<Room> rooms)
        {
            Rooms = rooms;
        }

        public Room Book(Request request)
        {
            var rnd = new Random();
            var roomType = request.HasDiscount ? request.DiscountRoomType : request.RoomType;
            var roomsFilteredByType = GetAllRoomsByRoomType(roomType).OrderBy(a => rnd.Next()).ToList();

            foreach (var room in roomsFilteredByType)
            {
                if (!room.Book(request.TimeRange)) continue;
                Profit += room.Price * (request.HasDiscount ? 0.7 : 1);
                return room;
            }
            return null;
        }

        public IEnumerable<Room> GetAllRoomsByRoomType(RoomType roomType)
        {
            return Rooms.FindAll(room => room.RoomType == roomType);
        }

    }
}
