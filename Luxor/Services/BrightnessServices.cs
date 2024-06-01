using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luxor.Controller;

namespace Luxor.Services
{
    public class BrightnessServices : IBrightnessServicesController
    {
        public int GetCurrentBrightness()
        {
            // To implement
            return 0;
        }

        public void SetMonitorBrightness(int brightness)
        {
            if (brightness < 0 || brightness > 100)
            {
                throw new ArgumentOutOfRangeException("Brightness should be between 0 and 100");
            }
            NativeMethods.SetMonitorBrightness(brightness);
        }
    }
}
