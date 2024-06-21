using System;

namespace Luxor.Settings
{
    public class UserSettings
    {
        public static TimeSpan DefaultSleepTime = new(21, 0, 0);
        public static TimeSpan DefaultWakeUpTime = new(8, 0, 0);

        public TimeSpan SleepTime { get; set; }
        public TimeSpan WakeUpTime { get; set; }

        public bool IsAuto { get; set; } = true;
        public UserSettings() { 
        
            SleepTime = DefaultSleepTime;
            WakeUpTime = DefaultWakeUpTime;
        }
    }
}