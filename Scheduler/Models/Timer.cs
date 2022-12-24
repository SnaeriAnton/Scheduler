using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Scheduler.Models
{
    class Timer 
    {
        private DispatcherTimer _timer;
        private string _time = "00:00";
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

        public event Action<string> ChangedTime;
        public event Action<string> ErrorOccurred;
        public event Action<bool> ChangedStatus;

        public string Time
        {
            get { return _time; }
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

                if (_time != value)
                    _time = value;
            }
        }

        public Timer()
        {
            _timer = new DispatcherTimer();
            SetTime();
        }

        private void SetTime()
        {
            // дописать парсер времени из Time
            _timer.Interval = new TimeSpan(0, 1, 05);
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

        public void Play()
        {
            if (_isPlay == true)
                return;

            _isPlay = true;
            Work();
        }

        public void Stop()
        {
            if (_isPlay == false)
                return;

            _isPlay = false;
            Work();
        }

        private async void Work()
        {

            ChangedStatus?.Invoke(_isPlay); // дописмат ьпроверку и логику срабатывания события

            while (_timer.Interval.TotalSeconds > 0 && _isPlay == true)
            {
                await Task.Delay(_dilay);
                try
                {
                    _timer.Interval -= TimeSpan.FromSeconds(1);
                    Time = _timer.Interval.Hours + _colon.ToString() + _timer.Interval.Minutes + _colon.ToString() + _timer.Interval.Seconds;
                    ChangedTime?.Invoke(Time);
                }
                catch (Exception exception)
                {
                    ErrorOccurred?.Invoke(exception.Message);
                }
            }
        }
    }
}
