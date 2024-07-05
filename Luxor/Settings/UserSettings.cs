using System;

namespace Luxor.Settings
{
    public class UserSettings
    {
        public TimeSpan SleepTime { get; set; } = new(21, 0, 0);
        public TimeSpan WakeUpTime { get; set; } = new(8, 0, 0);

        public bool IsDisabled { get; set; } = false;
        public UserSettings() 
        { 
      
        }
    }
}