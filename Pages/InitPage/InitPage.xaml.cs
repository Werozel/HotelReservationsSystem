using System;
using System.Threading;
using System.Threading.Tasks;
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
using Windows.UI;
using Hotels.Pages.ExperimentPage;
using Hotels.Models;
using Hotels.Models.Rooms;

namespace Hotels.Pages.InitPage
{

    public sealed partial class InitPage : Page
    {
        public InitPage()
        {
            this.InitializeComponent();

            (this.ExitButton).Click += (s, e) =>
            {
                Application.Current.Exit();
            };

            (this.StartButton).Click += (s, e) =>
            {
                IDictionary<RoomType, RoomInitInfo> roomsMap = new Dictionary<RoomType, RoomInitInfo>();

                bool success = true;
                success &= int.TryParse(this.SingleCountTB.Text, out int singleCount);
                success &= 0 < singleCount && singleCount < 99;
                success &= int.TryParse(this.SinglePriceTB.Text, out int singlePrice);
                success &= 0 < singlePrice && singlePrice <= 1000;
                roomsMap.Add(RoomType.SINGLE, new RoomInitInfo(singleCount, singlePrice));

                success &= int.TryParse(this.DoubleCountTB.Text, out int doubleCount);
                success &= 0 < doubleCount && doubleCount < 99;
                success &= int.TryParse(this.DoublePriceTB.Text, out int doublePrice);
                success &= 0 < doublePrice && doublePrice <= 1000;
                roomsMap.Add(RoomType.DOUBLE, new RoomInitInfo(doubleCount, doublePrice));

                success &= int.TryParse(this.DoubleWithSofaCountTB.Text, out int doubleWithSofaCount);
                success &= 0 < doubleWithSofaCount && doubleWithSofaCount < 99;
                success &= int.TryParse(this.DoubleWithSofaPriceTB.Text, out int doubleWithSofaPrice);
                success &= 0 < doubleWithSofaPrice && doubleWithSofaPrice <= 1000;
                roomsMap.Add(RoomType.DOUBLE_WITH_SOFA, new RoomInitInfo(doubleWithSofaCount, doubleWithSofaPrice));
                
                success &= int.TryParse(this.JuniorSuiteCountTB.Text, out int juniourSuiteCount);
                success &= 0 < juniourSuiteCount && juniourSuiteCount < 99;
                success &= int.TryParse(this.JuniorSuitePriceTB.Text, out int juniourSuitePrice);
                success &= 0 < juniourSuitePrice && juniourSuitePrice <= 1000;
                roomsMap.Add(RoomType.JUNIOR_SUITE, new RoomInitInfo(juniourSuiteCount, juniourSuitePrice));

                success &= int.TryParse(this.SuiteCountTB.Text, out int suiteCount);
                success &= 0 < suiteCount && suiteCount < 99;
                success &= int.TryParse(this.SuitePriceTB.Text, out int suitePrice);
                success &= 0 < suitePrice && suitePrice <= 1000;
                roomsMap.Add(RoomType.SUITE, new RoomInitInfo(suiteCount, suitePrice));
                
                success &= int.TryParse(this.DaysCountTB.Text, out int daysCount);
                success &= 12 <= daysCount && daysCount <= 30;

                success &= int.TryParse(this.HoursPerStepTB.Text, out int hoursPerStep);
                success &= 0 < hoursPerStep && hoursPerStep <= 24;

                success &= int.TryParse(this.MaxDaysToStayTB.Text, out int maxDaysToBook);
                success &= 0 < maxDaysToBook && maxDaysToBook <= 28;

                success &= int.TryParse(this.MaxHoursUntilRequestTB.Text, out int maxHoursUntilRequest);
                success &= 0 < maxHoursUntilRequest && maxHoursUntilRequest <= 12 && maxHoursUntilRequest <= hoursPerStep;

                success &= int.TryParse(this.DiscountTB.Text, out int discountInt);
                success &= 0 <= discountInt && discountInt <= 100;

                if (success)
                {
                    this.Frame.Navigate(typeof(ExperimentPage.ExperimentPage), new ExperimentParameters(roomsMap, daysCount, hoursPerStep, maxHoursUntilRequest, maxDaysToBook, discountInt / 100));
                } else
                {
                    ErrorBlock.Visibility = Visibility.Visible;
                }
            };

        }
    }
}
