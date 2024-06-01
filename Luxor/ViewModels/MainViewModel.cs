using System;
using System.Reactive.Linq;
using System.Windows.Input;
using Luxor.Services;
using ReactiveUI;

namespace Luxor.ViewModels;

public class MainViewModel : ViewModelBase
{
    bool _switchBrightness = false;

    private readonly IBrightnessServicesController _brightnessServicesController;
    public ICommand BrightnessCommand { get; }
    public ICommand SettingsCommand { get; }

    private int _brightness;
    public int Brightness
    {
        get => _brightness;
        set => this.RaiseAndSetIfChanged(ref _brightness, value);
    }


    public MainViewModel(IBrightnessServicesController brightnessServicesController) 
    {
        _brightnessServicesController = brightnessServicesController;

        this.WhenAnyValue(x => x.Brightness)
        .Throttle(TimeSpan.FromMilliseconds(50)) // Throttle to avoid too many updates
        .Subscribe(brightness => _brightnessServicesController.SetMonitorBrightness(brightness));

        BrightnessCommand = ReactiveCommand.Create(() =>
        {
            Brightness = 100;
        });

        SettingsCommand = ReactiveCommand.Create(() =>
        {
        });
    }
}
