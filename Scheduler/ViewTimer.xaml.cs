using System.Windows;

namespace Scheduler
{
    public partial class ViewTimer : Window
    {
        public ViewTimer() => InitializeComponent();

        public void ShowTime(string time) => timerView.Text = time;

        public void ShowTask(string time) => textTask.Text = time;
    }
}
