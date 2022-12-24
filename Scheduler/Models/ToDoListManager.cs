using System;
using System.Collections;
using System.ComponentModel;

namespace Scheduler.Models
{
    class ToDoListManager
    {
        private BindingList<TodoModel> _todoDataList;

        public event Action<string> ChangedTime;
        public event Action<bool, string> ErrorOccurred;

        public IEnumerable ToDoList => _todoDataList;
        public int Count => _todoDataList.Count;

        public void SetList(BindingList<TodoModel> toDoList)
        {
            _todoDataList = toDoList;

            Subscride();
        }

        public void RemoveRecord(int index)
        {
            if (index < _todoDataList.Count)
            {
                _todoDataList[index].Timer.ChangedTime -= OnChange;
                _todoDataList[index].Timer.ErrorOccurred -= OnErrorOccurred;
                _todoDataList.RemoveAt(index);
            }
        }        

        public void ChangeList(Action<object, ListChangedEventArgs> methodChanged)
        {
            _todoDataList.ListChanged += methodChanged.Invoke;
            _todoDataList.ListChanged += OnChangedList;
        }

        public void Stop(Action<object, ListChangedEventArgs> method) 
        {
            _todoDataList.ListChanged -= method.Invoke;
            _todoDataList.ListChanged -= OnChangedList;

            Unsubscride();
        }
        private void Subscride()
        {
            foreach (var toDo in _todoDataList)
            {
                toDo.Timer.ChangedTime += OnChange;
                toDo.Timer.ErrorOccurred += OnErrorOccurred;
            }
        }

        private void Unsubscride()
        {
            foreach (var toDo in _todoDataList)
            {
                toDo.Timer.ChangedTime -= OnChange;
                toDo.Timer.ErrorOccurred -= OnErrorOccurred;
            }
        }

        private void OnChange(string time) => ChangedTime?.Invoke(time);

        private void OnErrorOccurred(string messgae) => ErrorOccurred?.Invoke(false, messgae);

        private void OnChangedList(object sender, ListChangedEventArgs e)
        {
            Unsubscride();
            Subscride();
        }
    }
}
