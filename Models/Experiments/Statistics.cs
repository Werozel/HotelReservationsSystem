using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotels.Models.Experiments
{
    class Statistics
    {
        public int TotalRequestCount;
        public int RequestsAcceptedCount;
        public int RequestsDiscounted;
        public int RequestsRejectedCount;
        public double MissedProfit;
        IDictionary<Rooms.RoomType, int> RequestsPerRoomTypeMap;

        public Statistics(
            int totalRequestsCount,
            int requestsAcceptedCount,
            int requestsDiscounted,
            int requestsRejectedCount,
            double missedProfit
        )
        {
            this.TotalRequestCount = totalRequestsCount;
            this.RequestsAcceptedCount = requestsAcceptedCount;
            this.RequestsDiscounted = requestsDiscounted;
            this.RequestsRejectedCount = requestsRejectedCount;
            this.MissedProfit = missedProfit;
            RequestsPerRoomTypeMap = new Dictionary<Rooms.RoomType, int>();
        }

        public void IncRequestCountWithType(Rooms.RoomType roomType)
        {
            bool hasValue = RequestsPerRoomTypeMap.TryGetValue(roomType, out int oldValue);
            int newValue;
            if (hasValue)
            {
                newValue = oldValue + 1;
            } else
            {
                newValue = 1;
            }
            RequestsPerRoomTypeMap[roomType] = newValue;
        }

        public override string ToString()
        {
            string res = "Всего заявок: " + this.TotalRequestCount + "\n" +
                "Одобрено: " + this.RequestsAcceptedCount + "\n" +
                "Отклонено: " + this.RequestsRejectedCount + "\n" +
                "Скидок: " + this.RequestsDiscounted + "\n" +
                "Потерянная прибыль: " + this.MissedProfit + "\n\n";

            res += "Количество заявок по типу номера:\n";
            foreach (Rooms.RoomType roomType in RequestsPerRoomTypeMap.Keys.ToArray())
            {
                bool hasValue = RequestsPerRoomTypeMap.TryGetValue(roomType, out int value);
                if (hasValue)
                {
                    res += Rooms.RoomTypeHelper.RoomTypeToString(roomType) + ": " + value + "\n";
                }
            }
            return res;
        }
    }
}
