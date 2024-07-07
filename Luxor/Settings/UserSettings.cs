using System;
using System.Collections.Generic;

namespace Luxor.Settings
{
    public class UserSettings
    {
        public TimeSpan SleepTime { get; set; } = new(21, 0, 0);
        public TimeSpan WakeUpTime { get; set; } = new(8, 0, 0);

        public bool IsDisabled { get; set; } = false;

        public bool IsSmoothTransitionEnabled { get; set; } = true;

        public List<string> WhiteList {  get; set; } = new();

        public bool RunAtStart { get; set; } = true;

        public string Path { get; set; } = """%AppData%\Local\Luxor""";

        public void Replace(UserSettings importSettings)
        {
            this.IsDisabled = importSettings.IsDisabled;
            this.WhiteList = importSettings.WhiteList;
            this.IsSmoothTransitionEnabled = importSettings.IsSmoothTransitionEnabled;
            this.SleepTime = importSettings.SleepTime;
            this.WakeUpTime = importSettings.WakeUpTime;
        }

        public UserSettings()
        {

        }
    }
}