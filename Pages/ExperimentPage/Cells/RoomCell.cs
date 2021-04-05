using Hotels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace Hotels.Pages.ExperimentPage.Cells
{

    class RoomCell
    {
        public string Number { get; }
        public bool IsAvaliable { get; }
        public List<TimeRange> BookedTimes { get; }
        public int RequestsCount { get; }
        public string OccupancyText { get; }

        public RoomCell(string number, bool isAvaliable, List<TimeRange> bookedTimes, int requestCount, string occupancyText)
        {
            this.Number = number;
            this.IsAvaliable = isAvaliable;
            this.BookedTimes = bookedTimes;
            this.RequestsCount = requestCount;
            this.OccupancyText = occupancyText;
        }

        public SolidColorBrush GetBackgroundBrush()
        {
            return IsAvaliable
                ? new SolidColorBrush(Constants.Colors.GREEN)
                : new SolidColorBrush(Constants.Colors.RED);
        }
    }
}
