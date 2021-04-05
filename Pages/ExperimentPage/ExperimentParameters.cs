using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotels.Models;
using Hotels.Models.Rooms;

namespace Hotels.Pages.ExperimentPage
{
    class ExperimentParameters
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
            this.RoomsInfoMap = roomsInfoMap;
            this.DaysCount = daysCount;
            this.HoursPerStep = maxHoursPerStep;
            this.MaxHoursUntilRequest = maxHoursUntilRequest;
            this.MaxDaysToBook = MaxDaysToBook;
            this.Discount = discount;
        }
    }

    public class RoomInitInfo
    {
        public int Count { get; set; }
        public int Price { get; set; }

        public RoomInitInfo(int count, int price)
        {
            this.Count = count;
            this.Price = price;
        }
    }
}
