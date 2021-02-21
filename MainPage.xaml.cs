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

namespace Hotels
{

    public sealed partial class MainPage : Page
    {
        private int timesClicked = 0;

        public MainPage()
        {
            this.InitializeComponent();

            var mainTextBlock = this.FindName("MainText") as TextBlock;
            mainTextBlock.Text = "Hello world";

            Button mainButton = this.FindName("MainButton") as Button;
            mainButton.Click += (sender, e) =>
            {
                ChangeText();
            };

            Task.Run(new Action(TaskLogic));
        }

        private void ChangeText()
        {
            var mainTextBlock = this.FindName("MainText") as TextBlock;
            mainTextBlock.Text = "Clicked " + ++timesClicked;
        }

        private async void TaskLogic()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                (FindName("MainText") as TextBlock).Text = "In task";
                while (true)
                {
                    await Task.Delay(3000, new CancellationTokenSource().Token);
                    ChangeText();
                }
            });
        }
    }
}
