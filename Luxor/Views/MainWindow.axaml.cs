using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Luxor.ViewModels;
using ReactiveUI;

namespace Luxor.Views;

public partial class MainWindow : ReactiveWindow<MainViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
        this.WhenActivated(action =>
    action(ViewModel!.ShowDialog.RegisterHandler(DoShowDialogAsync)));
    }

    private async Task DoShowDialogAsync(InteractionContext<SettingsViewModel,
                                        SettingsViewModel?> interaction)
    {
        var dialog = new Settings();
        dialog.DataContext = interaction.Input;

        var result = await dialog.ShowDialog<SettingsViewModel?>(this);
        interaction.SetOutput(result);
    }
}
