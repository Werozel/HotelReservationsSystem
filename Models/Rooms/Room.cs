using System;
using System.Collections.Generic;
using System.Linq;

namespace Hotels.Models.Rooms
{
    internal class Room
    {
        public string Number { get; }
        public RoomType RoomType { get; }
        public int Price { get; }
        public List<TimeRange> BookedTimes { get; }

        public Room(string number, RoomType roomType, int price)
        {
            Number = number;
            RoomType = roomType;
            Price = price;
            BookedTimes = new List<TimeRange>();
        }

        public bool Book(TimeRange timeToBook)
        {
            if (BookedTimes.Any(timeToBook.Intersects))
            {
                return false;
            }
            BookedTimes.Add(timeToBook);
            return true;
        }

        public bool IsFree(DateTime dateTime)
        {
            return BookedTimes.All(time => !time.Contains(dateTime));
        }

        public int GetOccupancyInPeriod(TimeRange timeRange)
        {
            var periodTotalHours = (timeRange.End - timeRange.Start).TotalHours;
            var bookedTotalHours = 
                BookedTimes.Sum(bookedTimeRange => (bookedTimeRange.End - bookedTimeRange.Start).TotalHours);
            return Convert.ToInt32(Math.Round(bookedTotalHours / periodTotalHours * 100)); 
        }

        public int GetRequestsCount()
        {
            return BookedTimes.Count;
        }
    }
}
