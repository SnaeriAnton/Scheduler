using System;
using System.Collections;
using System.ComponentModel;
using System.Windows;

namespace Scheduler.Models
{
    class TaskModelManager
    {
        private BindingList<TaskModel> _taskDataList;
        private Reminder _reminder;
        private string _emptyLine = " ";

        public event Action<string> ChangedTime;
        public event Action<bool, string> ErrorOccurred;
        public event Action<bool> ChangedStatusTimer;
        public event Action<bool> ChangedStatus;
        public event Action<string> TimerIsOver;
        public event Action<string> ReminderIsOver;

        public IEnumerable Tasks => _taskDataList;
        public int Count => _taskDataList.Count;

        public TaskModelManager()
        {
            _reminder = new Reminder();
        }

        public void SetList(BindingList<TaskModel> toDoList)
        {
            _taskDataList = toDoList;

            Subscride();

            _reminder.TimerIsOver += OnReminderIsOver;
            _reminder.ErrorOccurred += OnErrorOccurred;
        }

        public void StartReminder() => _reminder.Play();

        public void RemoveRecord(int index)
        {
            if (index < _taskDataList.Count && index >= 0)
            {
                Timer timer = _taskDataList[index].Timer;
                timer.ChangedTime -= OnChange;
                timer.ErrorOccurred -= OnErrorOccurred;
                timer.ChangedStatus -= OnChangedStatusTimer;
                timer.TimerIsOver -= OnTimerIsOver;
                timer.Stop();
                _taskDataList.RemoveAt(index);
            }
        }

        public void ChangeList(Action<object, ListChangedEventArgs> methodChanged)
        {
            _taskDataList.ListChanged += methodChanged.Invoke;
            _taskDataList.ListChanged += OnChangedList;
        }

        public void Stop(Action<object, ListChangedEventArgs> method)
        {
            StopAllTimers();

            _reminder.Stop();
            _reminder.TimerIsOver -= OnReminderIsOver;
            _reminder.ErrorOccurred -= OnErrorOccurred;

            _taskDataList.ListChanged -= method.Invoke;
            _taskDataList.ListChanged -= OnChangedList;

            foreach (var task in _taskDataList)
                task.Close();

            Unsubscride();
        }

        public void RunTimer(int indexTask)
        {
            CheakWorkTimers(indexTask);

            if (indexTask < _taskDataList.Count && indexTask >= 0)
            {
                Timer timer = _taskDataList[indexTask].Timer;

                if (timer.Status == false)
                    timer.Play();
                else
                    timer.Stop();
            }
        }

        public string GetTaskText()
        {
            if (_taskDataList == null)
                return _emptyLine;

            foreach (var task in _taskDataList)
                if (task.Timer.Status == true)
                    return task.Task;
            
            return _emptyLine;
        }

        public bool CheсkStatusTask(int indexTask)
        {
            if (indexTask > _taskDataList.Count - 1 || indexTask < 0)
                return false;

            TaskModel task = _taskDataList[indexTask];

            if (task.Status == true || task.Time == "00:00:00")
                return false;

            return true;
        }

        private void StopAllTimers()
        {
            foreach (var task in _taskDataList)
                if (task.Timer.Status == true)
                    task.Timer.Stop();
        } 

        private void CheakWorkTimers(int indexTask)
        {
            foreach (var task in _taskDataList)
                if (task.Timer.Status == true)
                    if (task != _taskDataList[indexTask])
                        task.Timer.Stop();
        }

        private void Subscride()
        {
            foreach (var task in _taskDataList)
            {
                task.ChangedStatus += OnChangedStatus;
                task.Timer.ChangedTime += OnChange;
                task.Timer.ChangedStatus += OnChangedStatusTimer;
                task.Timer.ErrorOccurred += OnErrorOccurred;
                task.Timer.TimerIsOver += OnTimerIsOver;
            }
        }

        private void Unsubscride()
        {
            foreach (var task in _taskDataList)
            {
                task.ChangedStatus -= OnChangedStatus;
                task.Timer.ChangedTime -= OnChange;
                task.Timer.ChangedStatus -= OnChangedStatusTimer;
                task.Timer.ErrorOccurred -= OnErrorOccurred;
                task.Timer.TimerIsOver -= OnTimerIsOver;
            }
        }

        private void OnChangedStatus(bool value, Timer timer)
        {
            if (value == true)
                if (timer.Status == true)
                    timer.Stop();
            
            ChangedStatus?.Invoke(value);
        }

        private void OnChange(string time) => ChangedTime?.Invoke(time);

        private void OnErrorOccurred(string messgae) => ErrorOccurred?.Invoke(false, messgae);

        private void OnChangedStatusTimer(bool value) => ChangedStatusTimer?.Invoke(value);

        private void OnTimerIsOver(string message) => TimerIsOver?.Invoke(message);
        private void OnReminderIsOver(string message) => ReminderIsOver?.Invoke(message);

        private void OnChangedList(object sender, ListChangedEventArgs e)
        {
            Unsubscride();
            Subscride();
        }
    }
}
