using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Hotels.Models;
using Hotels.Pages.ExperimentPage.Cells;
using Hotels.Models.Rooms;
using Hotels.Models.Experiments;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Hotels.Models.Requests;
using Hotels.Models.Experiment;

namespace Hotels.Pages.ExperimentPage
{

    public sealed partial class ExperimentPage : Page
    {

        private Experiment Experiment;
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

        private string SelectedRoomNumber = "";

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
                this.BackgroundTaskRunning = false;
                this.Frame.Navigate(typeof(InitPage.InitPage));
            };
            this.StepButton.Click += (s, e) =>
            {
                if (IsExperimentEnded)
                {
                    ToExperimentEndedState();
                    return;
                }
                IsExperimentEnded = !Experiment.Step();
                UpdateCells();
                UpdateCurrentTimeText();
                UpdateRoomsList(Experiment.Hotel, Experiment.CurrentDateTime);
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

            this.ToTheEndButton.Click += (s, e) =>
            {
                Experiment.ToTheEnd();
                UpdateCells();
                UpdateCurrentTimeText();
                UpdateRoomsList(Experiment.Hotel, Experiment.CurrentDateTime);
                UpdateStatisticsText();
                UpdateProfitText();
                ToExperimentEndedState();
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
            Experiment = new Experiment(
                this.RoomsInfoMap,
                new TimeRange(today, experimentEndTime),
                parameters.HoursPerStep,
                parameters.MaxHoursUntilRequest,
                parameters.MaxDaysToBook,
                parameters.Discount
            );

            if (this.RoomsInfoMap.TryGetValue(RoomType.SINGLE, out RoomInitInfo singleRoomInfo))
            {
                this.SinglePriceTB.Text = singleRoomInfo.Price.ToString();
            }
            if (this.RoomsInfoMap.TryGetValue(RoomType.DOUBLE, out RoomInitInfo doubleRoomInfo))
            {
                this.DoublePriceTB.Text = doubleRoomInfo.Price.ToString();
            }
            if (this.RoomsInfoMap.TryGetValue(RoomType.DOUBLE_WITH_SOFA, out RoomInitInfo doubleWithSofaRoomInfo))
            {
                this.DoubleWithSofaPriceTB.Text = doubleWithSofaRoomInfo.Price.ToString();
            }
            if (this.RoomsInfoMap.TryGetValue(RoomType.JUNIOR_SUITE, out RoomInitInfo juniorSuiteRoomInfo))
            {
                this.JuniorSuitePriceTB.Text = juniorSuiteRoomInfo.Price.ToString();
            }
            if (this.RoomsInfoMap.TryGetValue(RoomType.SUITE, out RoomInitInfo suiteRoomInfo))
            {
                this.SuitePriceTB.Text = suiteRoomInfo.Price.ToString();
            }

            UpdateCells();
            UpdateCurrentTimeText();
            UpdateRoomsList(Experiment.Hotel, Experiment.CurrentDateTime);
            UpdateStatisticsText();
            UpdateProfitText();
        }

        private void OnRoomClick(Object s, ItemClickEventArgs e)
        {
            RoomCell cell = e.ClickedItem as RoomCell;
            UpdateRoomInfo(cell);
            this.SelectedRoomNumber = cell.Number;
        }

        private void UpdateRoomInfo(RoomCell cell)
        {
            List<TimeRange> bookedTimes = cell.BookedTimes as List<TimeRange>;
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
            this.ListRoomNumberTB.Text = cell.Number;
            this.ListRoomNumberTB.Visibility = Visibility.Visible;

            this.ListRoomOccupancyTB.Text = $"Номер заполнен на {cell.OccupancyText}";
            this.ListRoomOccupancyTB.Visibility = Visibility.Visible;

            this.ListRoomRequestCountTB.Text = $"Всего {cell.RequestsCount} заявок на этот номер";
            this.ListRoomRequestCountTB.Visibility = Visibility.Visible;
        }

        private void UpdateCells()
        {
            RequestCells.Clear();
            int currentStep = Experiment.CurrentStep;
            var cells = Experiment.RequestList
                .Select(request =>
                    new RequestCell(
                        RoomTypeHelper.RoomTypeToString(request.RoomType),
                        request.TimeRange.ToCellString(),
                        request.IsApproved(),
                        request.RoomNumber,
                        RequestTypeHelper.RequestTypeToString(request.Type),
                        request.HasDiscount,
                        "+" + request.Price,
                        request.Step,
                        currentStep == request.Step,
                        request.BookTime.ToString(Constants.FULL_FORMAT_STRING)
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
            DateTime currentDateTime = this.Experiment.CurrentDateTime;
            this.CurrentTimeTextBlock.Text = currentDateTime.ToString(Constants.FULL_FORMAT_STRING);
            DateTime endDateTime = this.Experiment.EndDateTime;
            this.DaysLeftTB.Text = String.Format("Осталось {0} дней", Math.Floor((endDateTime - currentDateTime).TotalDays));
        }

        private void UpdateRoomsList(Hotel hotel, DateTime currentDateTime)
        {
            TimeRange experimentTimeRange = new TimeRange(this.Experiment.StartDateTime, this.Experiment.EndDateTime.AddDays(ExperimentConfig.VALID_DAYS_AFTER_ENDING));

            SingleRoomCells.Clear();
            IList<Room> singleRooms = hotel.GetAllRoomsByRoomType(RoomType.SINGLE);
            foreach (Room room in singleRooms)
            {
                var newRoomCell = new RoomCell(
                    room.Number,
                    room.IsFree(currentDateTime),
                    room.BookedTimes,
                    room.GetRequestsCount(),
                    $"{room.GetOccupancyInPeriod(experimentTimeRange)}%"
                );
                if (room.Number == this.SelectedRoomNumber)
                {
                    UpdateRoomInfo(newRoomCell);
                }
                SingleRoomCells.Add(newRoomCell);
            }

            DoubleRoomCells.Clear();
            IList<Room> doubleRooms = hotel.GetAllRoomsByRoomType(RoomType.DOUBLE);
            foreach (Room room in doubleRooms)
            {
                var newRoomCell = new RoomCell(
                    room.Number,
                    room.IsFree(currentDateTime),
                    room.BookedTimes,
                    room.GetRequestsCount(),
                    $"{room.GetOccupancyInPeriod(experimentTimeRange)}%"
                );
                if (room.Number == this.SelectedRoomNumber)
                {
                    UpdateRoomInfo(newRoomCell);
                }
                DoubleRoomCells.Add(newRoomCell);
            }

            DoubleWithSofaRoomCells.Clear();
            IList<Room> doubleWithSofaRooms = hotel.GetAllRoomsByRoomType(RoomType.DOUBLE_WITH_SOFA);
            foreach (Room room in doubleWithSofaRooms)
            {

                var newRoomCell = new RoomCell(
                    room.Number,
                    room.IsFree(currentDateTime),
                    room.BookedTimes,
                    room.GetRequestsCount(),
                    $"{room.GetOccupancyInPeriod(experimentTimeRange)}%"
                );
                if (room.Number == this.SelectedRoomNumber)
                {
                    UpdateRoomInfo(newRoomCell);
                }
                DoubleWithSofaRoomCells.Add(newRoomCell);
            }

            JuniorSuiteRoomCells.Clear();
            IList<Room> juniorSuiteRooms = hotel.GetAllRoomsByRoomType(RoomType.JUNIOR_SUITE);
            foreach (Room room in juniorSuiteRooms)
            {
                var newRoomCell = new RoomCell(
                    room.Number,
                    room.IsFree(currentDateTime),
                    room.BookedTimes,
                    room.GetRequestsCount(),
                    $"{room.GetOccupancyInPeriod(experimentTimeRange)}%"
                );
                if (room.Number == this.SelectedRoomNumber)
                {
                    UpdateRoomInfo(newRoomCell);
                }
                JuniorSuiteRoomCells.Add(newRoomCell);
            }

            SuiteRoomCells.Clear();
            IList<Room> suiteRooms = hotel.GetAllRoomsByRoomType(RoomType.SUITE);
            foreach (Room room in suiteRooms)
            {
                var newRoomCell = new RoomCell(
                    room.Number,
                    room.IsFree(currentDateTime),
                    room.BookedTimes,
                    room.GetRequestsCount(),
                    $"{room.GetOccupancyInPeriod(experimentTimeRange)}%"
                );
                if (room.Number == this.SelectedRoomNumber)
                {
                    UpdateRoomInfo(newRoomCell);
                }
                SuiteRoomCells.Add(newRoomCell);
            }
        }

        private void UpdateStatisticsText()
        {
            StatisticsTextBlock.Text = Experiment.Statistics.ToString();
        }

        private void UpdateProfitText()
        {
            this.ProfitTextBlock.Text = "Прибыль: " + this.Experiment.Hotel.Profit;
        }

        private void ToExperimentEndedState()
        {
            StepButton.IsEnabled = false;
            ExperimentEndedTB.Visibility = Visibility.Visible;
            BackgroundTaskRunning = false;
            StartStopButton.Content = "Старт";
            StartStopButton.IsEnabled = false;
            ToTheEndButton.IsEnabled = false;
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
                    IsExperimentEnded = !Experiment.Step();
                    UpdateCells();
                    UpdateCurrentTimeText();
                    UpdateRoomsList(Experiment.Hotel, Experiment.CurrentDateTime);
                    UpdateStatisticsText();
                    UpdateProfitText();
                    await Task.Delay(1000, new CancellationTokenSource().Token);
                }
            });
        }
    }
}
