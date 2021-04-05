using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using Hotels.Models;
using Hotels.Pages.ExperimentPage.Cells;
using Hotels.Models.Rooms;
using Hotels.Models.Experiments;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Hotels.Models.Requests;

namespace Hotels.Pages.ExperimentPage
{

    public sealed partial class ExperimentPage : Page
    {

        private Experiment experiment;
        private bool IsExperimentEnded;

        private IDictionary<RoomType, RoomInitInfo> RoomsInfoMap;
        private int DaysCount;

        private ObservableCollection<RequestCell> RequestCells = new ObservableCollection<RequestCell>();

        private ObservableCollection<BookedTimeCell> BookedTimeCells = new ObservableCollection<BookedTimeCell>();

        private ObservableCollection<RoomCell> SingleRoomCells = new ObservableCollection<RoomCell>();
        private ObservableCollection<RoomCell> DoubleRoomCells = new ObservableCollection<RoomCell>();
        private ObservableCollection<RoomCell> DoubleWithSofaRoomCells = new ObservableCollection<RoomCell>();
        private ObservableCollection<RoomCell> JuniorSuiteRoomCells = new ObservableCollection<RoomCell>();
        private ObservableCollection<RoomCell> SuiteRoomCells = new ObservableCollection<RoomCell>();

        private bool BackgroundTaskRunning = false;

        public ExperimentPage()
        {
            this.InitializeComponent();

            RequestsListView.ItemsSource = RequestCells;

            this.SingleRoomsList.ItemsSource = SingleRoomCells;
            this.SingleRoomsList.ItemClick += OnRoomClick;
            this.DoubleRoomsList.ItemsSource = DoubleRoomCells;
            this.DoubleRoomsList.ItemClick += OnRoomClick;
            this.DoubleWithSofaRoomsList.ItemsSource = DoubleWithSofaRoomCells;
            this.DoubleWithSofaRoomsList.ItemClick += OnRoomClick;
            this.JuniorSuiteRoomsList.ItemsSource = JuniorSuiteRoomCells;
            this.JuniorSuiteRoomsList.ItemClick += OnRoomClick;
            this.SuiteRoomsList.ItemsSource = SuiteRoomCells;
            this.SuiteRoomsList.ItemClick += OnRoomClick;

            this.BookedTimesList.ItemsSource = this.BookedTimeCells;

            this.ExitButton.Click += (s, e) =>
            {
                Application.Current.Exit();
            };

            this.RestartButton.Click += (s, e) =>
            {
                this.Frame.Navigate(typeof(InitPage.InitPage));
            };
            this.StepButton.Click += (s, e) =>
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

            this.StartStopButton.Click += (s, e) =>
            {
                if (BackgroundTaskRunning)
                {
                    this.StartStopButton.Content = "Запуск";
                    BackgroundTaskRunning = false;
                }
                else
                {
                    this.StartStopButton.Content = "Стоп";
                    BackgroundTaskRunning = true;
                    RunBackground();
                }
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
                parameters.HoursPerStep,
                parameters.MaxDaysToBook
            );

            UpdateCells();
            UpdateCurrentTimeText();
            UpdateRoomsList(experiment.Hotel, experiment.CurrentDateTime);
            UpdateStatisticsText();
            UpdateProfitText();
        }

        private void OnRoomClick(Object s, ItemClickEventArgs e)
        {
            RoomCell cell = e.ClickedItem as RoomCell;
            List<TimeRange> bookedTimes = (cell.BookedTimes as List<TimeRange>);
            bookedTimes.Sort(
                new Comparison<TimeRange>((TimeRange a, TimeRange b) =>
                {
                    return a.Start.CompareTo(b.Start);
                })
            );
            BookedTimeCells.Clear();
            if (bookedTimes.Count == 0)
            {
                BookedTimeCells.Add(
                    new BookedTimeCell(
                        "Нет ни одной брони"
                    )
                );
                return;
            }
            foreach (TimeRange bookedTime in bookedTimes)
            {
                BookedTimeCells.Add(
                    new BookedTimeCell(
                        bookedTime.ToCellString()
                    )
                );
            }
        }

        private void UpdateCells()
        {
            RequestCells.Clear();
            var cells = experiment.RequestList
                .Select(request =>
                    new RequestCell(
                        RoomTypeHelper.RoomTypeToString(request.RoomType),
                        request.TimeRange.ToCellString(),
                        request.IsApproved(),
                        request.RoomNumber,
                        RequestTypeHelper.RequestTypeToString(request.Type),
                        request.HasDiscount,
                        "+" + request.Price
                    )
                )
                .ToList();
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
            this.CurrentTimeTextBlock.Text = this.experiment.CurrentDateTime.ToString(Constants.FULL_FORMAT_STRING);
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
                        room.IsFree(currentDateTime),
                        room.BookedTimes
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
                        room.IsFree(currentDateTime),
                        room.BookedTimes
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
                        room.IsFree(currentDateTime),
                        room.BookedTimes
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
                        room.IsFree(currentDateTime),
                        room.BookedTimes
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
                        room.IsFree(currentDateTime),
                        room.BookedTimes
                    )
                );
            }
        }

        private void UpdateStatisticsText()
        {
            StatisticsTextBlock.Text = experiment.Statistics.ToString();
        }

        private void UpdateProfitText()
        {
            this.ProfitTextBlock.Text = "Прибыль: " + this.experiment.Hotel.Profit;
        }

        private void ToExperimentEndedState()
        {
            StepButton.IsEnabled = false;
            CurrentTimeTextBlock.Text = "Эксперимент закончен";
            BackgroundTaskRunning = false;
            StartStopButton.Content = "Старт";
            StartStopButton.IsEnabled = false;
        }

        private async void RunBackground()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                while (BackgroundTaskRunning)
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
                    await Task.Delay(1000, new CancellationTokenSource().Token);
                }
            });
        }
    }
}
