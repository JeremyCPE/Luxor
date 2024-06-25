using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;

namespace Luxor.Services
{
    public interface ILuxorServices
    {
        bool Process();

        int GetCurrentBrightness();
        void SetMonitorBrightness(int brightness);

        int GetMonitorGamma();

        void SetMonitorGamma(int gamma, int brightness);

        TimeSpan GetWakeUpTime();
        TimeSpan GetSleepTime();

        void SetWakeUpTime(TimeSpan wakeUpTime);
        void SetSleepTime(TimeSpan sleepTime);

        int CalculateTimePercentage(TimeSpan wakeUpTime, TimeSpan sleepTime,TimeSpan currentTime);

    }
}
