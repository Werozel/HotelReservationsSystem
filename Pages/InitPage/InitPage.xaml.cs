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
                success &= singleCount < 99;
                success &= int.TryParse(this.SinglePriceTB.Text, out int singlePrice);
                roomsMap.Add(RoomType.SINGLE, new RoomInitInfo(singleCount, singlePrice));

                success &= int.TryParse(this.DoubleCountTB.Text, out int doubleCount);
                success &= doubleCount < 99;
                success &= int.TryParse(this.DoublePriceTB.Text, out int doublePrice);
                roomsMap.Add(RoomType.DOUBLE, new RoomInitInfo(doubleCount, doublePrice));

                success &= int.TryParse(this.DoubleWithSofaCountTB.Text, out int doubleWithSofaCount);
                success &= doubleWithSofaCount < 99;
                success &= int.TryParse(this.DoubleWithSofaPriceTB.Text, out int doubleWithSofaPrice);
                roomsMap.Add(RoomType.DOUBLE_WITH_SOFA, new RoomInitInfo(doubleWithSofaCount, doubleWithSofaPrice));
                
                success &= int.TryParse(this.JuniorSuiteCountTB.Text, out int juniourSuiteCount);
                success &= juniourSuiteCount < 99;
                success &= int.TryParse(this.JuniorSuitePriceTB.Text, out int juniourSuitePrice);
                roomsMap.Add(RoomType.JUNIOR_SUITE, new RoomInitInfo(juniourSuiteCount, juniourSuitePrice));

                success &= int.TryParse(this.SuiteCountTB.Text, out int suiteCount);
                success &= suiteCount < 99;
                success &= int.TryParse(this.SuitePriceTB.Text, out int suitePrice);
                roomsMap.Add(RoomType.SUITE, new RoomInitInfo(suiteCount, suitePrice));
                
                success &= int.TryParse(this.DaysCountTB.Text, out int daysCount);
                success &= 12 <= daysCount && daysCount <= 30;

                success &= int.TryParse(this.HoursPerStepTB.Text, out int maxHoursPerStep);
                success &= int.TryParse(this.MaxDaysToStayTB.Text, out int maxDaysToBook);
                success &= maxDaysToBook >= 1;

                if (success)
                {
                    this.Frame.Navigate(typeof(ExperimentPage.ExperimentPage), new ExperimentParameters(roomsMap, daysCount, maxHoursPerStep, maxDaysToBook));
                } else
                {
                    ErrorBlock.Visibility = Visibility.Visible;
                }
            };

        }
    }
}
