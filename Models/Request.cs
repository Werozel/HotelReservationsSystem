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

        public RequestType Type { get; }
        public RequestStatus Status { get; }
        public RoomType RoomType { get; }
        public TimeRange TimeRange { get; }

        public Request(RequestType type, RoomType roomType, TimeRange timeRange)
        {
            this.Type = type;
            this.RoomType = roomType;
            this.TimeRange = timeRange;
            this.Status = RequestStatus.PENDING;
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
            this.Status = RequestStatus.PENDING;
        }

    }
}
