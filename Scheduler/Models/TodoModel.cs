using Newtonsoft.Json;
using System;

namespace Scheduler.Models
{
    class TodoModel : Notifier
    {
        [JsonProperty(PropertyName = "creationDate")]
        public DateTime CreationDate { get; private set; } = DateTime.Now;

        private bool _status;
        private string _task;
        private float _timer;

        [JsonProperty(PropertyName = "status")]
        public bool Status
        {
            get { return _status; }

            set
            {
                if (_status != value)
                    _status = value;

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

        [JsonProperty(PropertyName = "timer")]
        public float Timer
        {
            get { return _timer; }
            set
            {
                if (value >= 0)
                    if (_timer != value)
                        _timer = value;

                NotifyPropertyChanged("Timer");
            }
        }
    }
}
