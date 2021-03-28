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
using Hotels.Models;

namespace Hotels.Pages.ExperimentPage
{

    public class RequestCell
    {
        public string RoomType { get; set; }
        public string TimeRange { get; set; }
        public bool IsApproved { get; set; }

        public RequestCell(string roomType, string timeRange, bool isApproved)
        {
            this.RoomType = roomType;
            this.TimeRange = timeRange;
            this.IsApproved = isApproved;
        }
    }

    public sealed partial class ExperimentPage : Page
    {

        // private Experiment experiment;

        public IDictionary<RoomType, RoomInitInfo> RoomsInfoMap;
        public int DaysCount;

        public ObservableCollection<RequestCell> RequestCells = new ObservableCollection<RequestCell>();

        public ExperimentPage()
        {
            this.InitializeComponent();

            ListView requestsListView = this.FindName("RequestsListView") as ListView;
            requestsListView.ItemsSource = RequestCells;

            /* DateTime today = DateTime.Today;
            DateTime experimentEndTime = today.AddDays(this.DaysCount);
            experiment = new Experiment(
                this.RoomsInfoMap, 
                new TimeRange(today, experimentEndTime),
                5 // TODO: Change from init
            ); */



        }

        // Executed before constructor
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ExperimentParameters parameters = e.Parameter as ExperimentParameters;
            this.DaysCount = parameters.DaysCount;
            this.RoomsInfoMap = parameters.RoomsInfoMap;

            // TODO: Remove
            RequestCells.Add(new RequestCell("Text1", "Text2", false));
        }
    }
}
