using System.Collections.Generic;

namespace Hotels.Models.Experiments
{
    internal class Statistics
    {
        public int TotalRequestCount;
        public int RequestsAcceptedCount;
        public int RequestsDiscounted;
        public int RequestsRejectedCount;
        public double MissedProfit;
        private readonly IDictionary<Rooms.RoomType, int> _requestsPerRoomTypeMap;

        public Statistics(
            int totalRequestsCount,
            int requestsAcceptedCount,
            int requestsDiscounted,
            int requestsRejectedCount,
            double missedProfit
        )
        {
            TotalRequestCount = totalRequestsCount;
            RequestsAcceptedCount = requestsAcceptedCount;
            RequestsDiscounted = requestsDiscounted;
            RequestsRejectedCount = requestsRejectedCount;
            MissedProfit = missedProfit;
            _requestsPerRoomTypeMap = new Dictionary<Rooms.RoomType, int>();
        }

        public void IncRequestCountWithType(Rooms.RoomType roomType)
        {
            var hasValue = _requestsPerRoomTypeMap.TryGetValue(roomType, out var oldValue);
            int newValue;
            if (hasValue)
            {
                newValue = oldValue + 1;
            } else
            {
                newValue = 1;
            }
            _requestsPerRoomTypeMap[roomType] = newValue;
        }

        public override string ToString()
        {
            var res = "Всего заявок: " + TotalRequestCount + "\n" +
                      "Одобрено: " + RequestsAcceptedCount + "\n" +
                      "Отклонено: " + RequestsRejectedCount + "\n" +
                      "Скидок: " + RequestsDiscounted + "\n" +
                      "Потерянная прибыль: " + MissedProfit + "\n\n";

            res += "Количество заявок по типу номера:\n";
            var currentRoomType = Rooms.RoomType.SINGLE;
            for (var i = 0; i < 5; i++, currentRoomType++)
            {
                var hasValue = _requestsPerRoomTypeMap.TryGetValue(currentRoomType, out int value);
                if (hasValue)
                {
                    res += Rooms.RoomTypeHelper.RoomTypeToString(currentRoomType) + ": " + value + "\n";
                } else
                {
                    res += Rooms.RoomTypeHelper.RoomTypeToString(currentRoomType) + ": 0\n";
                }
            }
            return res;
        }
    }
}
