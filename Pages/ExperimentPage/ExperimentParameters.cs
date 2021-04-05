using System.Collections.Generic;
using Hotels.Models.Rooms;

namespace Hotels.Pages.ExperimentPage
{
    internal class ExperimentParameters
    {
        public IDictionary<RoomType, RoomInitInfo> RoomsInfoMap { get; }
        public int DaysCount { get; }
        public int HoursPerStep { get; }
        public int MaxDaysToBook { get; }
        public int MaxHoursUntilRequest { get; }
        public double Discount { get; }

        public ExperimentParameters(
            IDictionary<RoomType, RoomInitInfo> roomsInfoMap, 
            int daysCount,
            int maxHoursPerStep,
            int maxHoursUntilRequest,
            int maxDaysToBook,
            double discount
        )
        {
            RoomsInfoMap = roomsInfoMap;
            DaysCount = daysCount;
            HoursPerStep = maxHoursPerStep;
            MaxHoursUntilRequest = maxHoursUntilRequest;
            MaxDaysToBook = maxDaysToBook;
            Discount = discount;
        }
    }

    public class RoomInitInfo
    {
        public int Count { get; }
        public int Price { get; }

        public RoomInitInfo(int count, int price)
        {
            Count = count;
            Price = price;
        }
    }
}
