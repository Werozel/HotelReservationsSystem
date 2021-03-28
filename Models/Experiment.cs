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
        public static readonly int MIN_DAYS_TO_STAY = 1;
        public static readonly int MAX_DAYS_TO_STAY = 14;
    }


    class Experiment
    {
        
        private readonly IDictionary<RoomType, RoomInitInfo> RoomsInfoMap;

        private Random Rand = new Random();

        private static readonly Array RoomTypeValues = Enum.GetValues(typeof(RoomType));
        private static readonly int MaxRoomTypeInt = Enum.GetValues(typeof(RoomType)).Cast<int>().Max();
        private static readonly int MaxRequestTypeInt = Enum.GetValues(typeof(RequestType)).Cast<int>().Max();

        private readonly int MaxHoursPerStep;
        private DateTime CurrentDateTime;
        private readonly DateTime StartDateTime;
        private readonly DateTime EndDateTime;

        public IList<Request> RequestList { get; } = new List<Request>();
        public Hotel hotel = new Hotel(new List<Room>());

        public Experiment(IDictionary<RoomType, RoomInitInfo> roomInfoMap, TimeRange experimentTimeRange, int maxHoursPerStep)
        {
            this.RoomsInfoMap = roomInfoMap;
            this.StartDateTime = experimentTimeRange.Start;
            this.EndDateTime = experimentTimeRange.End;
            this.CurrentDateTime = this.StartDateTime;
            this.MaxHoursPerStep = maxHoursPerStep;

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
                    hotel.Rooms.Add(room);
                }
            }
        }

        public IList<RequestCell> GetCells() { 
            return RequestList
                .Select(request =>
                    new RequestCell(
                        RoomTypeHelper.RoomTypeToString(request.RoomType),
                        request.TimeRange.ToCellString(),
                        request.IsApproved()
                    )
                )
                .ToList();
        }

        public void Step()
        {
            int hoursPassed = Rand.Next(1, this.MaxHoursPerStep);
            CurrentDateTime = CurrentDateTime.AddHours(Convert.ToDouble(hoursPassed));
            RequestType requestType = (RequestType)Rand.Next(0, MaxRequestTypeInt);
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
            Room bookedRoom = hotel.Book(request);
            request.RoomNumber = bookedRoom.Number;
            this.RequestList.Add(request);
        }
        
        private DateTime getRandomValidDateTime(DateTime start, DateTime end)
        {
            TimeSpan timeSpan = end - start;
            TimeSpan newSpan = new TimeSpan(Rand.Next(1, (int)timeSpan.TotalDays), 0, 0, 0);
            DateTime newDate = start + newSpan;
            return newDate;
        }
    }
}
