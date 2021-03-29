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
        public IList<TimeRange> BookedTimes { get; }

        public RoomCell(string number, bool isAvaliable, IList<TimeRange> bookedTimes)
        {
            this.Number = number;
            this.IsAvaliable = isAvaliable;
            this.BookedTimes = bookedTimes;
        }

        public SolidColorBrush GetBackgroundBrush()
        {
            return IsAvaliable
                ? new SolidColorBrush(Constants.Colors.GREEN)
                : new SolidColorBrush(Constants.Colors.RED);
        }
    }
}
