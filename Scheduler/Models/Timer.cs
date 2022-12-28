using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Scheduler.Models
{
    public class Timer
    {
        private DispatcherTimer _timer;
        private string _time = "00:00";
        private int _maxLengthString = 5;
        private char _colon = ':';
        private int _maxHours = 23;
        private int _maxMinutesAndSeconds = 59;
        private string _oneZero = "0";
        private string _twoZero = "00";
        private int _maxLengthTimeSigment = 2;
        private int _offsetLengthIndex = 1;
        private bool _isPlay = false;
        private int _dilay = 1000;
        private int _hours = 0;
        private int _minutes = 0;
        private int _seconds = 0;
        private string _currentTime = "00:00:00"; 
        private string _message = "Timer is over!";

        public event Action<string> ChangedTime;
        public event Action<string> ErrorOccurred;
        public event Action<bool> ChangedStatus;
        public event Action<string> ChangedTimer;
        public event Action<string> TimerIsOver;

        public bool Status => _isPlay;

        public string Time
        {
            get { return _time; }
            set
            {
                if (value.Contains(_colon.ToString()) == false)
                    return;

                for (int i = 0; i < value.Length; i++)
                    if (value[i] != _colon)
                        if (int.TryParse(value[i].ToString(), out int _) == false)
                            return;

                int countColons = 0;
                if (value.Length > _maxLengthString)
                {
                    for (int i = 0; i < value.Length; i++)
                        if (value[i] == _colon)
                            countColons++;

                    if (countColons == 1)
                        return;
                }
                else
                    value += _colon.ToString() + _twoZero;

                string newValue = "";
                int lastIndex = 0;

                newValue += BranchValue(value, lastIndex, ref lastIndex, _maxHours, ref _hours) + _colon;
                newValue += BranchValue(value, lastIndex + _offsetLengthIndex, ref lastIndex, _maxMinutesAndSeconds, ref _minutes) + _colon;
                newValue += BranchValue(value, lastIndex + _offsetLengthIndex, ref lastIndex, _maxMinutesAndSeconds, ref _seconds);

                value = newValue;


                if (_time != value)
                    _time = value;

                ChangedTimer?.Invoke(_time);
            }
        }

        public Timer()
        {
            _timer = new DispatcherTimer();
        }

        public void Play() => Work(true);

        public void Stop() => Work(false);

        private string BranchValue(string value, int index, ref int LastIndex, int checkNumber, ref int unitTime)
        {
            string str = "";

            for (int i = index; i < value.Length; i++)
            {
                if (value[i] == _colon)
                {
                    LastIndex = i;
                    break;
                }

                str += value[i];
            }

            CheckValidateNumber(ref str, ref unitTime, checkNumber);
            return AddMissingSigns(str);
        }

        private string AddMissingSigns(string strNumber)
        {
            if (strNumber.Length == 0)
            {
                string intermediateResult = _twoZero;
                return intermediateResult;
            }
            else if (strNumber.Length != _maxLengthTimeSigment)
            {
                string intermediateResult = _oneZero + strNumber;
                return intermediateResult;
            }
            else
                return strNumber;
        }

        private void CheckValidateNumber(ref string strNumber, ref int unitTime, int checkNumber)
        {
            if (strNumber.Length != 0)
            {
                int number = int.Parse(strNumber);

                if (number > checkNumber)
                {
                    unitTime = checkNumber;
                    strNumber = unitTime.ToString();
                }
                unitTime = number;
            }
        }

        private async void Work(bool value)
        {
            if (_hours == 0 && _minutes == 0 && _seconds == 0) //отрефакторить 
                return;

            if (value == true)
                _isPlay = value;
            else
                _isPlay = value;

            ChangedStatus?.Invoke(_isPlay);

            _timer.Interval = new TimeSpan(_hours, _minutes, _seconds);

            while (_timer.Interval.TotalSeconds > 0 && _isPlay == true) 
            {
                await Task.Delay(_dilay);

                if (_isPlay == false)
                    return;

                try
                {
                    _timer.Interval -= TimeSpan.FromSeconds(1);
                    _hours = _timer.Interval.Hours;
                    _minutes = _timer.Interval.Minutes;
                    _seconds = _timer.Interval.Seconds;
                    _currentTime = _hours + _colon.ToString() + _minutes + _colon.ToString() + _seconds;
                    ChangedTime?.Invoke(_currentTime);
                }
                catch (Exception exception)
                {
                    ErrorOccurred?.Invoke(exception.Message);
                }
            }

            if (_hours == 0 && _minutes == 0 && _seconds == 0)
            {
                _isPlay = false;
                ChangedStatus?.Invoke(_isPlay);
                TimerIsOver?.Invoke(_message);
            }

            ChangedTime?.Invoke(_currentTime);
            Time = _currentTime;
        }
    }
}
