using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Luxor.Settings;
using ReactiveUI;

namespace Luxor.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {

        private UserSettingsServices _userSettingsServices;
        public ICommand SaveChangesButton { get; }


        public SettingsViewModel()
        {
            _userSettingsServices = new();
            SaveChangesButton = ReactiveCommand.Create(() =>
            {
                if(_userSettingsServices.Save())
                {
                }
            });
        }
    }
}
