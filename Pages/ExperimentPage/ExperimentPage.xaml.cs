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
        public SolidColorBrush BackgroundBrush { get; }

        public RequestCell(string roomType, string timeRange, bool isApproved, string roomNumber)
        {
            this.RoomType = roomType;
            this.TimeRange = timeRange;
            this.IsApproved = isApproved;
            this.RoomNumber = roomNumber;
            this.BackgroundBrush = isApproved
                ? new SolidColorBrush(Colors.GREED)
                : new SolidColorBrush(Colors.RED);
        }

        public string FormatRoomNumber()
        {
            return RoomNumber ?? "";
        }
    }

    public class RoomCell
    {
        public string Number { get; }
        public bool IsAvaliable { get; }
        public SolidColorBrush BackgroundBrush { get; }

        public RoomCell(string number, bool isAvaliable)
        {
            this.Number = number;
            this.IsAvaliable = isAvaliable;
            this.BackgroundBrush = isAvaliable
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
                Request newRequest = experiment.Step();
                IsExperimentEnded = newRequest == null;
                UpdateCells(
                    new RequestCell(
                        RoomTypeHelper.RoomTypeToString(newRequest.RoomType),
                        newRequest.TimeRange.ToCellString(),
                        newRequest.IsApproved(),
                        newRequest.RoomNumber
                    )
                );
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
            
            InitCells();
            UpdateCurrentTimeText();
            UpdateRoomsList(experiment.Hotel, experiment.CurrentDateTime);
            UpdateStatisticsText();
            UpdateProfitText();
        }

        private void UpdateCells(RequestCell cell)
        {
            RequestCells.Add(cell);
            RequestsListView.ScrollIntoView(RequestsListView.Items[RequestsListView.Items.Count - 1]);
        }

        private void InitCells()
        {
            RequestCells.Clear();
            var cells = experiment.GetCells();
            foreach (RequestCell cell in cells)
            {
                RequestCells.Add(cell);
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
