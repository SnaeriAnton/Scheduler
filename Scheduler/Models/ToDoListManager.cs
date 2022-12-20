using System;
using System.Collections;
using System.ComponentModel;

namespace Scheduler.Models
{
    class ToDoListManager
    {
        private BindingList<TodoModel> _todoDataList;

        public IEnumerable ToDoList => _todoDataList;
        public int Count => _todoDataList.Count;

        public void SetList(BindingList<TodoModel> toDoList) => _todoDataList = toDoList;

        public void RemoveRecord(int index)
        {
            if (index < _todoDataList.Count)
                _todoDataList.RemoveAt(index);
        }

        public void ChangeList(Action<object, ListChangedEventArgs> methodChanged) => _todoDataList.ListChanged += methodChanged.Invoke;

        public void Stop(Action<object, ListChangedEventArgs> method) => _todoDataList.ListChanged -= method.Invoke;
    }
}
