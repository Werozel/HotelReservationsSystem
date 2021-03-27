using Hotels.Pages.ExperimentPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotels.Models
{

    class ExperimentConfig
    {
        public static readonly int MAX_REQUESTS_PER_STEP = 5;
        public static readonly int MIN_DAYS_TO_STAY = 1;
        public static readonly int MAX_DAYS_TO_STAY = 14;
    }


    class Experiment
    {

        private int DaysCount;
        private readonly IDictionary<RoomType, RoomInitInfo> RoomsInfoMap;

        private Random Rand = new Random();

        private readonly int MaxRoomTypeInt = Enum.GetValues(typeof(RoomType)).Cast<int>().Max();
        private readonly int MaxRquestTypeInt = Enum.GetValues(typeof(RequestType)).Cast<int>().Max();

        private readonly DateTime CurrentDateTime;
        private readonly DateTime StartDateTime;
        private readonly DateTime EndDateTime;

        Experiment(int daysCount, IDictionary<RoomType, RoomInitInfo> roomInfoMap, TimeRange experimentTimeRange)
        {
            this.DaysCount = daysCount;
            this.RoomsInfoMap = roomInfoMap;
            this.StartDateTime = experimentTimeRange.Start;
            this.EndDateTime = experimentTimeRange.End;
            this.CurrentDateTime = this.StartDateTime;
        }

        public IList<Request> Step()
        {

            IList<Request> requestList = new List<Request>();

            for (int requestsCount = Rand.Next(0, 5); requestsCount > 0; requestsCount++)
            {
                RequestType requestType = (RequestType)Rand.Next(0, MaxRquestTypeInt);
                RoomType roomType = (RoomType)Rand.Next(0, MaxRoomTypeInt);

                Request request;
                switch (requestType)
                {
                    case RequestType.BOOK:

                        DateTime randomStartDateTime = getRandomValidDateTime(CurrentDateTime, EndDateTime);
                        DateTime randomEndDateTime = getRandomValidDateTime(randomStartDateTime, EndDateTime);
                        TimeRange bookTimeRange = new TimeRange(randomStartDateTime, randomEndDateTime);

                        request = new Request(requestType, roomType, bookTimeRange);
                        break;
                    case RequestType.IMMEDIATE:

                        int lengthDays = Rand.Next(ExperimentConfig.MIN_DAYS_TO_STAY, ExperimentConfig.MAX_DAYS_TO_STAY + 1);

                        request = new Request(requestType, roomType, lengthDays);
                        break;
                    default:
                        throw new Exception(String.Format("Unknown request Type: %s", requestType));
                }
                requestList.Add(request);
            }
            return requestList;

        }

        // FIXME: Only days, not minutes
        private DateTime getRandomValidDateTime(DateTime start, DateTime end)
        {
            TimeSpan timeSpan = end - start;
            TimeSpan newSpan = new TimeSpan(Rand.Next(1, (int)timeSpan.TotalDays), 0, 0, 0);
            DateTime newDate = start + newSpan;
            return newDate;
        }
    }
}
