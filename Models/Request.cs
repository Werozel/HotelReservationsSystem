using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotels.Models
{

    enum RequestType
    {
        BOOK,
        IMMEDIATE,
    }

    class Request
    {

        public RequestType Type { get; }
        public RoomType RoomType { get; }
        public TimeRange TimeRange { get; }
        public String RoomNumber { get; set; } = null;

        public Request(RequestType type, RoomType roomType, TimeRange timeRange)
        {
            this.Type = type;
            this.RoomType = roomType;
            this.TimeRange = timeRange;
            this.RoomNumber = null;
        }

        public Request(RequestType type, RoomType roomType, int lengthDays)
        {
            if (type != RequestType.IMMEDIATE)
            {
                throw new Exception("type can only be RequestType.IMMEDIATE in this constructor");
            }
            this.Type = RequestType.IMMEDIATE;
            this.RoomType = roomType;
            this.TimeRange = new TimeRange(lengthDays);
            this.RoomNumber = null;
        }

        public bool IsApproved()
        {
            return RoomNumber != null;
        }
    }
}
