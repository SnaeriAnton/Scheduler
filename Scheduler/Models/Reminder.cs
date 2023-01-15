using System;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Scheduler.Models
{
    public class Reminder : Timer 
    {
        private int _timerReminder = 1;

        public override event Action<string> TimerIsOver;

        public Reminder()
        {
            Message = "Вернись к работе";
            Sound.SoundLocation = "alarm.wav";
            DispatcherTimer = new DispatcherTimer();
        }

        public override void Play()
        {
            Sound.Stop();
            Hours = _timerReminder;
            DispatcherTimer.Interval = new TimeSpan(Hours, Minutes, Seconds);
            Work(true);
        }

        protected override async void Work(bool value)
        {
            if (DispatcherTimer.Interval.TotalSeconds == 0)
                return;

            while (DispatcherTimer.Interval.TotalSeconds > 0 && value == true)
            {
                await Task.Delay(Dilay);

                if (value == false)
                    return;

                Ticking(); 
            }

            if (Hours == 0 && Minutes == 0 && Seconds == 0)
            {
                Sound.PlayLooping();
                TimerIsOver?.Invoke(Message);
                return;
            }

            return;
        }
    }
}
