﻿using System;
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

    private readonly ILuxorServices _luxorServices;
    public ICommand BrightnessCommand { get; }
    public ICommand GammaCommand { get; }
    public ICommand SettingsCommand { get; }
    public ICommand DashboardCommand { get; }

    private int _brightness;
    private int _gamma;
    private TimePicker _wakeUpTime = new TimePicker() { SelectedTime = new TimeSpan(8, 0, 0) };
    private TimePicker _sleepTime = new TimePicker() { SelectedTime = new TimeSpan(22, 15, 0) };
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
    
    public TimePicker WakeUpTime
    {
        get => _wakeUpTime;
        set
        {
            this.RaiseAndSetIfChanged(ref _wakeUpTime, value);
            _luxorServices.SetWakeUpTime(value);
        }
    }    
    public TimePicker SleepTime
    {
        get => _sleepTime;
        set
        {
            this.RaiseAndSetIfChanged(ref _sleepTime, value);
            _luxorServices.SetSleepTime(value);
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
        Brightness = _luxorServices.GetCurrentBrightness();

        this.WhenAnyValue(x => x.Brightness)
        .Throttle(TimeSpan.FromMilliseconds(50)) // Throttle to avoid too many updates
        .Subscribe(brightness => _luxorServices.SetMonitorBrightness(brightness));


        this.WhenAnyValue(x => x.Gamma)
        .Throttle(TimeSpan.FromMilliseconds(50)) // Throttle to avoid too many updates
        .Subscribe(blueReduction => _luxorServices.SetMonitorGamma(blueReduction, Brightness));

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
