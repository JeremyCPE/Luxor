using Avalonia.Controls;
using Avalonia.Threading;
using System;

namespace Luxor.Views;

public partial class MainView : UserControl
{
    private DispatcherTimer _timer;

    public MainView()
    {
        InitializeComponent();

        // Initialize the timer
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        _timer.Tick += Timer_Tick;
        _timer.Start();

        // Set the initial time
        TimeTextBlock.Text = DateTime.Now.ToString("HH:mm:ss");
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
        // Update the TextBlock with the current time
        TimeTextBlock.Text = DateTime.Now.ToString("HH:mm:ss");
    }
}
