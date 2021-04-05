using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotels.Models.Requests
{
    static class RequestTypeHelper
    {

        public static string RequestTypeToString(RequestType requestType)
        {
            switch (requestType)
            {
                case RequestType.BOOK:
                    return "Бронь";
                case RequestType.IMMEDIATE:
                    return "Заселение";
                default:
                    throw new Exception("RequestTypeToString: Unknown request type: " + requestType);
            }
        }
    }
}
