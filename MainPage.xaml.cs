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

namespace Hotels
{

    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            Button startButton = this.FindName("StartButton") as Button;
            startButton.Click += (s, e) =>
            {
                TextBox roomsCountTextBox = this.FindName("RoomsCountTextBox") as TextBox;
                TextBox stepsCountTextBox = this.FindName("StepsCountTextBox") as TextBox;
                string roomsCountString = roomsCountTextBox.Text;
                string stepsCountString = StepsCountTextBox.Text;
                
                bool success = int.TryParse(roomsCountString, out int roomsCount);
                success &= int.TryParse(stepsCountString, out int stepsCount);
                TextBlock debugTextBlock = this.FindName("DebugTextBlock") as TextBlock;
                if (!success)
                {
                    debugTextBlock.Text = "Need to be numbers";
                } else
                {
                    debugTextBlock.Text = "" + roomsCount + stepsCount;
                }
            };

        }

        private async void TaskLogic()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                
            });
        }
    }
}
