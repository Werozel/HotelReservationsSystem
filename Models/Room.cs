using System;
using System.Collections.Generic;

namespace Hotels.Models
{

    public enum RoomType
    {
        SINGLE,
        DOUBLE,
        DOUBLE_WITH_SOFA,
        JUNIOR_SUITE,
        SUITE,
    }

    class RoomTypeHelper
    {

        private RoomTypeHelper() { }

        public static string RoomTypeToRoomNumber(RoomType roomType, int roomTypeCount)
        {
            if (roomTypeCount < 0)
            {
                throw new Exception("Negative roomTypeCount!");
            }
            else if (roomTypeCount >= 100)
            {
                throw new Exception("Room type can't be >= 100");
            }
            else if (roomTypeCount < 10)
            {
                return (int)roomType + "0" + roomTypeCount;
            }
            else
            {
                return "" + (int)roomType + roomTypeCount;
            }
        }

        public static string RoomTypeToString(RoomType roomType)
        {
            switch (roomType)
            {
                case RoomType.SINGLE:
                    return "Одноместный";
                case RoomType.DOUBLE:
                    return "Двухместный";
                case RoomType.DOUBLE_WITH_SOFA:
                    return "Двухместный с диваном";
                case RoomType.JUNIOR_SUITE:
                    return "Полулюкс";
                case RoomType.SUITE:
                    return "Люкс";
                default:
                    throw new Exception("Unknown RoomType: " + roomType);
            }
        }

    }

    class Room
    {
        public static int NO_NUMBER = -1;

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

    }
}
