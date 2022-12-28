using System;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Scheduler.Models
{
    public class Reminder
    {
        private int _hours = 0;
        private int _minutes = 0;
        private int _seconds = 0;
        private int _timerMinutes = 1;
        private int _dilay = 1000;
        private DispatcherTimer _timer;
        private string _message = "Вернись к работе";

        public event Action<string> ErrorOccurred;
        public event Action<string> TimerIsOver;

        public Reminder()
        {
            _timer = new DispatcherTimer();
        }

        public void Play()
        {
            _timer.Interval = new TimeSpan(_hours, _timerMinutes, _seconds); // не работает
            Work(true);
        }

        public void Stop() => Work(false);

        private async void Work(bool value)
        {
            if (_hours == 0 && _minutes == 0 && _seconds == 0)
                return;

            while (_timer.Interval.TotalSeconds > 0 && value == true)
            {
                await Task.Delay(_dilay);

                if (value == false)
                    return;

                try
                {
                    _timer.Interval -= TimeSpan.FromSeconds(1);
                    _hours = _timer.Interval.Hours;
                    _minutes = _timer.Interval.Minutes;
                    _seconds = _timer.Interval.Seconds;
                }
                catch (Exception exception)
                {
                    ErrorOccurred?.Invoke(exception.Message);
                }
            }

            if (_hours == 0 && _minutes == 0 && _seconds == 0)
                TimerIsOver?.Invoke(_message);
        }
    }
}
