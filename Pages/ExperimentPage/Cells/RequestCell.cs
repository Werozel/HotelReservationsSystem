using Windows.UI.Xaml.Media;

namespace Hotels.Pages.ExperimentPage.Cells
{
    internal class RequestCell
    {
        public string RoomType { get; }
        public string TimeRange { get; }
        private bool IsApproved { get; }
        private string RoomNumber { get; }
        public string RequestType { get; }
        private bool HasDiscount { get; }
        public string Price { get; }
        public int Step { get; }
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
            int step,
            bool isCurrentStep,
            string bookTime
        )
        {
            RoomType = roomType;
            TimeRange = timeRange;
            IsApproved = isApproved;
            RoomNumber = roomNumber;
            RequestType = requestType;
            HasDiscount = hasDiscount;
            Price = price;
            Step = step;
            IsCurrentStep = isCurrentStep;
            BookTime = bookTime;
        }

        public string FormatRoomNumber()
        {
            return RoomNumber ?? "";
        }

        public SolidColorBrush GetBackgroundBrush()
        {
            if (!IsApproved)
            {
                return new SolidColorBrush(Constants.Colors.Red);
            } else if (HasDiscount)
            {
                return new SolidColorBrush(Constants.Colors.Yellow);
            } else
            {
                return new SolidColorBrush(Constants.Colors.Green);
            }
        }

        public SolidColorBrush GetBorderBrush()
        {
            return IsCurrentStep ? new SolidColorBrush(Constants.Colors.Grey) : null;
        }
    }
}
