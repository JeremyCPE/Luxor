using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Luxor.Services
{
    public class Cycle
    {
        ILuxorServices _luxorServices;
        public Cycle(ILuxorServices IluxorServices) 
        {
            _luxorServices = IluxorServices;
        }
        public int TransitionInSec { get; set; } = 5;
        public State State { get; set; } = State.Stop;

        public bool CanProcess { get; set; } = true;

        public async Task Start()
        {
            if (this.State != State.Pause && this.State != State.Stop)
            {
                Debug.WriteLine("Cycle already started");
                return;
            }
            this.State = State.Running;

            var periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(this.TransitionInSec));
            while (this.State == State.Running && CanProcess)
            {
                while (await periodicTimer.WaitForNextTickAsync())
                {
                    Do();
                }
            }
            
        }

        public void Do()
        {
            if (this.State != State.Running)
            {
                Debug.WriteLine("Process cannot be done if cycle hasn't started");
                return;
            }
            this.State = State.Processing;
            if(!_luxorServices.Process())
            {
                Debug.WriteLine("Error during the process");
            }
            this.State = State.Running;
        }
    }



    public enum State
    {
        Running,
        Pause,
        Stop,
        Processing,
    }
}
