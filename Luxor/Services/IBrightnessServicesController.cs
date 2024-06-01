using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luxor.Services
{
    public interface IBrightnessServicesController
    {
        int GetCurrentBrightness();
        void SetMonitorBrightness(int brightness);
    }
}
