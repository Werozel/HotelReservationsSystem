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

    }
}
