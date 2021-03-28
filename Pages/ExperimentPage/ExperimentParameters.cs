using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotels.Models;

namespace Hotels.Pages.ExperimentPage
{
    public class ExperimentParameters
    {
        public IDictionary<RoomType, RoomInitInfo> RoomsInfoMap { get; }
        public int DaysCount { get; }
        public int MaxHoursPerStep { get; }

        public ExperimentParameters(
            IDictionary<RoomType, RoomInitInfo> roomsInfoMap, 
            int daysCount,
            int maxHoursPerStep
        )
        {
            this.RoomsInfoMap = roomsInfoMap;
            this.DaysCount = daysCount;
            this.MaxHoursPerStep = maxHoursPerStep;
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
