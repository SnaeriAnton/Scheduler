using System.ComponentModel;

namespace Scheduler.Models
{
    class Notifier : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged; //подумать как модернезировать что бы сделать более mvvm

        protected virtual void NotifyPropertyChanged(string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
