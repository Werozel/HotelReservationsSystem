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

    enum RequestStatus
    {
        PENDING,
        APPROVED,
        DENIED,
    }

    class Request
    {

        public RequestType Type { get; set; }
        public RequestStatus Status { get; set; }
        public RoomType RoomType { get; set; }
        public TimeRange TimeRange { get; set; }

        public Request(RequestType type, RoomType roomType, TimeRange timeRange)
        {
            this.Type = type;
            this.RoomType = roomType;
            this.TimeRange = timeRange;
            this.Status = RequestStatus.PENDING;
        }

        public Request(RequestType type, RoomType roomType, int length)
        {
            if (type != RequestType.IMMEDIATE)
            {
                throw new Exception("type can only be RequestType.IMMEDIATE in this constructor");
            }
            this.Type = RequestType.IMMEDIATE;
            this.RoomType = roomType;
            this.TimeRange = new TimeRange(length);
            this.Status = RequestStatus.PENDING;
        }

    }
}
