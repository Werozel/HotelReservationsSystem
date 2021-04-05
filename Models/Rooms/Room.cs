using Hotels.Models.Rooms;
using System;
using System.Collections.Generic;

namespace Hotels.Models.Rooms
{
    class Room
    {
        public string Number { get; set; }
        public RoomType RoomType { get; set; }
        public int Price { get; set; }
        public List<TimeRange> BookedTimes { get; set; }

        public Room(string number, RoomType roomType, int price)
        {
            this.Number = number;
            this.RoomType = roomType;
            this.Price = price;
            this.BookedTimes = new List<TimeRange>();
        }

        public bool Book(TimeRange timeToBook)
        {
            foreach (TimeRange time in BookedTimes)
            {
                if (timeToBook.Intersects(time))
                {
                    return false;
                }
            }
            BookedTimes.Add(timeToBook);
            return true;
        }

        public bool IsFree(DateTime dateTime)
        {
            foreach (TimeRange time in BookedTimes)
            {
                if (time.Contains(dateTime))
                {
                    return false;
                }
            }
            return true;
        }

        public int GetOccupancyInPeriod(TimeRange timeRange)
        {
            double periodTotalHours = (timeRange.End - timeRange.Start).TotalHours;
            double bookedTotalHours = 0;
            foreach (TimeRange bookedTimeRange in this.BookedTimes)
            {
                bookedTotalHours += (bookedTimeRange.End - bookedTimeRange.Start).TotalHours;
            }
            return Convert.ToInt32(Math.Round(bookedTotalHours / periodTotalHours * 100)) ; 
        }

        public int GetRequestsCount()
        {
            return this.BookedTimes.Count;
        }

    }
}
