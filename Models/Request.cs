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

    static class RequestTypeHelper
    {

        public static string RequestTypeToString(RequestType requestType)
        {
            switch (requestType)
            {
                case RequestType.BOOK:
                    return "Бронирование";
                case RequestType.IMMEDIATE:
                    return "Поселение";
                default:
                    throw new Exception("RequestTypeToString: Unknown request type: " + requestType);
            }
        }
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

        public bool IsApproved()
        {
            return RoomNumber != null;
        }
    }
}
