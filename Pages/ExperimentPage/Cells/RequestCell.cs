using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private bool IsApproved { get; }
        private string RoomNumber { get; }
        public string RequestType { get; }
        private bool HasDiscount { get; }
        public string Price { get; }
        private bool IsCurrentStep { get; }
        public string BookTime { get;  }

        public RequestCell(
            string roomType, 
            string timeRange, 
            bool isApproved, 
            string roomNumber, 
            string requestType, 
            bool hasDiscount,
            string price,
            bool isCurrentStep,
            string bookTime
        )
        {
            this.RoomType = roomType;
            this.TimeRange = timeRange;
            this.IsApproved = isApproved;
            this.RoomNumber = roomNumber;
            this.RequestType = requestType;
            this.HasDiscount = hasDiscount;
            this.Price = price;
            this.IsCurrentStep = isCurrentStep;
            this.BookTime = bookTime;
        }

        public string FormatRoomNumber()
        {
            return RoomNumber ?? "";
        }

        public SolidColorBrush GetBackgroundBrush()
        {
            if (!this.IsApproved)
            {
                return new SolidColorBrush(Constants.Colors.RED);
            } else if (this.HasDiscount)
            {
                return new SolidColorBrush(Constants.Colors.YELLOW);
            } else
            {
                return new SolidColorBrush(Constants.Colors.GREEN);
            }
        }

        public SolidColorBrush GetBorderBrush()
        {
            if (this.IsCurrentStep)
            {
                return new SolidColorBrush(Constants.Colors.GREY);
            } else
            {
                return null;
            }
        }
    }
}
