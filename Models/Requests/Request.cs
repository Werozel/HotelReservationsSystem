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
        public String RoomNumber { get; set; } = null;
        public bool HasDiscount { get; set; } = false;
        public RoomType DiscountRoomType { get; set; }

        public Request(RequestType type, RoomType roomType, TimeRange timeRange)
        {
            this.Type = type;
            this.RoomType = roomType;
            this.TimeRange = timeRange;
            this.RoomNumber = null;
        }

        public bool IsApproved()
        {
            return RoomNumber != null;
        }
    }
}
