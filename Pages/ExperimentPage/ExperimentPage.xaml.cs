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
using Windows.UI;

namespace Hotels.Pages.ExperimentPage
{

    public static class Colors
    {
        public static Color RED = Color.FromArgb(255, 248, 206, 204);
        public static Color GREED = Color.FromArgb(255, 213, 232, 212);
    }


    public class RequestCell
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
                ? new SolidColorBrush(Colors.GREED)
                : new SolidColorBrush(Colors.RED);
        }
    }

    public class RoomCell
    {
        public string Number { get; }
        public bool IsAvaliable { get; }

        public RoomCell(string number, bool isAvaliable)
        {
            this.Number = number;
            this.IsAvaliable = isAvaliable;
        }

        public SolidColorBrush GetBackgroundBrush()
        {
            return IsAvaliable
                ? new SolidColorBrush(Colors.GREED)
                : new SolidColorBrush(Colors.RED);
        }
    }

    public sealed partial class ExperimentPage : Page
    {

        private Experiment experiment;
        private bool IsExperimentEnded;

        private IDictionary<RoomType, RoomInitInfo> RoomsInfoMap;
        private int DaysCount;

        private ObservableCollection<RequestCell> RequestCells = new ObservableCollection<RequestCell>();

        private ObservableCollection<RoomCell> SingleRoomCells = new ObservableCollection<RoomCell>();
        private ObservableCollection<RoomCell> DoubleRoomCells = new ObservableCollection<RoomCell>();
        private ObservableCollection<RoomCell> DoubleWithSofaRoomCells = new ObservableCollection<RoomCell>();
        private ObservableCollection<RoomCell> JuniorSuiteRoomCells = new ObservableCollection<RoomCell>();
        private ObservableCollection<RoomCell> SuiteRoomCells = new ObservableCollection<RoomCell>();

        public ExperimentPage()
        {
            this.InitializeComponent();

            ListView requestsListView = this.FindName("RequestsListView") as ListView;
            requestsListView.ItemsSource = RequestCells;

            this.SingleRoomsList.ItemsSource = SingleRoomCells;
            this.DoubleRoomsList.ItemsSource = DoubleRoomCells;
            this.DoubleWithSofaRoomsList.ItemsSource = DoubleWithSofaRoomCells;
            this.JuniorSuiteRoomsList.ItemsSource = JuniorSuiteRoomCells;
            this.SuiteRoomsList.ItemsSource = SuiteRoomCells;

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
                if (IsExperimentEnded)
                {
                    ToExperimentEndedState();
                    return;
                }
                IsExperimentEnded = !experiment.Step();
                UpdateCells();
                UpdateCurrentTimeText();
                UpdateRoomsList(experiment.Hotel, experiment.CurrentDateTime);
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
                parameters.MaxHoursPerStep,
                parameters.MaxDaysToBook
            );

            UpdateCells();
            UpdateCurrentTimeText();
            UpdateRoomsList(experiment.Hotel, experiment.CurrentDateTime);
            UpdateStatisticsText();
            UpdateProfitText();
        }

        private void UpdateCells()
        {
            RequestCells.Clear();
            var cells = experiment.GetCells();
            foreach (RequestCell cell in cells)
            {
                RequestCells.Add(cell);
            }
            var listItems = RequestsListView.Items;
            var listItemsCount = listItems.Count;
            if (listItemsCount != 0)
            {
                RequestsListView.ScrollIntoView(listItems[listItemsCount - 1]);

            }
        }

        private void UpdateCurrentTimeText()
        {
            this.CurrentTimeTextBlock.Text = this.experiment.CurrentDateTime.ToString(TimeRange.FULL_FORMAT_STRING);
        }

        private void UpdateRoomsList(Hotel hotel, DateTime currentDateTime)
        {
            SingleRoomCells.Clear();
            IList<Room> SingleRooms = hotel.GetAllRoomsByRoomType(RoomType.SINGLE);
            foreach (Room room in SingleRooms)
            {
                SingleRoomCells.Add(
                    new RoomCell(
                        room.Number,
                        room.IsFree(currentDateTime)
                    )
                );
            }

            DoubleRoomCells.Clear();
            IList<Room> DoubleRooms = hotel.GetAllRoomsByRoomType(RoomType.DOUBLE);
            foreach (Room room in DoubleRooms)
            {
                DoubleRoomCells.Add(
                    new RoomCell(
                        room.Number,
                        room.IsFree(currentDateTime)
                    )
                );
            }

            DoubleWithSofaRoomCells.Clear();
            IList<Room> DoubleWithSofaRooms = hotel.GetAllRoomsByRoomType(RoomType.DOUBLE_WITH_SOFA);
            foreach (Room room in DoubleWithSofaRooms)
            {
                DoubleWithSofaRoomCells.Add(
                    new RoomCell(
                        room.Number,
                        room.IsFree(currentDateTime)
                    )
                );
            }

            JuniorSuiteRoomCells.Clear();
            IList<Room> JuniorSuiteRooms = hotel.GetAllRoomsByRoomType(RoomType.JUNIOR_SUITE);
            foreach (Room room in JuniorSuiteRooms)
            {
                JuniorSuiteRoomCells.Add(
                    new RoomCell(
                        room.Number,
                        room.IsFree(currentDateTime)
                    )
                );
            }

            SuiteRoomCells.Clear();
            IList<Room> SuiteRooms = hotel.GetAllRoomsByRoomType(RoomType.SUITE);
            foreach (Room room in SuiteRooms)
            {
                SuiteRoomCells.Add(
                    new RoomCell(
                        room.Number,
                        room.IsFree(currentDateTime)
                    )
                );
            }
        }

        private void UpdateStatisticsText()
        {
            StatisticsTextBlock.Text = experiment.statistics.ToString();
        }

        private void UpdateProfitText()
        {
            this.ProfitTextBlock.Text = "Прибыль: " + this.experiment.Hotel.Profit;
        }

        private void ToExperimentEndedState()
        {
            StepButton.IsEnabled = false;
            CurrentTimeTextBlock.Text = "Эксперимент закончен";
        }
    }
}
