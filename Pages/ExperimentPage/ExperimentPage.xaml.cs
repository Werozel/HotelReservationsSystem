using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;

namespace Hotels.Pages.ExperimentPage
{

    public class Parameters
    {
        public int RoomsCount { get; set; }
        public int DaysCount { get; set; }

        public Parameters(int roomsCount, int daysCount)
        {
            this.RoomsCount = roomsCount;
            this.DaysCount = daysCount;
        }
    }

    public class RequestCell
    {
        public string RoomType { get; set; }
        public string TimeRange { get; set; }

        public RequestCell(string roomType, string timeRange)
        {
            this.RoomType = roomType;
            this.TimeRange = timeRange;
        }
    }

    public sealed partial class ExperimentPage : Page
    {

        public int RoomsCount { get; set; }
        public int DaysCount { get; set; }

        public ObservableCollection<RequestCell> RequestCells { get; } = new ObservableCollection<RequestCell>();

        public ExperimentPage()
        {
            this.InitializeComponent();

            ListView requestsListView = this.FindName("RequestsListView") as ListView;
            requestsListView.ItemsSource = RequestCells;

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Parameters parameters = e.Parameter as Parameters;
            this.RoomsCount = parameters.RoomsCount;
            this.DaysCount = parameters.DaysCount;

            RequestCells.Add(new RequestCell("Text1", "Text2"));
        }

        private async void TaskLogic()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {

            });
        }
    }
}
