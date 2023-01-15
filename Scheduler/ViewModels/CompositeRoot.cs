using Scheduler.Models;
using Scheduler.Services;
using System;
using System.Collections;
using System.ComponentModel;

namespace Scheduler.ViewModels
{
    public class CompositeRoot
    {
        private FileIOService _fileIOService;
        private TaskModelManager _taskModelManager;

        public IEnumerable Tasks => _taskModelManager.Tasks;

        public event Action<bool, string> ErrorOccurred;
        public event Action<string> ChangedTime;
        public event Action<bool> ChangedStatusTimer;
        public event Action<bool> ChangedStatus;
        public event Action<string> TimerIsOver;
        public event Action<string> ReminderIsOver;
        public event Action<bool> TaskClosed;

        public CompositeRoot()
        {
            _taskModelManager = new TaskModelManager();
            _fileIOService = new FileIOService();

            _taskModelManager.ErrorOccurred += OnErrorOccurred;
            _taskModelManager.ChangedTime += OnChangedTime;
            _taskModelManager.ChangedStatusTimer += OnChangedStatusTimer;
            _taskModelManager.TimerIsOver += OnTimerIsOver;
            _taskModelManager.ReminderIsOver += OnReminderIsOver;
            _taskModelManager.ChangedStatus += OnChangedStatus;
            _taskModelManager.StartReminder();
        }

        public void LoadData()
        {
            try
            {
                _taskModelManager.SetList(_fileIOService.loadData());
            }
            catch (Exception exception)
            {
                OnErrorOccurred(false, exception.Message);
            }

            _taskModelManager.ChangeList(TodoDataListListChanged);
        }

        public void Close()
        {
            _taskModelManager.ErrorOccurred -= OnErrorOccurred;
            _taskModelManager.ChangedTime -= OnChangedTime;
            _taskModelManager.ChangedStatusTimer -= OnChangedStatusTimer;
            _taskModelManager.TimerIsOver -= OnTimerIsOver;
            _taskModelManager.ReminderIsOver -= OnReminderIsOver;
            _taskModelManager.ChangedStatus -= OnChangedStatus;
            _taskModelManager.Stop(TodoDataListListChanged);
        }

        public string GetTaskText() => _taskModelManager.GetTaskText();

        public void RestartRemonder() => _taskModelManager.StartReminder();

        public void RunTimer(int indexTask) => _taskModelManager.RunTimer(indexTask);

        public void RemoveData(int indexTask) => _taskModelManager.RemoveRecord(indexTask);

        public bool CheсkStatusTask(int indexTask) => _taskModelManager.CheсkStatusTask(indexTask);

        private void TodoDataListListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded || e.ListChangedType == ListChangedType.ItemDeleted || e.ListChangedType == ListChangedType.ItemChanged)
            {
                try
                {
                    _fileIOService.SaveData(sender);
                }
                catch (Exception exception)
                {
                    OnErrorOccurred(false, exception.Message);
                }
            }
        }

        private void OnErrorOccurred(bool value, string message) => ErrorOccurred?.Invoke(false, message);

        private void OnChangedTime(string time) => ChangedTime?.Invoke(time);

        private void OnChangedStatusTimer(bool value) => ChangedStatusTimer?.Invoke(value);

        private void OnTimerIsOver(string message) => TimerIsOver?.Invoke(message);

        private void OnReminderIsOver(string message) => ReminderIsOver?.Invoke(message);

        private void OnChangedStatus(bool value) => ChangedStatus?.Invoke(value);
    }
}
