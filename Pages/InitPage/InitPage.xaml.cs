﻿using System;
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

            (this.FindName("ExitButton") as Button).Click += (s, e) =>
            {
                Application.Current.Exit();
            };

            TextBox singleCountTextBox = this.FindName("SingleCountTB") as TextBox;
            TextBox singlePriceTextBox = this.FindName("SinglePriceTB") as TextBox;

            TextBox doubleCountTextBox = this.FindName("DoubleCountTB") as TextBox;
            TextBox doublePriceTextBox = this.FindName("DoublePriceTB") as TextBox;

            TextBox doubleWithSofaCountTextBox = this.FindName("DoubleWithSofaCountTB") as TextBox;
            TextBox doubleWithSofaPriceTextBox = this.FindName("DoubleWithSofaPriceTB") as TextBox;

            TextBox juniourSuiteCountTextBox = this.FindName("JuniorSuiteCountTB") as TextBox;
            TextBox juniourSuitePriceTextBox = this.FindName("JuniorSuitePriceTB") as TextBox;

            TextBox suiteCountTextBox = this.FindName("SuiteCountTB") as TextBox;
            TextBox suitePriceTextBox = this.FindName("SuitePriceTB") as TextBox;

            TextBox daysCountTextBox = this.FindName("DaysCountTB") as TextBox;
            TextBox maxHoursPerStepTextBox = this.FindName("HoursPerStepTB") as TextBox;
            TextBox maxDaysToBookTextBox = this.FindName("MaxDaysToStayTB") as TextBox;

            Border errorBlock = this.FindName("ErrorBlock") as Border;

            (this.FindName("StartButton") as Button).Click += (s, e) =>
            {
                IDictionary<RoomType, RoomInitInfo> roomsMap = new Dictionary<RoomType, RoomInitInfo>();

                bool success = true;
                success &= int.TryParse(singleCountTextBox.Text, out int singleCount);
                success &= singleCount < 99;
                success &= int.TryParse(singlePriceTextBox.Text, out int singlePrice);
                roomsMap.Add(RoomType.SINGLE, new RoomInitInfo(singleCount, singlePrice));

                success &= int.TryParse(doubleCountTextBox.Text, out int doubleCount);
                success &= doubleCount < 99;
                success &= int.TryParse(doublePriceTextBox.Text, out int doublePrice);
                roomsMap.Add(RoomType.DOUBLE, new RoomInitInfo(doubleCount, doublePrice));

                success &= int.TryParse(doubleWithSofaCountTextBox.Text, out int doubleWithSofaCount);
                success &= doubleWithSofaCount < 99;
                success &= int.TryParse(doubleWithSofaPriceTextBox.Text, out int doubleWithSofaPrice);
                roomsMap.Add(RoomType.DOUBLE_WITH_SOFA, new RoomInitInfo(doubleWithSofaCount, doubleWithSofaPrice));
                
                success &= int.TryParse(juniourSuiteCountTextBox.Text, out int juniourSuiteCount);
                success &= juniourSuiteCount < 99;
                success &= int.TryParse(juniourSuitePriceTextBox.Text, out int juniourSuitePrice);
                roomsMap.Add(RoomType.JUNIOR_SUITE, new RoomInitInfo(juniourSuiteCount, juniourSuitePrice));

                success &= int.TryParse(suiteCountTextBox.Text, out int suiteCount);
                success &= suiteCount < 99;
                success &= int.TryParse(suitePriceTextBox.Text, out int suitePrice);
                roomsMap.Add(RoomType.SUITE, new RoomInitInfo(suiteCount, suitePrice));
                
                success &= int.TryParse(daysCountTextBox.Text, out int daysCount);
                success &= 12 <= daysCount && daysCount <= 30;

                success &= int.TryParse(maxHoursPerStepTextBox.Text, out int maxHoursPerStep);
                success &= int.TryParse(maxDaysToBookTextBox.Text, out int maxDaysToBook);
                success &= maxDaysToBook >= 1;

                if (success)
                {
                    this.Frame.Navigate(typeof(ExperimentPage.ExperimentPage), new ExperimentParameters(roomsMap, daysCount, maxHoursPerStep, maxDaysToBook));
                } else
                {
                    errorBlock.Visibility = Visibility.Visible;
                }
            };

        }
    }
}
