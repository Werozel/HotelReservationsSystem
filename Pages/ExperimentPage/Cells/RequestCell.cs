using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace Hotels.Pages.ExperimentPage.Cells
{
    class RequestCell
    {
        public string RoomType { get; }
        public string TimeRange { get; }
        public bool IsApproved { get; }
        public string RoomNumber { get; set; }
        public string RequestType { get; }

        public RequestCell(string roomType, string timeRange, bool isApproved, string roomNumber, string requestType)
        {
            this.RoomType = roomType;
            this.TimeRange = timeRange;
            this.IsApproved = isApproved;
            this.RoomNumber = roomNumber;
            this.RequestType = requestType;
        }

        public string FormatRoomNumber()
        {
            return RoomNumber ?? "";
        }

        public SolidColorBrush GetBackgroundBrush()
        {
            return IsApproved
                ? new SolidColorBrush(Constants.Colors.GREED)
                : new SolidColorBrush(Constants.Colors.RED);
        }
    }
}
