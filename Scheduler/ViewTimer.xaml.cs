using System.Windows;

namespace Scheduler
{
    public partial class ViewTimer : Window
    {
        public ViewTimer() => InitializeComponent();

        public void ShowTime(string time) => timerView.Text = time;

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}
