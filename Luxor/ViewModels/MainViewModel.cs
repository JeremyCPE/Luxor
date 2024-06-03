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
    public ICommand GammaCommand { get; }
    public ICommand SettingsCommand { get; }

    private int _brightness;
    private int _gamma;
    public int Brightness
    {
        get => _brightness;
        set => this.RaiseAndSetIfChanged(ref _brightness, value);
    }

    public int Gamma
    {
        get => _gamma;
        set => this.RaiseAndSetIfChanged(ref _gamma, value);
    }

    public MainViewModel()
    {

    }

    public MainViewModel(IBrightnessServicesController brightnessServicesController) 
    {
        _brightnessServicesController = brightnessServicesController;
        Brightness = _brightnessServicesController.GetCurrentBrightness();

        this.WhenAnyValue(x => x.Brightness)
        .Throttle(TimeSpan.FromMilliseconds(50)) // Throttle to avoid too many updates
        .Subscribe(brightness => _brightnessServicesController.SetMonitorBrightness(brightness));


        this.WhenAnyValue(x => x.Gamma)
        .Throttle(TimeSpan.FromMilliseconds(50)) // Throttle to avoid too many updates
        .Subscribe(gamma => _brightnessServicesController.SetMonitorGamma(gamma, Brightness));

        BrightnessCommand = ReactiveCommand.Create(() =>
        {
            Brightness = 100;
        });

        GammaCommand = ReactiveCommand.Create(() =>
        {
            Gamma = 100;
        });

        SettingsCommand = ReactiveCommand.Create(() =>
        {
        });
    }
}
