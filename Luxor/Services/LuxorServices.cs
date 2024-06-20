using System;
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
using static Luxor.Controller.NativeMethods;

namespace Luxor.Services
{
    public class LuxorServices : ILuxorServices
    {
        private readonly UserSettings userSettings;

        public LuxorServices()
        {
            userSettings = new();
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
            IntPtr hdc = NativeMethods.GetDC(IntPtr.Zero);

            bool result = NativeMethods.GetDeviceGammaRamp(hdc, ref ramp);

            Console.WriteLine(ramp.Blue);

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

            IntPtr hdc = NativeMethods.GetDC(IntPtr.Zero);
            bool result = NativeMethods.SetDeviceGammaRamp(hdc, ref ramp);
            if (!result)
            {
                int errorCode = Marshal.GetLastWin32Error();
                // log the info, or try to catch the exception
            }
            NativeMethods.ReleaseDC(IntPtr.Zero, hdc);
        }

        public void SetSleepTime(TimeSpan sleepTime)
        {
            Debug.WriteLine($"SetSleepTime changed by {sleepTime}");
            // throw new NotImplementedException();
            userSettings.SleepTime = sleepTime;
        }

        public void SetWakeUpTime(TimeSpan wakeUpTime)
        {
            Debug.WriteLine($"SetWakeUpTime changed by {wakeUpTime}");
            // throw new NotImplementedException();
            userSettings.WakeUpTime = wakeUpTime;
        }
    }
}
