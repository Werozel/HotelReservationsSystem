using Hotels.Models;
using System.Collections.Generic;
using Windows.UI.Xaml.Media;

namespace Hotels.Pages.ExperimentPage.Cells
{

    internal class RoomCell
    {
        public string Number { get; }
        private readonly bool _isAvailable;
        public List<TimeRange> BookedTimes { get; }
        public int RequestsCount { get; }
        public string OccupancyText { get; }

        public RoomCell(string number, bool isAvailable, List<TimeRange> bookedTimes, int requestCount, string occupancyText)
        {
            Number = number;
            _isAvailable = isAvailable;
            BookedTimes = bookedTimes;
            RequestsCount = requestCount;
            OccupancyText = occupancyText;
        }

        public SolidColorBrush GetBackgroundBrush()
        {
            return _isAvailable
                ? new SolidColorBrush(Constants.Colors.Green)
                : new SolidColorBrush(Constants.Colors.Red);
        }
    }
}
