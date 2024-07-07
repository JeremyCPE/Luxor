using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using Luxor.Settings;
using ReactiveUI;

namespace Luxor.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public ICommand SaveChangesButton { get; }


        public SettingsViewModel()
        {
            SaveChangesButton = ReactiveCommand.Create(() =>
            {
               // _settingsServices.Save();
            });
        }
    }
}
