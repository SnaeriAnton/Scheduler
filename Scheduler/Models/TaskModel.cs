using Newtonsoft.Json;
using System;

namespace Scheduler.Models
{
    class TaskModel : Notifier
    {
        [JsonProperty(PropertyName = "creationDate")]
        public DateTime CreationDate { get; private set; } = DateTime.Now;

        private bool _status;
        private string _task;
        private Timer _timer = new Timer();

        public Timer Timer => _timer;
        public event Action<bool, Timer> ChangedStatus;

        [JsonProperty(PropertyName = "status")]
        public bool Status
        {
            get { return _status; }

            set
            {
                if (_status != value)
                    _status = value;

                ChangedStatus?.Invoke(value, _timer);
                NotifyPropertyChanged("Status");
            }
        }

        [JsonProperty(PropertyName = "task")]
        public string Task
        {
            get { return _task; }

            set
            {
                if (_task != value)
                    _task = value;

                NotifyPropertyChanged("Task");
            }
        }

        [JsonProperty(PropertyName = "time")]
        public string Time
        {
            get { return _timer.Time; }
            set
            {
                if (_timer.Time != value)
                    _timer.Time = value;

                NotifyPropertyChanged("Time");
            }
        }

        public TaskModel()
        {
            _timer.ChangedTimer += OnChangedTimer;
        }

        public void Close()
        {
            _timer.ChangedTimer -= OnChangedTimer;
        }

        private void OnChangedTimer(string time) => Time = time;
    }
}
