using System;
using System.Reactive.Linq;
using System.Windows.Input;
using Avalonia.Controls;
using Luxor.Services;
using Luxor.Settings;
using Luxor.Views;
using ReactiveUI;

namespace Luxor.ViewModels;

public class MainViewModel : ViewModelBase
{
    bool _switchBrightness = false;

    private readonly ILuxorServices _luxorServices;
    public ICommand BrightnessCommand { get; }
    public ICommand GammaCommand { get; }
    public ICommand SettingsCommand { get; }
    public ICommand DashboardCommand { get; }


    private int _brightness;
    private int _gamma;
    private TimeSpan _wakeUpTime;
    private TimeSpan _sleepTime;
    private bool _isDisabled;

    // TODO : Need to use PropertyChange observable
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
    public TimeSpan WakeUpTime
    {
        get => _wakeUpTime;
        set
        {
            this.RaiseAndSetIfChanged(ref _wakeUpTime, value);
            _luxorServices.SetWakeUpTime(value);
        }
    }    
    public TimeSpan SleepTime
    {
        get => _sleepTime;
        set
        {
            this.RaiseAndSetIfChanged(ref _sleepTime, value);
            _luxorServices.SetSleepTime(value);
        }
    }

    public bool IsDisabled
    {
        get => _isDisabled;
        set
        {
            this.RaiseAndSetIfChanged(ref _isDisabled, value);
            _luxorServices.SetIsDisabled(value);
        }
    }

    public Interaction<SettingsViewModel, SettingsViewModel?> ShowDialog { get; }
    public Interaction<DashboardViewModel, DashboardViewModel?> ShowDashDialog { get; }

    public MainViewModel()
    {

    }

    public MainViewModel(ILuxorServices luxorServicesController) 
    {
        ShowDialog = new Interaction<SettingsViewModel, SettingsViewModel?>();
        ShowDashDialog = new Interaction<DashboardViewModel, DashboardViewModel?>();

        _luxorServices = luxorServicesController;

        _wakeUpTime = _luxorServices.GetWakeUpTime();
        _sleepTime = _luxorServices.GetSleepTime();
        Brightness = _luxorServices.GetCurrentBrightness();

        this.WhenAnyValue(x => x.Brightness)
        .Throttle(TimeSpan.FromMilliseconds(50)) // Throttle to avoid too many updates
        .Subscribe(brightness => _luxorServices.SetMonitorBrightness(brightness));


        this.WhenAnyValue(x => x.Gamma)
        .Throttle(TimeSpan.FromMilliseconds(50)) // Throttle to avoid too many updates
        .Subscribe(blueReduction => _luxorServices.SetMonitorGamma(blueReduction, Brightness));

        BrightnessCommand = ReactiveCommand.Create(() =>
        {
            if (Brightness == 100)
            {
                Brightness = 0;
            }
            else
            {
                Brightness = 100;
            }
        });

        GammaCommand = ReactiveCommand.Create(() =>
        {
            if(Gamma == 100)
            {
                Gamma = 0;
            }
            else
            {
                Gamma = 100; 
            }
        });


        SettingsCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var store = new SettingsViewModel();

            var result = await ShowDialog.Handle(store);
        });
        DashboardCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var store = new DashboardViewModel();

            var result = await ShowDashDialog.Handle(store);
        });

    }

}
