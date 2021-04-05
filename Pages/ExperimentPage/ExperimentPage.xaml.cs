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
    public sealed partial class ExperimentPage
    {

        private Experiment _experiment;
        private bool _isExperimentEnded;

        private IDictionary<RoomType, RoomInitInfo> _roomInitInfoMap;
        private int _daysCount;

        private readonly ObservableCollection<RequestCell> _requestCells = new ObservableCollection<RequestCell>();

        private readonly ObservableCollection<BookedTimeCell> _bookedTimeCells = 
            new ObservableCollection<BookedTimeCell>();

        private readonly ObservableCollection<RoomCell> _singleRoomCells = new ObservableCollection<RoomCell>();
        private readonly ObservableCollection<RoomCell> _doubleRoomCells = new ObservableCollection<RoomCell>();
        private readonly ObservableCollection<RoomCell> _doubleWithSofaRoomCells = new ObservableCollection<RoomCell>();
        private readonly ObservableCollection<RoomCell> _juniorSuiteRoomCells = new ObservableCollection<RoomCell>();
        private readonly ObservableCollection<RoomCell> _suiteRoomCells = new ObservableCollection<RoomCell>();

        private bool _backgroundTaskRunning;

        private string _selectedRoomNumber = "";

        public ExperimentPage()
        {
            InitializeComponent();

            RequestsListView.ItemsSource = _requestCells;

            SingleRoomsList.ItemsSource = _singleRoomCells;
            SingleRoomsList.ItemClick += OnRoomClick;
            DoubleRoomsList.ItemsSource = _doubleRoomCells;
            DoubleRoomsList.ItemClick += OnRoomClick;
            DoubleWithSofaRoomsList.ItemsSource = _doubleWithSofaRoomCells;
            DoubleWithSofaRoomsList.ItemClick += OnRoomClick;
            JuniorSuiteRoomsList.ItemsSource = _juniorSuiteRoomCells;
            JuniorSuiteRoomsList.ItemClick += OnRoomClick;
            SuiteRoomsList.ItemsSource = _suiteRoomCells;
            SuiteRoomsList.ItemClick += OnRoomClick;

            BookedTimesList.ItemsSource = _bookedTimeCells;

            ExitButton.Click += (s, e) =>
            {
                Application.Current.Exit();
            };

            RestartButton.Click += (s, e) =>
            {
                _backgroundTaskRunning = false;
                Frame.Navigate(typeof(InitPage.InitPage));
            };
            StepButton.Click += (s, e) =>
            {
                if (_isExperimentEnded)
                {
                    ToExperimentEndedState();
                    return;
                }
                _isExperimentEnded = !_experiment.Step();
                UpdateCells();
                UpdateCurrentTimeText();
                UpdateRoomsList(_experiment.Hotel, _experiment.CurrentDateTime);
                UpdateStatisticsText();
                UpdateProfitText();
            };

            StartStopButton.Click += (s, e) =>
            {
                if (_backgroundTaskRunning)
                {
                    StartStopButton.Content = "Запуск";
                    _backgroundTaskRunning = false;
                }
                else
                {
                    StartStopButton.Content = "Стоп";
                    _backgroundTaskRunning = true;
                    RunBackground();
                }
            };

            ToTheEndButton.Click += (s, e) =>
            {
                _experiment.ToTheEnd();
                UpdateCells();
                UpdateCurrentTimeText();
                UpdateRoomsList(_experiment.Hotel, _experiment.CurrentDateTime);
                UpdateStatisticsText();
                UpdateProfitText();
                ToExperimentEndedState();
            };

        }

        // Executed after constructor
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var parameter = e.Parameter;
            if (!(parameter is ExperimentParameters))
            {
                return;
            }
            var parameters = parameter as ExperimentParameters;
            _daysCount = parameters.DaysCount;
            _roomInitInfoMap = parameters.RoomsInfoMap;

            var today = DateTime.Today;
            var experimentEndTime = today.AddDays(_daysCount);
            _experiment = new Experiment(
                _roomInitInfoMap,
                new TimeRange(today, experimentEndTime),
                parameters.HoursPerStep,
                parameters.MaxHoursUntilRequest,
                parameters.MaxDaysToBook,
                parameters.Discount
            );

            if (_roomInitInfoMap.TryGetValue(RoomType.SINGLE, out RoomInitInfo singleRoomInfo))
            {
                SinglePriceTextBlock.Text = singleRoomInfo.Price.ToString();
            }
            if (_roomInitInfoMap.TryGetValue(RoomType.DOUBLE, out RoomInitInfo doubleRoomInfo))
            {
                DoublePriceTextBlock.Text = doubleRoomInfo.Price.ToString();
            }
            if (_roomInitInfoMap.TryGetValue(RoomType.DOUBLE_WITH_SOFA, out RoomInitInfo doubleWithSofaRoomInfo))
            {
                DoubleWithSofaPriceTextBlock.Text = doubleWithSofaRoomInfo.Price.ToString();
            }
            if (_roomInitInfoMap.TryGetValue(RoomType.JUNIOR_SUITE, out RoomInitInfo juniorSuiteRoomInfo))
            {
                JuniorSuitePriceTextBlock.Text = juniorSuiteRoomInfo.Price.ToString();
            }
            if (_roomInitInfoMap.TryGetValue(RoomType.SUITE, out RoomInitInfo suiteRoomInfo))
            {
                SuitePriceTextBlock.Text = suiteRoomInfo.Price.ToString();
            }

            UpdateCells();
            UpdateCurrentTimeText();
            UpdateRoomsList(_experiment.Hotel, _experiment.CurrentDateTime);
            UpdateStatisticsText();
            UpdateProfitText();
        }

        private void OnRoomClick(object s, ItemClickEventArgs e)
        {
            var clickedItem = e.ClickedItem;
            if (!(clickedItem is RoomCell))
            {
                return;
            }
            var cell = clickedItem as RoomCell;
            UpdateRoomInfo(cell);
            _selectedRoomNumber = cell.Number;
        }

        private void UpdateRoomInfo(RoomCell cell)
        {
            var bookedTimes = cell.BookedTimes;
            bookedTimes.Sort((a, b) => a.Start.CompareTo(b.Start));
            _bookedTimeCells.Clear();
            if (bookedTimes.Count == 0)
            {
                _bookedTimeCells.Add(
                    new BookedTimeCell(
                        "Нет ни одной брони"
                    )
                );
                return;
            }
            foreach (var bookedTime in bookedTimes)
            {
                _bookedTimeCells.Add(
                    new BookedTimeCell(
                        bookedTime.ToCellString()
                    )
                );
            }
            ListRoomNumberTextBlock.Text = cell.Number;
            ListRoomNumberTextBlock.Visibility = Visibility.Visible;

            ListRoomOccupancyTextBlock.Text = $"Номер заполнен на {cell.OccupancyText}";
            ListRoomOccupancyTextBlock.Visibility = Visibility.Visible;

            ListRoomRequestCountTextBlock.Text = $"Всего {cell.RequestsCount} заявок на этот номер";
            ListRoomRequestCountTextBlock.Visibility = Visibility.Visible;
        }

        private void UpdateCells()
        {
            _requestCells.Clear();
            var currentStep = _experiment.CurrentStep;
            var cells = _experiment.RequestList
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
                        request.BookTime.ToString(Constants.FullFormatString)
                    )
                )
                .ToList();
            foreach (var cell in cells)
            {
                _requestCells.Add(cell);
            }
            var listItems = RequestsListView.Items;
            if (listItems == null)
            {
                return;
            }
            var listItemsCount = listItems.Count;
            if (listItemsCount != 0)
            {
                RequestsListView.ScrollIntoView(listItems[listItemsCount - 1]);
            }
        }

        private void UpdateCurrentTimeText()
        {
            var currentDateTime = _experiment.CurrentDateTime;
            CurrentTimeTextBlock.Text = currentDateTime.ToString(Constants.FullFormatString);
            var endDateTime = _experiment.EndDateTime;
            DaysLeftTextBlock.Text = $"Осталось {Math.Floor((endDateTime - currentDateTime).TotalDays)} дней";
        }

        private void UpdateRoomsList(Hotel hotel, DateTime currentDateTime)
        {
            var experimentTimeRange = new TimeRange(
                _experiment.StartDateTime, 
                _experiment.EndDateTime.AddDays(ExperimentConfig.ValidDaysAfterEnding)
            );

            _singleRoomCells.Clear();
            var singleRooms = hotel.GetAllRoomsByRoomType(RoomType.SINGLE);
            foreach (var room in singleRooms)
            {
                var newRoomCell = new RoomCell(
                    room.Number,
                    room.IsFree(currentDateTime),
                    room.BookedTimes,
                    room.GetRequestsCount(),
                    $"{room.GetOccupancyInPeriod(experimentTimeRange)}%"
                );
                if (room.Number == _selectedRoomNumber)
                {
                    UpdateRoomInfo(newRoomCell);
                }
                _singleRoomCells.Add(newRoomCell);
            }

            _doubleRoomCells.Clear();
            var doubleRooms = hotel.GetAllRoomsByRoomType(RoomType.DOUBLE);
            foreach (var room in doubleRooms)
            {
                var newRoomCell = new RoomCell(
                    room.Number,
                    room.IsFree(currentDateTime),
                    room.BookedTimes,
                    room.GetRequestsCount(),
                    $"{room.GetOccupancyInPeriod(experimentTimeRange)}%"
                );
                if (room.Number == _selectedRoomNumber)
                {
                    UpdateRoomInfo(newRoomCell);
                }
                _doubleRoomCells.Add(newRoomCell);
            }

            _doubleWithSofaRoomCells.Clear();
            var doubleWithSofaRooms = hotel.GetAllRoomsByRoomType(RoomType.DOUBLE_WITH_SOFA);
            foreach (var room in doubleWithSofaRooms)
            {

                var newRoomCell = new RoomCell(
                    room.Number,
                    room.IsFree(currentDateTime),
                    room.BookedTimes,
                    room.GetRequestsCount(),
                    $"{room.GetOccupancyInPeriod(experimentTimeRange)}%"
                );
                if (room.Number == _selectedRoomNumber)
                {
                    UpdateRoomInfo(newRoomCell);
                }
                _doubleWithSofaRoomCells.Add(newRoomCell);
            }

            _juniorSuiteRoomCells.Clear();
            var juniorSuiteRooms = hotel.GetAllRoomsByRoomType(RoomType.JUNIOR_SUITE);
            foreach (var room in juniorSuiteRooms)
            {
                var newRoomCell = new RoomCell(
                    room.Number,
                    room.IsFree(currentDateTime),
                    room.BookedTimes,
                    room.GetRequestsCount(),
                    $"{room.GetOccupancyInPeriod(experimentTimeRange)}%"
                );
                if (room.Number == _selectedRoomNumber)
                {
                    UpdateRoomInfo(newRoomCell);
                }
                _juniorSuiteRoomCells.Add(newRoomCell);
            }

            _suiteRoomCells.Clear();
            var suiteRooms = hotel.GetAllRoomsByRoomType(RoomType.SUITE);
            foreach (var room in suiteRooms)
            {
                var newRoomCell = new RoomCell(
                    room.Number,
                    room.IsFree(currentDateTime),
                    room.BookedTimes,
                    room.GetRequestsCount(),
                    $"{room.GetOccupancyInPeriod(experimentTimeRange)}%"
                );
                if (room.Number == _selectedRoomNumber)
                {
                    UpdateRoomInfo(newRoomCell);
                }
                _suiteRoomCells.Add(newRoomCell);
            }
        }

        private void UpdateStatisticsText()
        {
            StatisticsTextBlock.Text = _experiment.Statistics.ToString();
        }

        private void UpdateProfitText()
        {
            ProfitTextBlock.Text = "Прибыль: " + _experiment.Hotel.Profit;
        }

        private void ToExperimentEndedState()
        {
            StepButton.IsEnabled = false;
            ExperimentEndedTextBlock.Visibility = Visibility.Visible;
            _backgroundTaskRunning = false;
            StartStopButton.Content = "Старт";
            StartStopButton.IsEnabled = false;
            ToTheEndButton.IsEnabled = false;
        }

        private async void RunBackground()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                while (_backgroundTaskRunning)
                {
                    if (_isExperimentEnded)
                    {
                        ToExperimentEndedState();
                        return;
                    }
                    _isExperimentEnded = !_experiment.Step();
                    UpdateCells();
                    UpdateCurrentTimeText();
                    UpdateRoomsList(_experiment.Hotel, _experiment.CurrentDateTime);
                    UpdateStatisticsText();
                    UpdateProfitText();
                    await Task.Delay(1000, new CancellationTokenSource().Token);
                }
            });
        }
    }
}
