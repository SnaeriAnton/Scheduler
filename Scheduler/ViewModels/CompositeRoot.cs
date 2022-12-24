using Scheduler.Models;
using Scheduler.Services;
using System;
using System.Collections;
using System.ComponentModel;

namespace Scheduler.ViewModels
{
    class CompositeRoot
    {
        private FileIOService _fileIOService;
        private ToDoListManager _toDoListManager;

        public IEnumerable ToDoList => _toDoListManager.ToDoList;

        public event Action<bool, string> ErrorOccurred;
        public event Action<string> ChangedTime;

        public CompositeRoot()
        {
            _toDoListManager = new ToDoListManager();
            _fileIOService = new FileIOService();

            _toDoListManager.ErrorOccurred += OnErrorOccurred;
            _toDoListManager.ChangedTime += OnChangedTime;
        }

        public void LoadData()
        {
            try
            {
                _toDoListManager.SetList(_fileIOService.loadData());
            }
            catch (Exception exception)
            {
                OnErrorOccurred(false, exception.Message);
            }

            _toDoListManager.ChangeList(TodoDataListListChanged);
        }

        public void Close()
        {
            _toDoListManager.ErrorOccurred -= OnErrorOccurred;
            _toDoListManager.ChangedTime -= OnChangedTime;
            _toDoListManager.Stop(TodoDataListListChanged);
        }

        public void RemoveData(int index) => _toDoListManager.RemoveRecord(index);

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
    }
}
