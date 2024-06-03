using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luxor.Services
{
    public interface ILuxorServices
    {
        int GetCurrentBrightness();
        void SetMonitorBrightness(int brightness);

        int GetMonitorGamma();

        void SetMonitorGamma(int gamma, int brightness);
    }
}
