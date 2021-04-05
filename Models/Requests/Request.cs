using Hotels.Models.Rooms;
using System;

namespace Hotels.Models.Requests
{
    internal class Request
    {
        public RequestType Type { get; }
        public RoomType RoomType { get; }
        public TimeRange TimeRange { get; }
        public string RoomNumber { get; set; }
        public bool HasDiscount { get; set; }
        public RoomType DiscountRoomType { get; set; }
        public double Price { get; set; }
        public int Step { get; }
        public DateTime BookTime { get; }

        public Request(RequestType type, RoomType roomType, TimeRange timeRange, int step, DateTime bookTime)
        {
            Type = type;
            RoomType = roomType;
            TimeRange = timeRange;
            RoomNumber = null;
            Step = step;
            BookTime = bookTime;
        }

        public bool IsApproved()
        {
            return RoomNumber != null;
        }
    }
}
