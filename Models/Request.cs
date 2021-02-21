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
        NOW,
    }

    class Request
    {

        public TimeRange TimeRange { get; set; }

    }
}
