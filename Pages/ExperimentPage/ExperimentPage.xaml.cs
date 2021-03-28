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
        public string RoomNumber { get; set; }

        public RequestCell(string roomType, string timeRange, bool isApproved, string roomNumber)
        {
            this.RoomType = roomType;
            this.TimeRange = timeRange;
            this.IsApproved = isApproved;
            this.RoomNumber = roomNumber;
        }
    }

    public sealed partial class ExperimentPage : Page
    {

        private Experiment experiment;

        private IDictionary<RoomType, RoomInitInfo> RoomsInfoMap;
        private int DaysCount;

        private ObservableCollection<RequestCell> RequestCells = new ObservableCollection<RequestCell>();
        

        public ExperimentPage()
        {
            this.InitializeComponent();

            ListView requestsListView = this.FindName("RequestsListView") as ListView;
            requestsListView.ItemsSource = RequestCells;

            (this.FindName("ExitButton") as Button).Click += (s, e) =>
            {
                Application.Current.Exit();
            };

            (this.FindName("RestartButton") as Button).Click += (s, e) =>
            {
                this.Frame.Navigate(typeof(InitPage.InitPage));
            };
            (this.FindName("StepButton") as Button).Click += (s, e) =>
            {
                experiment.Step();
                UpdateCells();
                UpdateCurrentTimeText();
                UpdateRoomsList();
                UpdateStatisticsText();
                UpdateProfitText();
            };

        }

        // Executed after constructor
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ExperimentParameters parameters = e.Parameter as ExperimentParameters;
            this.DaysCount = parameters.DaysCount;
            this.RoomsInfoMap = parameters.RoomsInfoMap;

            DateTime today = DateTime.Today;
            DateTime experimentEndTime = today.AddDays(this.DaysCount);
            experiment = new Experiment(
                this.RoomsInfoMap,
                new TimeRange(today, experimentEndTime),
                parameters.MaxHoursPerStep
            );
            
            UpdateCells();
            UpdateCurrentTimeText();
            UpdateRoomsList();
            UpdateStatisticsText();
            UpdateProfitText();
        }

        private void UpdateCells()
        {
            RequestCells.Clear();
            foreach (RequestCell cell in experiment.GetCells())
            {
                RequestCells.Add(cell);
            }
        }

        private void UpdateCurrentTimeText()
        {
            this.CurrentTimeTextBlock.Text = this.experiment.CurrentDateTime.ToString(TimeRange.FORMAT_STRING);
        }

        private void UpdateRoomsList()
        {

        }

        private void UpdateStatisticsText()
        {

        }

        private void UpdateProfitText()
        {
            this.ProfitTextBlock.Text = "Прибыль: " + this.experiment.Hotel.Profit;
        }
    }
}
