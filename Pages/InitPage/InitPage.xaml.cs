using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Hotels.Pages.ExperimentPage;
using Hotels.Models.Rooms;

namespace Hotels.Pages.InitPage
{
    public sealed partial class InitPage
    {
        public InitPage()
        {
            InitializeComponent();

            ExitButton.Click += (s, e) =>
            {
                Application.Current.Exit();
            };

            StartButton.Click += (s, e) =>
            {
                IDictionary<RoomType, RoomInitInfo> roomsMap = new Dictionary<RoomType, RoomInitInfo>();

                var success = true;
                success &= int.TryParse(SingleCountTextBox.Text, out var singleCount);
                success &= 0 < singleCount && singleCount < 99;
                success &= int.TryParse(SinglePriceTextBox.Text, out var singlePrice);
                success &= 0 < singlePrice && singlePrice <= 1000;
                roomsMap.Add(RoomType.SINGLE, new RoomInitInfo(singleCount, singlePrice));

                success &= int.TryParse(DoubleCountTextBox.Text, out var doubleCount);
                success &= 0 < doubleCount && doubleCount < 99;
                success &= int.TryParse(DoublePriceTextBox.Text, out var doublePrice);
                success &= 0 < doublePrice && doublePrice <= 1000;
                roomsMap.Add(RoomType.DOUBLE, new RoomInitInfo(doubleCount, doublePrice));

                success &= int.TryParse(DoubleWithSofaCountTextBox.Text, out var doubleWithSofaCount);
                success &= 0 < doubleWithSofaCount && doubleWithSofaCount < 99;
                success &= int.TryParse(DoubleWithSofaPriceTextBox.Text, out var doubleWithSofaPrice);
                success &= 0 < doubleWithSofaPrice && doubleWithSofaPrice <= 1000;
                roomsMap.Add(RoomType.DOUBLE_WITH_SOFA, new RoomInitInfo(doubleWithSofaCount, doubleWithSofaPrice));
                
                success &= int.TryParse(JuniorSuiteCountTextBox.Text, out var juniorSuiteCount);
                success &= 0 < juniorSuiteCount && juniorSuiteCount < 99;
                success &= int.TryParse(JuniorSuitePriceTextBox.Text, out var juniorSuitePrice);
                success &= 0 < juniorSuitePrice && juniorSuitePrice <= 1000;
                roomsMap.Add(RoomType.JUNIOR_SUITE, new RoomInitInfo(juniorSuiteCount, juniorSuitePrice));

                success &= int.TryParse(SuiteCountTextBox.Text, out var suiteCount);
                success &= 0 < suiteCount && suiteCount < 99;
                success &= int.TryParse(SuitePriceTextBox.Text, out var suitePrice);
                success &= 0 < suitePrice && suitePrice <= 1000;
                roomsMap.Add(RoomType.SUITE, new RoomInitInfo(suiteCount, suitePrice));
                
                success &= int.TryParse(DaysCountTextBox.Text, out var daysCount);
                success &= 12 <= daysCount && daysCount <= 30;

                success &= int.TryParse(HoursPerStepTextBox.Text, out var hoursPerStep);
                success &= 0 < hoursPerStep && hoursPerStep <= 24;

                success &= int.TryParse(MaxDaysToStayTextBox.Text, out var maxDaysToBook);
                success &= 0 < maxDaysToBook && maxDaysToBook <= 28;

                success &= int.TryParse(MaxHoursUntilRequestTextBox.Text, out var maxHoursUntilRequest);
                success &= 0 < maxHoursUntilRequest && maxHoursUntilRequest <= 12 && maxHoursUntilRequest <= hoursPerStep;

                success &= int.TryParse(DiscountTextBox.Text, out var discountInt);
                success &= 0 <= discountInt && discountInt <= 100;

                if (success)
                {
                    Frame.Navigate(
                        typeof(ExperimentPage.ExperimentPage), 
                        new ExperimentParameters(
                            roomsMap, 
                            daysCount, 
                            hoursPerStep, 
                            maxHoursUntilRequest, 
                            maxDaysToBook, 
                            Convert.ToInt32(discountInt / 100)
                        )
                    );
                } else
                {
                    ErrorBlock.Visibility = Visibility.Visible;
                }
            };
        }
    }
}
