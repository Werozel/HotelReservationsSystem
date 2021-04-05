using Hotels.Models.Experiment;
using Hotels.Models.Requests;
using Hotels.Models.Rooms;
using Hotels.Pages.ExperimentPage;
using Hotels.Pages.ExperimentPage.Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotels.Models.Experiments
{

    class Experiment
    {
        
        private readonly IDictionary<RoomType, RoomInitInfo> RoomsInfoMap;

        private Random Rand = new Random();

        public Statistics Statistics { get; } = new Statistics(0, 0, 0, 0); 

        private static readonly Array RoomTypeValues = Enum.GetValues(typeof(RoomType));
        private static readonly int MaxRoomTypeInt = Enum.GetValues(typeof(RoomType)).Cast<int>().Max();
        private static readonly int MaxRequestTypeInt = Enum.GetValues(typeof(RequestType)).Cast<int>().Max();

        private readonly int HoursPerStep;
        private readonly double Discount;
        private readonly int MaxHoursUntilRequest;
        public DateTime CurrentDateTime { get; private set; }
        private readonly DateTime StartDateTime;
        public DateTime EndDateTime { get; private set; }

        public List<Request> RequestList { get; } = new List<Request>();
        public Hotel Hotel = new Hotel(new List<Room>());

        private readonly int MaxDaysToBook;

        public Experiment(IDictionary<RoomType, RoomInitInfo> roomInfoMap, TimeRange experimentTimeRange, int maxHoursPerStep, int maxHoursUntilRequest, int maxDaysToBook, double discount)
        {
            this.RoomsInfoMap = roomInfoMap;
            this.StartDateTime = experimentTimeRange.Start;
            this.EndDateTime = experimentTimeRange.End;
            this.CurrentDateTime = this.StartDateTime;
            this.HoursPerStep = maxHoursPerStep;
            this.MaxDaysToBook = maxDaysToBook;
            this.MaxHoursUntilRequest = maxHoursUntilRequest;
            this.Discount = discount;

            foreach (RoomType roomType in RoomTypeValues)
            {
                bool roomInitValueFound = roomInfoMap.TryGetValue(roomType, out RoomInitInfo roomInitInfo);
                if (!roomInitValueFound)
                {
                    throw new Exception("Unknown RoomType: " + roomType);
                }

                for (int i = 0; i < roomInitInfo.Count; i++)
                {
                    Room room = new Room(
                        RoomTypeHelper.RoomTypeToRoomNumber(roomType, i),
                        roomType,
                        roomInitInfo.Price
                    );
                    Hotel.Rooms.Add(room);
                }
            }
        }

        public bool Step()
        {
            if (CurrentDateTime >= EndDateTime)
            {
                return false;
            }
            DateTime nextStepDateTime = CurrentDateTime.AddHours(HoursPerStep);
            if (nextStepDateTime > EndDateTime)
            {
                nextStepDateTime = EndDateTime;
            }
            DateTime maxLimitDayTime = nextStepDateTime.AddHours(-MaxHoursUntilRequest);
            while (CurrentDateTime < maxLimitDayTime)
            {
                int hoursPassed = Rand.Next(1, this.MaxHoursUntilRequest);
                CurrentDateTime = CurrentDateTime.AddHours(Convert.ToDouble(hoursPassed));

                GenerateRequest();
            }
            CurrentDateTime = nextStepDateTime;
            GenerateRequest();

            return true;
        }

        private void GenerateRequest()
        {
            RequestType requestType = (RequestType)Rand.Next(0, MaxRequestTypeInt + 1);
            RoomType roomType = (RoomType)Rand.Next(0, MaxRoomTypeInt + 1);

            Request request;
            switch (requestType)
            {
                case RequestType.BOOK:

                    DateTime offsetEndDate = EndDateTime.AddDays(ExperimentConfig.VALID_DAYS_AFTER_ENDING).Date;
                    DateTime randomStartDateTime = GetRandomValidDateTime(CurrentDateTime.Date, offsetEndDate);
                    DateTime randomEndDateTime = GetRandomValidDateTime(
                        randomStartDateTime.Date,
                        offsetEndDate
                    );

                    TimeRange bookTimeRange = new TimeRange(
                        randomStartDateTime.Date,
                        randomEndDateTime.Date
                    );

                    request = new Request(requestType, roomType, bookTimeRange);
                    break;
                case RequestType.IMMEDIATE:

                    int lengthDays = Rand.Next(1, MaxDaysToBook + 1);

                    DateTime start = CurrentDateTime.Date;
                    DateTime end = start.AddDays(lengthDays);
                    TimeRange immediateBookTimeRange = new TimeRange(start, end);

                    request = new Request(requestType, roomType, immediateBookTimeRange);
                    break;
                default:
                    throw new Exception(String.Format("Unknown request Type: %s", requestType));
            }
            Statistics.TotalRequestCount++;
            Room bookedRoom = Hotel.Book(request);
            if (bookedRoom != null)
            {
                Statistics.RequestsAcceptedCount++;
                request.RoomNumber = bookedRoom.Number;
                request.Price = bookedRoom.Price;
            }
            else
            {
                Statistics.RequestsRejectedCount++;
                bool gotRoomInfo = RoomsInfoMap.TryGetValue(roomType, out RoomInitInfo roomInfo);
                double roomTypeProfit = 0;
                if (gotRoomInfo)
                {
                    roomTypeProfit = roomInfo.Price;
                }

                if (roomType == RoomType.SUITE)
                {
                    this.RequestList.Add(request);
                    Statistics.MissedProfit += roomTypeProfit;
                    return;
                }

                request.HasDiscount = true;
                var discountRoomType = roomType + 1;
                request.DiscountRoomType = discountRoomType;

                bool gotDiscountRoomInfo = RoomsInfoMap.TryGetValue(discountRoomType, out RoomInitInfo discountRoomInfo);
                double discountRoomTypeProfit = 0;
                if (gotDiscountRoomInfo)
                {
                    discountRoomTypeProfit = (1 - this.Discount) * discountRoomInfo.Price;
                }

                Room discountBookedRoom = Hotel.Book(request);
                if (discountBookedRoom != null)
                {
                    Statistics.RequestsAcceptedCount++;
                    request.RoomNumber = discountBookedRoom.Number;
                    request.Price = discountRoomTypeProfit;
                    Statistics.MissedProfit += Math.Max(roomTypeProfit - discountRoomTypeProfit, 0);
                }
                else
                {
                    Statistics.RequestsRejectedCount++;
                    Statistics.MissedProfit += roomTypeProfit;
                }

            }
            this.RequestList.Add(request);
        }
        
        private DateTime GetRandomValidDateTime(DateTime start, DateTime end)
        {
            TimeSpan timeSpan = end.Date - start.Date;
            int totalDays = (int) timeSpan.TotalDays;
            TimeSpan newSpan = new TimeSpan(Rand.Next(1, totalDays), 0, 0, 0);
            DateTime newDate = start + newSpan;
            return newDate;
        }
    }
}
