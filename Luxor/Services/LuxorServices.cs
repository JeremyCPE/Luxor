﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Logging;
using Luxor.Controller;
using Luxor.Settings;
using Luxor.ViewModels;
using static Luxor.Controller.NativeMethods;

namespace Luxor.Services
{
    public class LuxorServices : ILuxorServices
    {
        private readonly UserSettings _userSettings;

        private readonly Cycle _cycle;

        public LuxorServices()
        {
            _userSettings = new();
            _cycle = new(this);
            Task.Run(() => _cycle.Start());
        }

        public bool Process()
        {
            Debug.WriteLine($"Process executed ! {_cycle}");
            AdaptGamma();
            return true;
        }

        /// <summary>
        /// Adapt the gamma depending of the WakeUpTime and SleepTime. What we want : blue reduction 100% when sleepTime = currentTime
        /// </summary>
        private void AdaptGamma()
        {
            if(_userSettings.IsDisabled)
            {
                return;
            }
            TimeSpan currentTime = DateTime.Now.TimeOfDay;
            var percent = CalculateTimePercentage(_userSettings.WakeUpTime,_userSettings.SleepTime, currentTime);

            Debug.WriteLine($"Reduction of % {percent} blueReduction...");
            SetMonitorGamma(percent, GetCurrentBrightness());
        }

        public int GetCurrentBrightness()
        {
            return NativeMethods.GetCurrentBrightness();
        }

        public int GetMonitorGamma()
        {
            NativeMethods.RAMP ramp = new NativeMethods.RAMP
            {
                Red = new ushort[256],
                Green = new ushort[256],
                Blue = new ushort[256]
            };
            try
            {
                IntPtr hdc = NativeMethods.GetDC(IntPtr.Zero);

                bool result = NativeMethods.GetDeviceGammaRamp(hdc, ref ramp);
            }

            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to getMonitorGamma, please verify the settings of your laptop {ex.Message}");
            }
            return 0;
        }

        public void SetMonitorBrightness(int brightness)
        {
            if (brightness < 0 || brightness > 100)
            {
                throw new ArgumentOutOfRangeException("Brightness should be between 0 and 100");
            }
            try
            {
                NativeMethods.SetMonitorBrightness(brightness);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to set monitor brightness: {brightness}, please verify the settings of your laptop {ex.Message}");
            }
        }

        public void SetMonitorGamma(int blueReduction, int brightness)
        {
            if (blueReduction < 0 || blueReduction > 100)
                throw new ArgumentOutOfRangeException("Blue reduction must be between 0 and 100."); // TODO : Manage those exception

            if (brightness < 0 || brightness > 100)
                throw new ArgumentOutOfRangeException("Brightness must be between 0 and 100.");

            NativeMethods.RAMP ramp = new NativeMethods.RAMP
            {
                Red = new ushort[256],
                Green = new ushort[256],
                Blue = new ushort[256]
            };

            // Calculate the ramp values
            for (int i = 0; i < 256; i++)
            {
                int value = i * (brightness + 128);
                if (value > 65535) value = 65535;

                ramp.Red[i] = ramp.Green[i] = (ushort)value;

                // Reduce blue light
                int blueValue = (int)(value * (1.0 - blueReduction / 100.0));
                if (blueValue > 65535) blueValue = 65535;
                ramp.Blue[i] = (ushort)blueValue;
            }
            try
            {
                IntPtr hdc = NativeMethods.GetDC(IntPtr.Zero);
                bool result = NativeMethods.SetDeviceGammaRamp(hdc, ref ramp);
                if (!result)
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    // log the info, or try to catch the exception
                }
                NativeMethods.ReleaseDC(IntPtr.Zero, hdc);
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"Failed to setMonitor Gamma, please verify your computer, {ex.Message}");
            }
           }

        public TimeSpan GetSleepTime()
        {
            return _userSettings.SleepTime;
        }

        public TimeSpan GetWakeUpTime()
        {
            return _userSettings.WakeUpTime;
        }
        public void SetSleepTime(TimeSpan sleepTime)
        {
            Debug.WriteLine($"SetSleepTime changed by {sleepTime}"); // Todo : better logging
            if (sleepTime == _userSettings.WakeUpTime)
            {
                Debug.WriteLine("WakeUpTime cannot be the same than sleepTime");
                return;
            }
            _userSettings.SleepTime = sleepTime;
        }

        public void SetWakeUpTime(TimeSpan wakeUpTime)
        {
            Debug.WriteLine($"SetWakeUpTime changed by {wakeUpTime}");
            if (wakeUpTime == _userSettings.SleepTime)
            {
                Debug.WriteLine("WakeUpTime cannot be the same than sleepTime");
                return;
            }
            _userSettings.WakeUpTime = wakeUpTime;
        }


        public int CalculateTimePercentage(TimeSpan wakeUpTime, TimeSpan sleepTime, TimeSpan currentTime)
        {
            // Normalize sleepTime to handle cases where sleepTime is after midnight
            if (sleepTime < wakeUpTime)
            {
                sleepTime += TimeSpan.FromDays(1);
            }

            // Adjust current time to fall within the wake-sleep period
            if (currentTime < wakeUpTime)
            {
                currentTime += TimeSpan.FromDays(1);
            }

            // Calculate the total span and elapsed time
            TimeSpan totalSpan = (sleepTime - wakeUpTime).Duration();
            TimeSpan elapsedTime = currentTime - wakeUpTime;

            // Calculate the percentage
            double percentage = (elapsedTime.TotalMinutes / totalSpan.TotalMinutes) * 100;
            if(percentage < 0) percentage = 0;
            if(percentage >= 100) percentage = 100;

            return (int)Math.Ceiling(percentage);

        }

        public void SetIsDisabled(bool isDisabled)
        {
            if (isDisabled)
            {
                _cycle.Stop();
                SetMonitorGamma(0, GetCurrentBrightness());
            }
            else
            {
                Task.Run(() => _cycle.Start());
            }
            _userSettings.IsDisabled = isDisabled;
        }
    }
    }
