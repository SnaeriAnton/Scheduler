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
        private string _timer = "00:00";
        private string _timerTemplate = "00:00";
        private int _indexColon = 2;
        private char _zero = '0';
        private char _colon = ':';

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
        public string Timer
        {
            get { return _timer; }
            set
            {

                if (value.Contains(":") == false)
                    return;

                if (value.Length > 5)
                    return;

                for (int i = 0; i < value.Length; i++)
                    if (value[i] != ':')
                        if (int.TryParse(value[i].ToString(), out int _) == false)
                            return;

                string[] newValue = { "00:00" };
                int indexColon = 0;

                if (value.Length != 5)
                {
                    for (int i = 0; i < value.Length; i++)
                    {
                        if (value[i] == ':')
                            indexColon = i;
                    }

                    //for (int i = indexColon; i > 0; i--)
                    //{
                    //    newValue[i] = value[i].ToString();
                    //}


                    for (int i = 0; i < 5; i++)
                    {
                        newValue[i] = _zero.ToString();
                        newValue[i] = value[i].ToString();
                        newValue[i] = _zero.ToString();
                    }

                    value = newValue.ToString();
                }


                if (_timer != value)
                    _timer = value;

                NotifyPropertyChanged("Timer");
            }
        }
    }
}
