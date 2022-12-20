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
        private int _maxLengthString = 5;
        private char _colon = ':';
        private int _maxHours = 23;
        private int _maxMinutes = 59;
        private string _zeroOne = "0";
        private string _zeroTwo = "00";
        private int _maxLengthTimeSigment = 2;
        private int _offsetLengthIndex = 1;

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

                if (value.Contains(_colon.ToString()) == false)
                    return;

                if (value.Length > _maxLengthString)
                    return;

                for (int i = 0; i < value.Length; i++)
                    if (value[i] != _colon)
                        if (int.TryParse(value[i].ToString(), out int _) == false)
                            return;

                string newValue = "";
                int indexColon = 0;
                string str = "";

                for (int i = 0; i < value.Length; i++)
                    if (value[i] == _colon)
                        indexColon = i;

                for (int i = 0; i < value.Length; i++)
                {
                    if (value[i] == _colon)
                        break;

                    str += value[i];
                }

                CheckValidateNumber(ref str, _maxHours);
                newValue += AddMissingSigns(str);

                str = "";
                newValue += _colon;

                for (int i = indexColon + _offsetLengthIndex; i < value.Length; i++)
                    str += value[i];

                CheckValidateNumber(ref str, _maxMinutes);
                newValue += AddMissingSigns(str);

                value = newValue;

                if (_timer != value)
                    _timer = value;

                NotifyPropertyChanged("Timer");
            }
        }

        private string AddMissingSigns(string strNumber)
        {
            if (strNumber.Length == 0)
            {
                string intermediateResult = _zeroTwo;
                return intermediateResult;
            }
            else if (strNumber.Length != _maxLengthTimeSigment)
            {
                string intermediateResult = _zeroOne + strNumber;
                return intermediateResult;
            }
            else
                return strNumber;
        }

        private void CheckValidateNumber(ref string strNumber, int checkNumber)
        {
            if (strNumber.Length != 0)
            {
                int numberTwo = int.Parse(strNumber);

                if (numberTwo > checkNumber)
                    strNumber = checkNumber.ToString();
            }
        }
    }
}
