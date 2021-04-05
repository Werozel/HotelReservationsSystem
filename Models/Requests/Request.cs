using Hotels.Models.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotels.Models.Requests
{
    class Request
    {

        public RequestType Type { get; }
        public RoomType RoomType { get; }
        public TimeRange TimeRange { get; }
        public string RoomNumber { get; set; } = null;
        public bool HasDiscount { get; set; } = false;
        public RoomType DiscountRoomType { get; set; }
        public double Price { get; set; }
        public int Step { get; }
        public DateTime BookTime { get; }

        public Request(RequestType type, RoomType roomType, TimeRange timeRange, int step, DateTime bookTime)
        {
            this.Type = type;
            this.RoomType = roomType;
            this.TimeRange = timeRange;
            this.RoomNumber = null;
            this.Step = step;
            this.BookTime = bookTime;
        }

        public bool IsApproved()
        {
            return RoomNumber != null;
        }
    }
}
