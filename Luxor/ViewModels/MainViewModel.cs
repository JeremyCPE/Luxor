using System;
using System.Reactive.Linq;
using System.Windows.Input;
using Avalonia.Controls;
using Luxor.Services;
using Luxor.Views;
using ReactiveUI;

namespace Luxor.ViewModels;

public class MainViewModel : ViewModelBase
{
    bool _switchBrightness = false;

    private readonly ILuxorServices _brightnessServicesController;
    public ICommand BrightnessCommand { get; }
    public ICommand GammaCommand { get; }
    public ICommand SettingsCommand { get; }
    public ICommand DashboardCommand { get; }

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

    public Interaction<SettingsViewModel, SettingsViewModel?> ShowDialog { get; }
    public Interaction<DashboardViewModel, DashboardViewModel?> ShowDashDialog { get; }

    public MainViewModel()
    {

    }

    public MainViewModel(ILuxorServices brightnessServicesController) 
    {
        ShowDialog = new Interaction<SettingsViewModel, SettingsViewModel?>();
        ShowDashDialog = new Interaction<DashboardViewModel, DashboardViewModel?>();

        _brightnessServicesController = brightnessServicesController;
        Brightness = _brightnessServicesController.GetCurrentBrightness();

        this.WhenAnyValue(x => x.Brightness)
        .Throttle(TimeSpan.FromMilliseconds(50)) // Throttle to avoid too many updates
        .Subscribe(brightness => _brightnessServicesController.SetMonitorBrightness(brightness));


        this.WhenAnyValue(x => x.Gamma)
        .Throttle(TimeSpan.FromMilliseconds(50)) // Throttle to avoid too many updates
        .Subscribe(blueReduction => _brightnessServicesController.SetMonitorGamma(blueReduction, Brightness));

        BrightnessCommand = ReactiveCommand.Create(() =>
        {
            Brightness = 100;
        });

        GammaCommand = ReactiveCommand.Create(() =>
        {
            Gamma = 100;
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
