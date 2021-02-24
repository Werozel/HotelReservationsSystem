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

namespace Hotels.Pages.InitPage
{

    public sealed partial class InitPage : Page
    {
        public InitPage()
        {
            this.InitializeComponent();

            (this.FindName("StartButton") as Button).Click += (s, e) =>
            {
                IDictionary<RoomType, RoomInitInfo> roomsMap = new Dictionary<RoomType, RoomInitInfo>();

                bool success = true;
                TextBox singleCountTextBox = this.FindName("SingleCountTB") as TextBox;
                TextBox singlePriceTextBox = this.FindName("SinglePriceTB") as TextBox;
                success &= int.TryParse(singleCountTextBox.Text, out int singleCount);
                success &= int.TryParse(singlePriceTextBox.Text, out int singlePrice);
                roomsMap.Add(RoomType.SINGLE, new RoomInitInfo(singleCount, singlePrice));

                TextBox doubleCountTextBox = this.FindName("DoubleCountTB") as TextBox;
                TextBox doublePriceTextBox = this.FindName("DoublePriceTB") as TextBox;
                success &= int.TryParse(doubleCountTextBox.Text, out int doubleCount);
                success &= int.TryParse(doublePriceTextBox.Text, out int doublePrice);
                roomsMap.Add(RoomType.DOUBLE, new RoomInitInfo(doubleCount, doublePrice));

                TextBox doubleWithSofaCountTextBox = this.FindName("DoubleWithSofaCountTB") as TextBox;
                TextBox doubleWithSofaPriceTextBox = this.FindName("DoubleWithSofaPriceTB") as TextBox;
                success &= int.TryParse(doubleWithSofaCountTextBox.Text, out int doubleWithSofaCount);
                success &= int.TryParse(doubleWithSofaPriceTextBox.Text, out int doubleWithSofaPrice);
                roomsMap.Add(RoomType.DOUBLE_WITH_SOFA, new RoomInitInfo(doubleWithSofaCount, doubleWithSofaPrice));

                TextBox juniourSuiteCountTextBox = this.FindName("JuniorSuiteCountTB") as TextBox;
                TextBox juniourSuitePriceTextBox = this.FindName("JuniorSuitePriceTB") as TextBox;
                success &= int.TryParse(juniourSuiteCountTextBox.Text, out int juniourSuiteCount);
                success &= int.TryParse(juniourSuitePriceTextBox.Text, out int juniourSuitePrice);
                roomsMap.Add(RoomType.JUNIOR_SUITE, new RoomInitInfo(juniourSuiteCount, juniourSuitePrice));

                TextBox suiteCountTextBox = this.FindName("SuiteCountTB") as TextBox;
                TextBox suitePriceTextBox = this.FindName("SuitePriceTB") as TextBox;
                success &= int.TryParse(suiteCountTextBox.Text, out int suiteCount);
                success &= int.TryParse(suitePriceTextBox.Text, out int suitePrice);
                roomsMap.Add(RoomType.SUITE, new RoomInitInfo(suiteCount, suitePrice));

                TextBox daysCountTextBox = this.FindName("DaysCountTB") as TextBox;
                success &= int.TryParse(daysCountTextBox.Text, out int daysCount);

                if (success)
                {
                    this.Frame.Navigate(typeof(ExperimentPage.ExperimentPage), new ExperimentParameters(roomsMap, daysCount));
                } else
                {
                    Border errorBlock = this.FindName("ErrorBlock") as Border;
                    errorBlock.Visibility = Visibility.Visible;
                }
            };

        }
    }
}
