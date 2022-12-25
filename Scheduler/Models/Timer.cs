using System;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Scheduler.Models
{
    class Timer
    {
        private DispatcherTimer _timer;
        private string _time = "00:00";
        private string _tamplateFullTime = "00:00:00";
        private int _maxLengthString = 5;
        private char _colon = ':';
        private int _maxHours = 23;
        private int _maxMinutes = 59;
        private string _zeroOne = "0";
        private string _zeroTwo = "00";
        private int _maxLengthTimeSigment = 2;
        private int _offsetLengthIndex = 1;
        private bool _isPlay = false;
        private int _dilay = 1000;
        private int _hours = 0;
        private int _minutes = 0;
        private int _seconds = 0;
        private string _currentTime = "00:00:00";
        private int _colonPositionOnFullTime = 2;
        private string _message = "Timer is over!";

        public event Action<string> ChangedTime;
        public event Action<string> ErrorOccurred;
        public event Action<bool> ChangedStatus;
        public event Action<string> ChangedTimer;
        public event Action<string> ChangedFullTime;
        public event Action<string> TimerIsOver;

        public bool Status => _isPlay;
        public string CurrecntTime // переписать логику проверки объеденить currentTime с Time 
        {
            get { return _currentTime; }
            set
            {
                if (value.Contains(_colon.ToString()) == false) { }
                    _currentTime = _tamplateFullTime;

                if (_currentTime != value)
                    _currentTime = value;

                SetSeconds(_currentTime);
            }
        }

        public string Time //при изменение времени обнулять секунды
        {
            get { return _time; }
            set
            {
                if (_isPlay == true)
                    return;

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

                CheckValidateNumber(ref str, ref _hours, _maxHours);
                newValue += AddMissingSigns(str);

                str = "";
                newValue += _colon;

                for (int i = indexColon + _offsetLengthIndex; i < value.Length; i++)
                    str += value[i];

                CheckValidateNumber(ref str, ref _minutes, _maxMinutes);
                newValue += AddMissingSigns(str);

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

        private void ResetSeconds() //при изменение времени обнулять секунды
        {
            for (int i = 0; i < _time.Length; i++)
            {
                if (_time[i] != CurrecntTime[i])
                {
                    ChangedFullTime?.Invoke(_currentTime);
                    return;
                }
            }
        }

        private void SetSeconds(string strTime)
        {
            string strSeconds = "";
            int counColon = 0;

            for (int i = 0; i < strTime.Length; i++) 
            {
                if (counColon == _colonPositionOnFullTime)
                    strSeconds += strTime[i];
                else if (strTime[i] == _colon)
                    counColon++;
            }

            _seconds = int.Parse(strSeconds);
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
            if (_hours == 0 && _minutes == 0 && _seconds == 0)
                return;

            if (value == true)
                _isPlay = value;
            else
                _isPlay = value;

            ChangedStatus?.Invoke(_isPlay);

            _timer.Interval = new TimeSpan(_hours, _minutes, _seconds);

            while (_timer.Interval.TotalSeconds > 0 && _isPlay == true) // подумать как мгновенно остановить таймер
            {
                await Task.Delay(_dilay);
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
            ChangedFullTime?.Invoke(_currentTime);
            Time = CutSeconds(_currentTime);
        }

        private string CutSeconds(string time)
        {
            string newString = "";
            bool isColon = false;

            for (int i = 0; i < time.Length; i++) 
            {
                if (time[i] == _colon)
                {
                    if (isColon == true)
                        break;

                    isColon = true;
                }

                newString += time[i].ToString();
            }
            return newString;
        }
    }
}
