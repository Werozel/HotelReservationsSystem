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

namespace Hotels.Pages.InitPage
{

    public sealed partial class InitPage : Page
    {
        public InitPage()
        {
            this.InitializeComponent();

            Button startButton = this.FindName("StartButton") as Button;
            startButton.Click += (s, e) =>
            {
                TextBox roomsCountTextBox = this.FindName("RoomsCountTextBox") as TextBox;
                TextBox daysCountTextBox = this.FindName("DaysCountTextBox") as TextBox;
                string roomsCountString = roomsCountTextBox.Text;
                string daysCountString = daysCountTextBox.Text;
                
                bool success = int.TryParse(roomsCountString, out int roomsCount);
                success &= int.TryParse(daysCountString, out int daysCount);
                TextBlock errorTextBlock = this.FindName("ErrorTextBlock") as TextBlock;
                if (!success)
                {
                    errorTextBlock.Text = "Need to be numbers";
                } else
                {
                    this.Frame.Navigate(typeof(ExperimentPage.ExperimentPage), new Parameters(roomsCount, daysCount));
                }
            };

        }
    }
}
