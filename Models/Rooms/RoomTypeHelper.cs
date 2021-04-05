using System;

namespace Hotels.Models.Rooms
{
    internal static class RoomTypeHelper
    {

        public static string RoomTypeToRoomNumber(RoomType roomType, int roomTypeCount)
        {
            if (roomTypeCount < 0)
            {
                throw new Exception("Negative roomTypeCount!");
            }
            if (roomTypeCount >= 99)
            {
                throw new Exception("Room type can't be >= 99");
            }
            if (roomTypeCount < 10)
            {
                return "№" + ((int)roomType + 1) + "0" + (roomTypeCount + 1);
            }
            return "№" + ((int)roomType + 1) + (roomTypeCount + 1);
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
}
