using Scheduler.Models;
using Scheduler.Services;
using System;
using System.Collections;
using System.ComponentModel;
using System.Windows;

namespace Scheduler.ViewModels
{
    class CompositeRoot
    {
        private FileIOService _fileIOService;
        private ToDoListManager _toDoListManager;

        public IEnumerable ToDoList => _toDoListManager.ToDoList;

        public event Action<bool> LoadedData;
        public event Action<bool> SavedDat;

        public CompositeRoot()
        {
            _toDoListManager = new ToDoListManager();
            _fileIOService = new FileIOService();
        }

        public void LoadData()
        {
            try
            {
                _toDoListManager.SetList(_fileIOService.loadData());
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                LoadedData?.Invoke(false);
            }

            _toDoListManager.ChangeList(TodoDataListListChanged);
        }

        public void Close() => _toDoListManager.Stop(TodoDataListListChanged);

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
                    MessageBox.Show(exception.Message);
                    SavedDat?.Invoke(false);
                }
            }
        }
    }
}
