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
        public IDictionary<RoomType, RoomInitInfo> RoomsInfoMap { get; set; }
        public int DaysCount { get; set; }

        public ExperimentParameters(IDictionary<RoomType, RoomInitInfo> roomsInfoMap, int daysCount)
        {
            this.RoomsInfoMap = roomsInfoMap;
            this.DaysCount = daysCount;
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
