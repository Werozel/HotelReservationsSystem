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
        public bool HasDiscount { get; }

        public RequestCell(
            string roomType, 
            string timeRange, 
            bool isApproved, 
            string roomNumber, 
            string requestType, 
            bool hasDiscount
        )
        {
            this.RoomType = roomType;
            this.TimeRange = timeRange;
            this.IsApproved = isApproved;
            this.RoomNumber = roomNumber;
            this.RequestType = requestType;
            this.HasDiscount = hasDiscount;
        }

        public string FormatRoomNumber()
        {
            return RoomNumber ?? "";
        }

        public SolidColorBrush GetBackgroundBrush()
        {
            if (this.HasDiscount)
            {
                return new SolidColorBrush(Constants.Colors.YELLOW);
            } else
            {
                return IsApproved
                    ? new SolidColorBrush(Constants.Colors.GREED)
                    : new SolidColorBrush(Constants.Colors.RED);
            }
        }
    }
}
