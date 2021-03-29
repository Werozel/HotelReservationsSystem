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
        public int RequestsRejectedCount;
        public int MissedProfit;

        public Statistics(
            int totalRequestsCount,
            int requestsAcceptedCount,
            int requestsRejectedCount,
            int missedProfit
        )
        {
            this.TotalRequestCount = totalRequestsCount;
            this.RequestsAcceptedCount = requestsAcceptedCount;
            this.RequestsRejectedCount = requestsRejectedCount;
            this.MissedProfit = missedProfit;
        }

        public override string ToString()
        {
            return "Всего заявок: " + this.TotalRequestCount + "\n" +
                "Одобрено: " + this.RequestsAcceptedCount + "\n" +
                "Отклонено: " + this.RequestsRejectedCount + "\n" +
                "Потерянная прибыль: " + this.MissedProfit + "\n";
        }
    }
}
