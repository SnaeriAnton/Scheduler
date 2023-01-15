using System;
using System.Media;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Scheduler.Models
{
    public class Timer
    {
        protected int Hours = 0;
        protected int Minutes = 0;
        protected int Seconds = 0;
        protected int Dilay = 1000;
        protected DispatcherTimer DispatcherTimer;
        protected SoundPlayer Sound = new SoundPlayer();
        protected string Message;
        private string _time = "00:00:00";
        private int _maxLengthString = 5;
        private char _colon = ':';
        private int _maxHours = 23;
        private int _maxMinutesAndSeconds = 59;
        private string _oneZero = "0";
        private string _twoZero = "00";
        private int _maxLengthTimeSigment = 2;
        private int _offsetLengthIndex = 1;
        private bool _isPlay = false;
        private string _currentTime = "00:00:00"; 

        public event Action<string> ChangedTime;
        public virtual event Action<string> ErrorOccurred;
        public event Action<bool> ChangedStatus;
        public event Action<string> ChangedTimer;
        public virtual event Action<string> TimerIsOver;

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

                newValue += BranchValue(value, lastIndex, ref lastIndex, _maxHours, ref Hours) + _colon;
                newValue += BranchValue(value, lastIndex + _offsetLengthIndex, ref lastIndex, _maxMinutesAndSeconds, ref Minutes) + _colon;
                newValue += BranchValue(value, lastIndex + _offsetLengthIndex, ref lastIndex, _maxMinutesAndSeconds, ref Seconds);

                value = newValue;


                if (_time != value)
                    _time = value;

                ChangedTimer?.Invoke(_time);
            }
        }

        public Timer()
        {
            Message = "Время вышло!";
            Sound.SoundLocation = "chpok.wav";
            DispatcherTimer = new DispatcherTimer();
        }

        public virtual void Play()
        {
            Sound.Stop();
            Work(true);
        }

        public void Stop()
        {
            Sound.Stop();
            Work(false);
        }

        protected void Ticking()
        {
            try
            {
                DispatcherTimer.Interval -= TimeSpan.FromSeconds(1);
                Hours = DispatcherTimer.Interval.Hours;
                Minutes = DispatcherTimer.Interval.Minutes;
                Seconds = DispatcherTimer.Interval.Seconds;
                _currentTime = Hours + _colon.ToString() + Minutes + _colon.ToString() + Seconds;
                ChangedTime?.Invoke(_currentTime);
            }
            catch (Exception exception)
            {
                ErrorOccurred?.Invoke(exception.Message);
            }
        }

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

        protected virtual async void Work(bool value)
        {
            if (Hours == 0 && Minutes == 0 && Seconds == 0) //отрефакторить 
                return;

            if (value == true)
                _isPlay = value;
            else
                _isPlay = value;

            ChangedStatus?.Invoke(_isPlay);

            DispatcherTimer.Interval = new TimeSpan(Hours, Minutes, Seconds);

            while (DispatcherTimer.Interval.TotalSeconds > 0 && _isPlay == true) 
            {
                await Task.Delay(Dilay);

                if (_isPlay == false)
                    return;

                Ticking();
            }

            if (Hours == 0 && Minutes == 0 && Seconds == 0)
            {
                Sound.Play();
                _isPlay = false;
                ChangedStatus?.Invoke(_isPlay);
                TimerIsOver?.Invoke(Message);
            }

            ChangedTime?.Invoke(_currentTime);
            Time = _currentTime;
        }
    }
}
