using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luxor.Settings
{
    public interface IUserSettingsServices
    {
        bool Import(string file);

        string Export();

        bool Save();
    }
}
