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

        public List<string> WhiteList {  get; set; }

        public bool RunAtStart { get; set; } = true;


        public UserSettings() 
        { 
      
        }
    }
}