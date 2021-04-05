using Hotels.Models.Requests;
using Hotels.Models.Rooms;
using Hotels.Pages.ExperimentPage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hotels.Models.Experiments
{

    class Experiment
    {
        private readonly IDictionary<RoomType, RoomInitInfo> _roomsInfoMap;

        private readonly Random _rand = new Random();

        public Statistics Statistics { get; } = new Statistics(0, 0, 0, 0, 0); 

        private static readonly Array RoomTypeValues = Enum.GetValues(typeof(RoomType));
        private static readonly int MaxRoomTypeInt = Enum.GetValues(typeof(RoomType)).Cast<int>().Max();
        private static readonly int MaxRequestTypeInt = Enum.GetValues(typeof(RequestType)).Cast<int>().Max();

        public int CurrentStep;
        private readonly int _hoursPerStep;
        private readonly double _discount;
        private readonly int _maxHoursUntilRequest;
        public DateTime CurrentDateTime { get; private set; }
        public DateTime StartDateTime { get; }
        public DateTime EndDateTime { get; }

        public List<Request> RequestList { get; } = new List<Request>();
        public readonly Hotel Hotel = new Hotel(new List<Room>());

        private readonly int _maxDaysToBook;

        public Experiment(
            IDictionary<RoomType, RoomInitInfo> roomInfoMap, 
            TimeRange experimentTimeRange, 
            int maxHoursPerStep, 
            int maxHoursUntilRequest, 
            int maxDaysToBook, 
            double discount
        )
        {
            _roomsInfoMap = roomInfoMap;
            StartDateTime = experimentTimeRange.Start;
            EndDateTime = experimentTimeRange.End;
            CurrentDateTime = StartDateTime;
            _hoursPerStep = maxHoursPerStep;
            _maxDaysToBook = maxDaysToBook;
            _maxHoursUntilRequest = maxHoursUntilRequest;
            _discount = discount;

            foreach (RoomType roomType in RoomTypeValues)
            {
                var roomInitValueFound = roomInfoMap.TryGetValue(roomType, out RoomInitInfo roomInitInfo);
                if (!roomInitValueFound)
                {
                    throw new Exception("Unknown RoomType: " + roomType);
                }

                for (var i = 0; i < roomInitInfo.Count; i++)
                {
                    var room = new Room(
                        RoomTypeHelper.RoomTypeToRoomNumber(roomType, i),
                        roomType,
                        roomInitInfo.Price
                    );
                    Hotel.Rooms.Add(room);
                }
            }
        }

        public void ToTheEnd()
        {
            while (Step())
            {
            }
        }

        public bool Step()
        {
            if (CurrentDateTime >= EndDateTime)
            {
                return false;
            }
            ++CurrentStep;
            var nextStepDateTime = CurrentDateTime.AddHours(_hoursPerStep);
            if (nextStepDateTime > EndDateTime)
            {
                nextStepDateTime = EndDateTime;
            }
            
            var maxLimitDayTime = nextStepDateTime.AddHours(-_maxHoursUntilRequest);
            while (CurrentDateTime < maxLimitDayTime)
            {
                var hoursPassed = _rand.Next(1, _maxHoursUntilRequest);
                CurrentDateTime = CurrentDateTime.AddHours(Convert.ToDouble(hoursPassed));

                GenerateRequest();
            }
            CurrentDateTime = nextStepDateTime;
            GenerateRequest();

            return true;
        }

        private void GenerateRequest()
        {
            var requestType = (RequestType)_rand.Next(0, MaxRequestTypeInt + 1);
            var roomType = (RoomType)_rand.Next(0, MaxRoomTypeInt + 1);

            Request request;
            switch (requestType)
            {
                case RequestType.BOOK:

                    var offsetEndDate = EndDateTime.AddDays(ExperimentConfig.ValidDaysAfterEnding).Date;
                    var randomStartDateTime = GetRandomValidDateTime(CurrentDateTime.Date, offsetEndDate);
                    var randomEndDateTime = GetRandomValidDateTime(
                        randomStartDateTime.Date,
                        offsetEndDate
                    );

                    var bookTimeRange = new TimeRange(
                        randomStartDateTime.Date,
                        randomEndDateTime.Date
                    );

                    request = new Request(requestType, roomType, bookTimeRange, CurrentStep, CurrentDateTime);
                    break;
                case RequestType.IMMEDIATE:

                    var lengthDays = _rand.Next(1, _maxDaysToBook + 1);

                    var start = CurrentDateTime.Date;
                    var end = start.AddDays(lengthDays);
                    var immediateBookTimeRange = new TimeRange(start, end);

                    request = new Request(requestType, roomType, immediateBookTimeRange, CurrentStep, CurrentDateTime);
                    break;
                default:
                    throw new Exception($"Unknown request Type: {requestType}");
            }
            Statistics.TotalRequestCount++;
            var bookedRoom = Hotel.Book(request);
            if (bookedRoom != null)
            {
                Statistics.RequestsAcceptedCount++;
                request.RoomNumber = bookedRoom.Number;
                request.Price = bookedRoom.Price;
            }
            else
            {
                Statistics.RequestsRejectedCount++;
                var gotRoomInfo = _roomsInfoMap.TryGetValue(roomType, out RoomInitInfo roomInfo);
                double roomTypeProfit = 0;
                if (gotRoomInfo)
                {
                    roomTypeProfit = roomInfo.Price;
                }

                if (roomType == RoomType.SUITE)
                {
                    RequestList.Add(request);
                    Statistics.IncRequestCountWithType(request.RoomType);
                    Statistics.MissedProfit += roomTypeProfit;
                    return;
                }

                request.HasDiscount = true;
                var discountRoomType = roomType + 1;
                request.DiscountRoomType = discountRoomType;

                var gotDiscountRoomInfo = _roomsInfoMap.TryGetValue(discountRoomType, out RoomInitInfo discountRoomInfo);
                double discountRoomTypeProfit = 0;
                if (gotDiscountRoomInfo)
                {
                    discountRoomTypeProfit = (1 - _discount) * discountRoomInfo.Price;
                }

                var discountBookedRoom = Hotel.Book(request);
                if (discountBookedRoom != null)
                {
                    Statistics.RequestsAcceptedCount++;
                    request.RoomNumber = discountBookedRoom.Number;
                    request.Price = discountRoomTypeProfit;
                    Statistics.MissedProfit += Math.Max(roomTypeProfit - discountRoomTypeProfit, 0);
                    Statistics.RequestsDiscounted += 1;
                }
                else
                {
                    Statistics.RequestsRejectedCount++;
                    Statistics.MissedProfit += roomTypeProfit;
                }

            }
            Statistics.IncRequestCountWithType(request.RoomType);
            RequestList.Add(request);
        }
        
        private DateTime GetRandomValidDateTime(DateTime start, DateTime end)
        {
            var timeSpan = end.Date - start.Date;
            var totalDays = (int) timeSpan.TotalDays;
            var newSpan = new TimeSpan(_rand.Next(1, totalDays), 0, 0, 0);
            var newDate = start + newSpan;
            return newDate;
        }
    }
}
