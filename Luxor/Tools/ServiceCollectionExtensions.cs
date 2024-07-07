using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luxor.Services;
using Luxor.Settings;
using Luxor.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Luxor.Tools
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCommonServices(this IServiceCollection collection)
        {
            collection.AddTransient<ILuxorServices,LuxorServices>();
            collection.AddTransient<MainViewModel>();
            collection.AddTransient<SettingsViewModel>();
        }
    }
}
