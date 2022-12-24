﻿using Newtonsoft.Json;
using System;

namespace Scheduler.Models
{
    class TodoModel : Notifier
    {
        [JsonProperty(PropertyName = "creationDate")]
        public DateTime CreationDate { get; private set; } = DateTime.Now;

        private bool _status;
        private string _task;
        private Timer _timer = new Timer();

        public Timer Timer => _timer;

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
    }
}
