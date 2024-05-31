using System.Windows.Input;
using ReactiveUI;

namespace Luxor.ViewModels;

public class MainViewModel : ViewModelBase
{
    bool _switchBrightness = false;
    public ICommand BrightnessCommand { get; }
    public ICommand SettingsCommand { get; }



    public MainViewModel() 
    {
        BrightnessCommand = ReactiveCommand.Create(() =>
        {
            _switchBrightness = !_switchBrightness;
        });

        SettingsCommand = ReactiveCommand.Create(() =>
        {
        });
    }
}
