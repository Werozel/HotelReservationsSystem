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

        public RequestType RequestType { get; set; }
        public TimeRange TimeRange { get; set; }

        public Request(RequestType type, TimeRange timeRange)
        {
            this.RequestType = type;
            this.TimeRange = timeRange;
        }

        public Request(RequestType type, int length)
        {
            if (type != RequestType.IMMEDIATE)
            {
                throw new Exception("type can only be RequestType.IMMEDIATE in this constructor");
            }
            this.RequestType = RequestType.IMMEDIATE;
            this.TimeRange = new TimeRange(length);
        }

    }
}
