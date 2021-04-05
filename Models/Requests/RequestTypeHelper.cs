using System;

namespace Hotels.Models.Requests
{
    internal static class RequestTypeHelper
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
